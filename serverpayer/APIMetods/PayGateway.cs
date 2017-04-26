using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using LogHelper;
using APIMetods.database;
using APIMetods.database.model;

namespace APIMetods
{
    public class PayGateway : IResponseGateway
    {
        enum StatusEnum
        {
            Ok = 1,
            BackTransaction,
            WrongData,
            CloseLimit,
            ValidationError
        }
        //ид банка
        private StatusEnum result;
        private readonly int vendorCard_id = 1;
        private int order_id;
        private string card_number;
        private int expiry_month;
        private int expiry_year;
        private int cvv;
        private decimal amount_kop;
        private string cardholder_name;
        private Dictionary<string, string> param;

        public PayGateway()
        {

        }

        private JObject GetResponseStatus(StatusEnum stat)
        {
            JObject response = new JObject();
            status statusObj = GatewayContext.statuses.Find(s=>s.status_id==(int)stat);
            response.Add("id", statusObj.status_id);
            response.Add("mesasage", statusObj.mesasage);
            return response;
        }
        private bool CheckValidationParams()
        {
            bool ValidationData = false;
            if (Regex.Match(param["card_number"], @"(\d\d\d\d\d\d\d\d\d\d\d\d\d\d\d\d)").Success)
            {
                ValidationData = true;
                this.card_number = param["card_number"];
            }
            if (Regex.Match(param["order_id"], @"(\d+)").Success && ValidationData)
            {
                ValidationData = int.TryParse(param["order_id"], out this.order_id);
            }
            else
            {
                ValidationData = false;
            }
            if (Regex.Match(param["expiry_month"], @"(\d|[\d\d])").Success && ValidationData)
            {
                if (ValidationData = int.TryParse(param["expiry_month"], out this.expiry_month) && ValidationData)
                {

                    ValidationData = this.expiry_month > 0 && this.expiry_month <= 12;

                }
            }
            else
            {
                ValidationData = false;
            }
            if (Regex.Match(param["expiry_year"], @"([\d\d\d\d])").Success && ValidationData)
            {
                if (ValidationData = int.TryParse(param["expiry_year"], out this.expiry_year) && ValidationData)
                {

                    ValidationData = this.expiry_year > 2000 && this.expiry_month <= 2100;

                }
            }
            else
            {
                ValidationData = false;
            }
            if (Regex.Match(param["cvv"], @"(\d\d\d)").Success && ValidationData)
            {
                ValidationData = int.TryParse(param["cvv"], out this.cvv);

            }
            else
            {
                ValidationData = false;
            }
            if (Regex.Match(param["amount_kop"], @"(\d+)").Success && ValidationData)
            {
                ValidationData = decimal.TryParse(param["amount_kop"], out this.amount_kop);
            }
            else
            {
                ValidationData = false;
            }

            this.cardholder_name = param["cardholder_name"];
            return ValidationData;

        }

        private void Pay()
        {
            card vendorCard = GatewayContext.cards.Find(c=>c.card_id== vendorCard_id);
            card consumerCard = GatewayContext.cards.Where(p => (p.card_number == card_number) &&
                                                (p.expiry_month == this.expiry_month) &&
                                                (p.expiry_year == this.expiry_year) &&
                                                (p.cvv == this.cvv) &&
                                                (cardholder_name != null ? p.cardholder_name == cardholder_name : true)).FirstOrDefault();
            if (consumerCard != null && vendorCard != null)
            {
                if (this.amount_kop > consumerCard.cash)
                {
                    result = StatusEnum.CloseLimit;
                }
                else
                {
                    result = StatusEnum.Ok;
                    consumerCard.cash -= this.amount_kop;
                    if (vendorCard.cash != null)
                    {
                        vendorCard.cash += this.amount_kop;
                    }

                }
            }
            else
            {
                result = StatusEnum.WrongData;
            }
            order order = new order()
            {
                status_id = (int)result,
                order_id = this.order_id,
                amount_kop = this.amount_kop
            };
            GatewayContext.orders.Add(order);
            card_order card_orderConsumer = new card_order()
            {
                card_id = consumerCard.card_id,
                order_id = this.order_id,
                isconsumer = true
            };
            GatewayContext.card_orders.Add(card_orderConsumer);
            card_order card_orderVendor = new card_order()
            {
                card_id = vendorCard.card_id,
                order_id = this.order_id,
                isconsumer = false
            };
            GatewayContext.card_orders.Add(card_orderVendor);

        }

        public JObject ResponseGateway(Dictionary<string, string> _param)
        {
            param = _param;
            if (CheckValidationParams())
            {
                Pay();
            }
            else
            {
                result = StatusEnum.ValidationError;
            }
            return GetResponseStatus((StatusEnum)result);
        }

    }
}
