using Microsoft.EntityFrameworkCore;
using QuanLyPhongTro.model;

namespace QuanLyPhongTro.data
{
    public class ManagementDbContext : DbContext
    {
        public ManagementDbContext(DbContextOptions<ManagementDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users {get;set;}
        public DbSet<BoardingHouse> BoardingHouses {get;set;}
        public DbSet<Rooms> Rooms {get;set;}
        public DbSet<Customers> Customers {get;set;}
        public DbSet<Contracts> Contracts {get;set;}
        public DbSet<Services> Services {get;set;}
        public DbSet<ServiceUsages> ServiceUsages {get;set;}
        public DbSet<Bills> Bills {get;set;}
        public DbSet<BillDetails> BillDetails {get;set;}
        public DbSet<Payments> Payments {get;set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ManagementDbContext).Assembly);
        }
    }
}