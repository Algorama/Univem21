using Kernel.Domain.Model.Settings;
using Kernel.Infra;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Univem.Churras.Infra;

namespace Univem.Churras.Tests
{
    [TestClass]
    public class Startup
    {
        [AssemblyInitialize]
        public static async Task Initialize(TestContext context)
        {
            IoC.Start<ChurrasContext>();

            var settings = IoC.Get<AppSettings>();

            if (settings.NoSqlDbSettings.AccountEndpoint == "https://localhost:8081")
            {
                var dbContext = IoC.Get<ChurrasContext>();
                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();
            }

            await Task.CompletedTask;
        }
    }
}
