using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyPhongTro.model;

namespace QuanLyPhongTro.data.configuration
{
    public class BillDetailConfiguration : IEntityTypeConfiguration<BillDetails>
    {
        public void Configure(EntityTypeBuilder<BillDetails> builder)
        {
            builder.ToTable("BillDetails");

            builder.HasKey(bd => bd.BillDetailID);

            builder.Property(bd => bd.Description)
                   .HasMaxLength(200);

            builder.Property(bd => bd.TotalPrice)
                   .IsRequired()
                   .HasColumnType("decimal(8,2)");

            builder.HasOne(bd => bd.Bill)
                   .WithMany(b => b.BillDetails)
                   .HasForeignKey(bd => bd.BillID)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}