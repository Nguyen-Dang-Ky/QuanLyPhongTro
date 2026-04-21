using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyPhongTro.model;

namespace QuanLyPhongTro.data.configuration
{
    public class ContractConfiguration : IEntityTypeConfiguration<Contracts>
    {
        public void Configure(EntityTypeBuilder<Contracts> builder)
        {
            builder.ToTable("Contracts");

            builder.HasKey(c => c.ContractID);

            builder.Property(c => c.StartDate)
                   .IsRequired();

            builder.Property(c => c.EndDate)
                   .IsRequired();

            builder.Property(c => c.DepositAmount)
                   .IsRequired()
                   .HasColumnType("decimal(8,2)");
            
            builder.Property(c => c.MonthlyPrice)
                   .IsRequired()
                   .HasColumnType("decimal(8,2)");

            builder.Property(c=>c.Status)
                   .IsRequired()
                   .HasDefaultValue(ContractStatus.Active);

            builder.HasOne(c => c.Customer)
                   .WithMany()
                   .HasForeignKey(c => c.CustomerID)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Room)
                   .WithMany()
                   .HasForeignKey(c => c.RoomID)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}