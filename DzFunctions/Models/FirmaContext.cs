using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace DzFunctions.Models
{
    public class FirmaContext : DbContext
    {
        private static DbContextOptions<FirmaContext> opt;

        static FirmaContext()
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            var config = configBuilder.Build();
            string connectionString = config.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<FirmaContext>();
            optionsBuilder.UseSqlServer(connectionString, sqlOpt =>
            {
                sqlOpt.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorNumbersToAdd: null
                );
                sqlOpt.CommandTimeout(60);
            });

            opt = optionsBuilder.Options;
        }

        public FirmaContext() : base(opt) {}
        public DbSet<SalesManager> SalesManagers { get; set; }
        public DbSet<BuyerCompany> BuyerCompanies { get; set; }
        public DbSet<OfficeSupply> OfficeSupplies { get; set; }
        public DbSet<SupplyType> SupplyTypes { get; set; }
        public DbSet<Sale> Sales { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AverageQuantityDto>().HasNoKey();
        }
    }
}