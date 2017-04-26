using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace APIMetods.database.model
{
    public class status
    {
        public status()
        {
            this.orders = new HashSet<order>();
        }

        public int status_id { get; set; }
        public string mesasage { get; set; }

        public virtual ICollection<order> orders { get; set; }

    }
}
