using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyPhongTro.model;

namespace QuanLyPhongTro.data.configuration
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customers>
    {
        public void Configure(EntityTypeBuilder<Customers> builder)
        {
            builder.ToTable("Customers");

            builder.HasKey(c => c.CustomerID);

            builder.Property(c => c.IdentityNumber)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.HasOne(c => c.User)
                   .WithOne(u => u.Customers)
                   .HasForeignKey<Customers>(c => c.UserID)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Rooms)
                   .WithMany(r => r.Customers)
                   .HasForeignKey(c => c.RoomID)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.Contracts)
                   .WithOne(ct => ct.Customer)
                   .HasForeignKey(ct => ct.CustomerID)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}