using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyPhongTro.model;

namespace QuanLyPhongTro.data.configuration
{
    public class ServiceConfiguration : IEntityTypeConfiguration<Services>
    {
        public void Configure(EntityTypeBuilder<Services> builder)
        {
            builder.ToTable("Services");

            builder.HasKey(s => s.ServiceID);

            builder.Property(s => s.ServiceName)
                .IsRequired()
                .HasColumnType("char(50)");

            builder.Property(s => s.Unit)
                .IsRequired()
                .HasColumnType("char(30)");

            builder.Property(s => s.Price)
                .IsRequired()
                .HasColumnType("decimal(8,2)");

            builder.HasOne(s => s.BoardingHouse)
                .WithMany(b => b.Services)
                .HasForeignKey(s => s.BoardingHouseID)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(s => s.ServiceUsages)
                .WithOne(su => su.Service)
                .HasForeignKey(su => su.ServiceID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}