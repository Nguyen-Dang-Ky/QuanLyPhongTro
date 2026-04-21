using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyPhongTro.model;

namespace QuanLyPhongTro.data.configuration
{
    public class RoomConfiguration : IEntityTypeConfiguration<Rooms>
    {
        public void Configure(EntityTypeBuilder<Rooms> builder)
        {
            builder.ToTable("Rooms");

            builder.HasKey(r => r.RoomID);

            builder.Property(r => r.RoomNumber)
                   .IsRequired()
                   .HasMaxLength(20);

              builder.Property(r => r.Status)
                     .IsRequired()
                     .HasDefaultValue(RoomStatus.Available);

            builder.HasOne(r => r.BoardingHouse)
                   .WithMany(b => b.Rooms)
                   .HasForeignKey(r => r.BoardingHouseID)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(r => r.Customers)
                   .WithOne(c => c.Rooms)
                   .HasForeignKey(c => c.RoomID)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(r => r.Contracts)
                   .WithOne(ct => ct.Room)
                   .HasForeignKey(ct => ct.RoomID)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(r => r.Bills)
                   .WithOne(b => b.Room)
                   .HasForeignKey(b => b.RoomID)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(r => r.ServiceUsages)
                   .WithOne(su => su.Room)
                   .HasForeignKey(su => su.RoomID)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}