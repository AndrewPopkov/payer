using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using APIMetods.database;
using APIMetods.database.model;
using System.Text.RegularExpressions;
using LogHelper;
using System.Linq;

namespace APIMetods
{
    public class RefundGateway : IResponseGateway
    {
        enum statusEnum
        {
            Ok=1,
            BackTransaction,
            WrongData,
            CloseLimit,
            ValidationError
        }
        //ид банка
        private JObject result;
        private readonly int vendorCard_id = 1;
        private int order_id;
        private Dictionary<string, string> param;

        public RefundGateway()
        {

        }

        private JObject GetResponseStatus(statusEnum stat)
        {
            JObject response = new JObject();
            status statusObj = GatewayContext.statuses.Find(s => s.status_id == (int)stat);
            response.Add("id", statusObj.status_id);
            response.Add("mesasage", statusObj.mesasage);
            return response;
        }

        private bool CheckValidationParams()
        {
            bool ValidationData = false;
            if (Regex.Match(param["order_id"], @"(\d+)").Success)
            {
                ValidationData = int.TryParse(param["order_id"], out this.order_id);
            }
            return ValidationData;
        }

        public void Refund()
        {

                        order order = GatewayContext.orders.Find(o=>o.order_id==order_id);
                        card consumer_card = null;
                        card vendorCard = null;
                        if (order != null)
                        {
                            IEnumerable<card_order> Card_order = GatewayContext.card_orders.Where(o=>o.order_id==this.order_id);
                            foreach (card_order obj in Card_order)
                            {
                                if (obj.isconsumer)
                                {
                                    consumer_card = GatewayContext.cards.Find(c=>c.card_id==obj.card_id);
                                }
                                else
                                {
                                    vendorCard = GatewayContext.cards.Find(c => c.card_id == obj.card_id);
                                }
                            }
                            if (consumer_card != null && vendorCard != null)
                            {
                                consumer_card.cash += order.amount_kop;
                                if (vendorCard.cash != null)
                                {
                                    vendorCard.cash -= order.amount_kop;
                                }
                                order.status_id = (int)statusEnum.BackTransaction;
                                result = GetResponseStatus(statusEnum.Ok);
                            }
                        }
                        else
                        {
                            result = GetResponseStatus(statusEnum.WrongData);
                        }

        }

        public JObject ResponseGateway(Dictionary<string, string> _param)
        {
            param = _param;
            if (CheckValidationParams())
            {
                Refund();
            }
            else
            {
                result = GetResponseStatus(statusEnum.ValidationError);
            }
            return result;
        }
    }
}
