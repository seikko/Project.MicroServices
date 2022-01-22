using Course.Services.Domain.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Services.Order.Infrastructure
{
    public class OrderDbContext : DbContext
    {
        public const string Default_Schema = "ordering";

        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Services.Domain.OrderAggregate.Order>().ToTable("Orders", Default_Schema);
            modelBuilder.Entity<OrderItem>().ToTable("OrderItems", Default_Schema);
        
            modelBuilder.Entity<OrderItem>().Property(x => x.Price).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Services.Domain.OrderAggregate.Order>().OwnsOne(x => x.Address).WithOwner();
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<OrderItem>  OrderItems { get; set; }
        public DbSet<Services.Domain.OrderAggregate.Order> Orders { get; set; }
    }
}
