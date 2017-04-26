using System.Data.Entity;
using APIMetods.database.model;
using System.Collections.Generic;

namespace APIMetods.database
{
    static public class GatewayContext
    {
        static  GatewayContext()
        {
            statuses = new List<status>();
            statuses.Add(new status { status_id = 1, mesasage = "Ok" });
            statuses.Add(new status { status_id = 2, mesasage = "BackTransaction" });
            statuses.Add(new status { status_id = 3, mesasage = "WrongData" });
            statuses.Add(new status { status_id = 4, mesasage = "CloseLimit" });
            statuses.Add(new status { status_id = 5, mesasage = "ValidationError" });

            cards = new List<card>();
            cards.Add(new card { card_id = 1, card_number = "1111111111111111",expiry_month=5, expiry_year=2019,cvv=777,cardholder_name=null,cash=null });
            cards.Add(new card { card_id = 2, card_number = "2222222222222222", expiry_month = 6, expiry_year = 2018, cvv = 999, cardholder_name = "IVANOVIVAN", cash = 500000000 });

            orders = new List<order>();
            orders.Add(new order { order_id = 3, amount_kop = 4444444,status_id=1 });

            card_orders = new List<card_order>();
            card_orders.Add(new card_order { card_order_id = 1, card_id = 1, isconsumer = false ,order_id=3});
            card_orders.Add(new card_order { card_order_id = 2, card_id = 2, isconsumer = true, order_id = 3 });

        }

        static public List<status> statuses { get; set; }
        static public List<card> cards { get; set; }
        static public List<order> orders { get; set; }
        static public List<card_order> card_orders { get; set; }

    }
}
