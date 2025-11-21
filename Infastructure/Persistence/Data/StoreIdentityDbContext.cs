using DomainLayer.Models.IdentityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data
{
    public class StoreIdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public StoreIdentityDbContext(DbContextOptions<StoreIdentityDbContext> options)
            : base(options)
        {
        }

        public DbSet<Address> Addresses { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure one-to-one ApplicationUser <-> Address
            builder.Entity<ApplicationUser>()
                   .HasOne(u => u.Address)
                   .WithOne(a => a.User)
                   .HasForeignKey<Address>(a => a.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Optional: configure column types / lengths
            builder.Entity<Address>(b =>
            {
                b.Property(a => a.FirstName).HasMaxLength(100);
                b.Property(a => a.LastName).HasMaxLength(100);
                b.Property(a => a.Street).HasMaxLength(200);
                b.Property(a => a.City).HasMaxLength(100);
                b.Property(a => a.Country).HasMaxLength(100);
            });


            // builder.Entity<ApplicationUser>().ToTable("AspNetUsers");
            // builder.Entity<IdentityRole>().ToTable("AspNetRoles");

            //builder.Ignore<IdentityUserClaim<string>>();
            //builder.Ignore<IdentityUserToken<string>>();
            //builder.Ignore<IdentityUserLogin<string>>();
            //builder.Ignore<IdentityRoleClaim<string>>();
        }
    }
}
