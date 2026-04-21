using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyPhongTro.model;

namespace QuanLyPhongTro.data.configuration
{
    public class ServiceUsageConfiguration : IEntityTypeConfiguration<ServiceUsages>
    {
        public void Configure(EntityTypeBuilder<ServiceUsages> builder)
        {
            builder.HasKey(su => su.ServiceUsageID);
            builder.HasOne(su => su.Service)
                   .WithMany()
                   .HasForeignKey(su => su.ServiceID);
            builder.HasOne(su => su.Room)
                   .WithMany()
                   .HasForeignKey(su => su.RoomID);
        }
    }
}