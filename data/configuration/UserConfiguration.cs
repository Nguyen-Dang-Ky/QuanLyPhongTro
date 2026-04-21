using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyPhongTro.model;

namespace QuanLyPhongTro.data.configuration 
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
              // CONFIGURATION USER-BOARDINGHOUSE-CUSTOMER

              builder.ToTable("Users");

              builder.HasKey(u => u.UserID);

              builder.Property(u => u.FullName)
                     .IsRequired()
                     .HasMaxLength(100);

              builder.Property(u=>u.CreatedAt)
                     .HasColumnType("date")
                     .HasDefaultValueSql("GETDATE()");

              builder.HasIndex(u => u.Email)
                     .IsUnique();
              builder.Property(u=>u.IsActive)
                     .HasDefaultValue(true);

              builder.HasMany(u => u.BoardingHouses)
                     .WithOne(b => b.Owner)
                     .HasForeignKey(b => b.OwnerID)
                     .OnDelete(DeleteBehavior.Restrict);

              builder.HasOne(u => u.Customers)
                     .WithOne(c => c.User)
                     .HasForeignKey<Customers>(c => c.UserID)
                     .OnDelete(DeleteBehavior.Restrict);
        }
    }
}