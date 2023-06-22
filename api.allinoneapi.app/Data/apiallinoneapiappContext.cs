#nullable disable
//using System.Data.Entity;

//using api.allinoneapi.app.Models;
using api.allinoneapi.app.Models.Test;
using api.allinoneapi.Models;
using api.allinoneapi.Models.Stocks.Polygon;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace api.allinoneapi.app.Data
{
    public class apiallinoneapiappContext : DbContext
    {
        private string connectionString;
        public apiallinoneapiappContext ()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json", optional: false);
            var configuration = builder.Build();
            connectionString = configuration.GetConnectionString("apiallinoneapiContext").ToString();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
        public DbSet<Crypto_Symbols> Crypto_Symbols { get; set; }
        public DbSet<Crypto_Price> Crypto_Price { get; set; }
        public DbSet<StockInstruments> StockInstruments { get; set; }
        public DbSet<StockDescription> StockDescription { get; set; }
        public DbSet<Binance_CryptoKandles> CryptoKandles { get; set; }
        public DbSet<Test> Test { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Test>((pc =>
            {
                pc.HasNoKey();
            }));
        }
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<decimal>()
                .HavePrecision(20, 10);
        }
        ~apiallinoneapiappContext()
        {
        }
    }
}
