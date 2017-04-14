using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace APIMetods.database.model
{
    class card_order
    {
        public card_order()
        {
        }

        public int card_order_id { get; set; }
        public int card_id { get; set; }
        public int order_id { get; set; }
        public bool isconsumer { get; set; }


        public virtual card card { get; set; }
        public virtual order order { get; set; }
    }
}
