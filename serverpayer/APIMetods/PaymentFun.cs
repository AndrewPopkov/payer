using APIMetods.database;
using LogHelper;
using System;
using System.Linq;
using System.Data.Entity;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace APIMetods
{
    public class PaymentFun
    {
        enum statusEnum
        {
            Ok,
            BackTransaction,
            WrongData,
            CloseLimit,
            ValidationError
        }
        //ид банка
        private readonly int vendorCard_id = 1;
        private int order_id;
        private string card_number;
        private int expiry_month;
        private int expiry_year;
        private int cvv;
        private decimal amount_kop;
        private string cardholder_name;
        public PaymentFun()
        {

        }
        private JObject GetValidationError()
        {
            JObject result = new JObject();
            result.Add("id", (int)statusEnum.ValidationError);
            result.Add("mesasage", "Ошибка валидации");
            return result;
        }
        private bool RefundValidationParams(string str_order_id)
        {
            bool ValidationData = false;
            if (Regex.Match(str_order_id, @"(\d+)").Success)
            {
                ValidationData = int.TryParse(str_order_id, out this.order_id);
            }
            return ValidationData;
        }

        private bool GetStatusValidationParams(string str_order_id)
        {
            bool ValidationData = false;
            if (Regex.Match(str_order_id, @"(\d+)").Success)
            {
                ValidationData = int.TryParse(str_order_id, out this.order_id);
            }
            return ValidationData;
        }
        private bool PayValidationParams(string str_order_id, string card_number, string str_expiry_month,
                                         string str_expiry_year, string str_cvv, string str_amount_kop,
                                         string cardholder_name)
        {
            bool ValidationData = false;
            if (Regex.Match(card_number, @"(\d\d\d\d\d\d\d\d\d\d\d\d\d\d\d\d)").Success)
            {
                ValidationData = true;
                this.card_number = card_number;
            }
            if (Regex.Match(str_order_id, @"(\d+)").Success)
            {
                ValidationData = int.TryParse(str_order_id, out this.order_id);
            }
            if (Regex.Match(str_expiry_month, @"(\d\d)").Success)
            {
                if (int.TryParse(str_expiry_month, out this.expiry_month))
                {
                    if (this.expiry_month > 0 && this.expiry_month <= 12)
                    {
                        ValidationData = true;
                    }
                }
            }
            if (Regex.Match(str_expiry_month, @"(\d\d\d\d)").Success)
            {
                if (int.TryParse(str_expiry_year, out this.expiry_year))
                {
                    if (this.expiry_year > 2000 && this.expiry_month <= 2100)
                    {
                        ValidationData = true;
                    }
                }
            }
            if (Regex.Match(str_cvv, @"(\d\d\d)").Success)
            {
                if (int.TryParse(str_cvv, out this.cvv))
                {
                    ValidationData = true;
                }
            }
            if (Regex.Match(str_amount_kop, @"(\d+)").Success)
            {
                if (decimal.TryParse(str_amount_kop, out this.amount_kop))
                {
                    ValidationData = true;
                }
            }
            this.cardholder_name = cardholder_name;
            return ValidationData;

        }
        public JObject Pay(string order_id, string card_number, string expiry_month, string expiry_year, string cvv, string amount_kop, string cardholder_name = "")
        {
            int status;
            JObject result = null;
            status statusobj;

            if (PayValidationParams(order_id, card_number, expiry_month, expiry_year, cvv, amount_kop, cardholder_name))
            {

                using (bankGatewayEntities db = new bankGatewayEntities())
                {
                    //using (var transaction = db.Database.BeginTransaction())
                    //{
                        try
                        {
                            card consumerCard = db.cards.Where(p => p.card_number == card_number &&
                                                                p.expiry_month == this.expiry_month &&
                                                                p.expiry_year == this.expiry_year &&
                                                                p.cvv == this.cvv && cardholder_name != null ? p.cardholder_name == cardholder_name : true).FirstOrDefault();
                            if (consumerCard != null)
                            {

                                if (this.amount_kop > consumerCard.cash)
                                {
                                    status = (int)statusEnum.CloseLimit;
                                }
                                else
                                {
                                    status = (int)statusEnum.Ok;
                                    consumerCard.cash -= this.amount_kop;

                                }

                            }
                            else
                            {
                                status = (int)statusEnum.WrongData;
                            }

                            order order = new order()
                            {
                                consumer_id = consumerCard.card_id,
                                vendor_id = vendorCard_id,
                                status_id = status,
                                order_id = this.order_id,
                                amount_kop = this.amount_kop
                            };
                            db.orders.Add(order);
                            db.SaveChanges();
                            //transaction.Commit();
                            statusobj = db.statuss.Find(status);
                            result = new JObject(statusobj);
                        }

                        catch (Exception ex)
                        {
                           // transaction.Rollback();
                            Log.Write(ex);
                        }
                   // }
                }
            }
            else
            {

               result = GetValidationError();
            }

            return result;
        }
        public JObject GetStatus(string order_id)
        {
            JObject result = null;
            status status;
            if (GetStatusValidationParams(order_id))
            {
                using (bankGatewayEntities db = new bankGatewayEntities())
                {
                    try
                    {
                        order order = db.orders.Find(this.order_id);
                        if (order != null)
                        {
                            status = db.statuss.Find(order.status_id);
                            result = new JObject(status);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Write(ex);
                    }


                }
            }
            else
            {
                result = GetValidationError();
            }


            return result;
        }
        public JObject Refund(string order_id)
        {
            JObject result = null;
            status status;
            if (RefundValidationParams(order_id))
            {
                using (bankGatewayEntities db = new bankGatewayEntities())
                {
                    using (var transaction = db.Database.BeginTransaction())
                    {
                        try
                        {

                            order order = db.orders.Find(this.order_id);
                            if (order != null)
                            {
                                card consumer_card = db.cards.Find(order.consumer_id);
                                if (consumer_card != null)
                                {
                                    consumer_card.cash += order.amount_kop;
                                    order.status_id = (int)statusEnum.BackTransaction;
                                    db.SaveChanges();
                                    transaction.Commit();
                                    status = db.statuss.Find((int)statusEnum.Ok);
                                    result = new JObject(status);
                                }
                            }
                            else
                            {
                                status = db.statuss.Find((int)statusEnum.WrongData);
                                result = new JObject(status);
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
            else
            {
                result = GetValidationError();
            }
            return result;
        }
    }
}
