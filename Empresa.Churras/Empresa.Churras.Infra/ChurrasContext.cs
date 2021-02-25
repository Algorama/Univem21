using Kernel.Domain.Model.Settings;
using Kernel.Infra.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Empresa.Churras.Infra
{
    public class ChurrasContext : KernelContext
    {
        public ChurrasContext(AppSettings appSettings) : base(appSettings)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ChurrasContext).Assembly);
        }
    }
}