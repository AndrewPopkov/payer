using System.Data.Entity;
using APIMetods.database.model;

namespace APIMetods.database
{
    public class GatewayContext : DbContext
    {
        public GatewayContext()
        {

        }

        public DbSet<status> statuses { get; set; }
        public DbSet<card> cards { get; set; }
        public DbSet<order> orders { get; set; }
        public DbSet<card_order> card_orders { get; set; }

    }
}
