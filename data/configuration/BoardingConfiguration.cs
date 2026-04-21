using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyPhongTro.model;

namespace QuanLyPhongTro.data.configuration
{
    public class BoardingConfiguration : IEntityTypeConfiguration<BoardingHouse>
    {
        public void Configure(EntityTypeBuilder<BoardingHouse> builder)
        {
            builder.ToTable("BoardingHouses");

            builder.HasKey(b => b.BoardingHouseID);

            builder.Property(b => b.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(b => b.Address)
                   .IsRequired()
                   .HasMaxLength(200);

              builder.Property(b=>b.CreatedAt)
                     .HasColumnType("date")
                     .HasDefaultValueSql("GETDATE()");

            builder.HasOne(b => b.Owner)
                   .WithMany(u => u.BoardingHouses)
                   .HasForeignKey(b => b.OwnerID)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(b => b.Rooms)
                   .WithOne(r => r.BoardingHouse)
                   .HasForeignKey(r => r.BoardingHouseID)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(b => b.Services)
                   .WithOne(s => s.BoardingHouse)
                   .HasForeignKey(s => s.BoardingHouseID)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}