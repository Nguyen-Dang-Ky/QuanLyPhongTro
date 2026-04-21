using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyPhongTro.model;

namespace QuanLyPhongTro.data.configuration
{
    public class BillConfiguration : IEntityTypeConfiguration<Bills>
    {
        public void Configure(EntityTypeBuilder<Bills> builder)
        {
            builder.ToTable("Bills");

            builder.HasKey(b => b.BillID);

            builder.Property(b => b.TotalAmount)
                   .IsRequired()
                   .HasColumnType("decimal(8,2)");

            builder.HasOne(b => b.Room)
                   .WithMany(r => r.Bills)
                   .HasForeignKey(b => b.RoomID)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(b => b.BillDetails)
                   .WithOne(bd => bd.Bill)
                   .HasForeignKey(bd => bd.BillID)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(b=>b.Payments)
                   .WithOne(p=>p.Bill)
                   .HasForeignKey(p=>p.BillID)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}