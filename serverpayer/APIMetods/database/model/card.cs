using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace APIMetods.database.model
{
    public class card
    {
        public card()
        {
            this.card_orders = new HashSet<card_order>();
        }

        public int card_id { get; set; }
        public string card_number { get; set; }
        public int expiry_month { get; set; }
        public int expiry_year { get; set; }
        public int cvv { get; set; }
        public string cardholder_name { get; set; }
        public Nullable<decimal> cash { get; set; }

        public virtual ICollection<card_order> card_orders { get; set; }

    }
}
