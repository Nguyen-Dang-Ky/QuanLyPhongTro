using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyPhongTro.model;

namespace QuanLyPhongTro.data.configuration
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payments>
    {
        public void Configure(EntityTypeBuilder<Payments> builder)
        {
            builder.ToTable("Payments");

            builder.HasKey(p => p.PaymentID);

            builder.Property(p => p.PaymentDate)
                   .IsRequired()
                   .HasColumnType("date");

            builder.Property(p => p.AmountPaid)
                   .IsRequired()
                   .HasColumnType("decimal(8,2)");

            builder.HasOne(p => p.Bill)
                   .WithMany(b => b.Payments)
                   .HasForeignKey(p => p.BillID)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}