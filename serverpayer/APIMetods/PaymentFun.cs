using APIMetods.database;
using LogHelper;
using System;
using System.Linq;
using System.Data.Entity;
using Newtonsoft.Json.Linq;

namespace APIMetods
{
    public class PaymentFun
    {
        enum statusEnum
        {
            Ok,
            BackTransaction,
            BankError,
            WrongData,
            CloseLimit,
        }
        public static JObject Pay(int order_id, string card_number, int expiry_month, int expiry_year, int cvv, decimal amount_kop, string cardholder_name = "")
        {
            int status;
            JObject result = null;
            status_t statusobj;
            using (bankGatewayEntities db = new bankGatewayEntities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        
                        card_t vendorCard = db.card_t.Find(1);
                        card_t consumerCard = db.card_t.Where(p => p.card_number == card_number &&
                                                            p.expiry_month == expiry_month &&
                                                            p.expiry_year == expiry_year &&
                                                            p.cvv == cvv && cardholder_name != null ? p.cardholder_name == cardholder_name : true).FirstOrDefault();
                        if ( consumerCard!= null)
                        {

                            if (amount_kop > consumerCard.cash)
                            {
                                status = (int)statusEnum.CloseLimit;
                            }
                            else
                            {
                                status = (int)statusEnum.Ok;
                                consumerCard.cash -= amount_kop;
                                
                            }

                        }
                        else
                        {
                            status = (int)statusEnum.WrongData;
                        }
                        order_t order = new order_t()
                        {
                            consumer_id = consumerCard.card_id,
                            vendor_id = vendorCard.card_id,
                            status_id = status,
                            order_id = order_id,
                            amount_kop = amount_kop
                        };
                        db.order_t.Add(order);
                        db.SaveChanges();
                        transaction.Commit();
                        statusobj = db.status_t.Find(status);
                        result = new JObject(statusobj);
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Log.Write(ex);
                    }
                }
            }

            return result;
        }
        public static JObject GetStatus(int order_id)
        {
            JObject result=null;
            status_t status;
            using (bankGatewayEntities db = new bankGatewayEntities())
            {
                try
                {
                     order_t order = db.order_t.Find(order_id);
                     if (order != null)
                     {
                         status = db.status_t.Find(order.status_id);
                         result = new JObject(status);
                     }  
                }
                catch (Exception ex)
                {
                    Log.Write(ex);
                }    

               
            }

            return result;
        }
        public static int Refund(int order_id)
        {
            return 0;
        }
    }
}
