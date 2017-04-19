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
            using (GatewayContext db = new GatewayContext())
            {
                status statusObj = db.statuses.Find((int)stat);              
                result.Add("id", statusObj.status_id);
                result.Add("mesasage", statusObj.mesasage);
            }
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
            using (GatewayContext db = new GatewayContext())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        order order = db.orders.Find(order_id);
                        card consumer_card = null;
                        card vendorCard = null;
                        if (order != null)
                        {
                            IEnumerable<card_order> Card_order = db.card_orders.Where(o=>o.order_id==this.order_id);
                            foreach (card_order obj in Card_order)
                            {
                                if (obj.isconsumer)
                                {
                                    consumer_card = db.cards.Find(obj.card_id);
                                }
                                else
                                {
                                    vendorCard = db.cards.Find(obj.card_id);
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
                                db.SaveChanges();
                                transaction.Commit();
                                result = GetResponseStatus(statusEnum.Ok);
                            }
                        }
                        else
                        {
                            result = GetResponseStatus(statusEnum.WrongData);
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Log.Write(ex);
                    }
                }
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
