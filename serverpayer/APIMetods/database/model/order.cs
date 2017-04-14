using System.Collections.Generic;

namespace APIMetods.database.model
{
   public class order
    {
       public order()
        {
            this.card_orders = new HashSet<card_order>();
        }
        public int order_id { get; set; }
        public decimal amount_kop { get; set; }
        public int status_id { get; set; }

        public virtual status status { get; set; }
        public virtual ICollection<card_order> card_orders { get; set; }
    }
}
