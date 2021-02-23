using Kernel.Domain.Model.Entities;
using Kernel.Domain.Model.Settings;
using Microsoft.EntityFrameworkCore;

namespace Algorama.Kernel.Infra.Repositories
{
    public class KernelContext : DbContext
    {
        public AppSettings AppSettings { get; set; }

        public KernelContext(AppSettings appSettings)
        {
            AppSettings = appSettings;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseCosmos(
               AppSettings.NoSqlDbSettings.AccountEndpoint,
               AppSettings.NoSqlDbSettings.AccountKey,
               AppSettings.NoSqlDbSettings.DatabaseName,
               options => {}
             );
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultContainer("Kernel");
            modelBuilder.Entity<Sequence>().ToContainer("Kernel").HasPartitionKey(x => x.Discriminator);
        }
    }
}
