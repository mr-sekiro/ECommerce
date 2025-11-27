using DomainLayer.Models.OrderModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data.Configurations.OrderConfigurations
{
    public class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");

            builder.Property(o => o.Subtotal).HasColumnType("decimal(8,2)");
            builder.Property(o => o.OrderDate).IsRequired();

            // owned type mapping
            builder.OwnsOne(o => o.ShipToAddress, a =>
            {
                a.Property(p => p.FirstName).HasMaxLength(100).IsRequired();
                a.Property(p => p.LastName).HasMaxLength(100).IsRequired();
                a.Property(p => p.Street).HasMaxLength(200);
                a.Property(p => p.City).HasMaxLength(100);
                a.Property(p => p.Country).HasMaxLength(100);
            });

            builder.HasMany(o => o.OrderItems)
                   .WithOne(oi => oi.Order)
                   .HasForeignKey(oi => oi.OrderId);

            builder.HasOne(o => o.DeliveryMethod)
                   .WithMany()
                   .HasForeignKey(o => o.DeliveryMethodId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
