using Empresa.Churras.Infra;
using Kernel.Domain.Model.Settings;
using Kernel.Infra;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Empresa.Churras.Tests
{
    [TestClass]
    public class Startup
    {
        public static IConfiguration Configuration { get; set; }

        [AssemblyInitialize]
        public static async Task Initialize(TestContext context)
        {
            IoC.Start<ChurrasContext>();

            var settings = IoC.Get<AppSettings>();
            var dbContext = IoC.Get<ChurrasContext>();

            if (settings.NoSqlDbSettings.AccountEndpoint == "https://localhost:8081")
            {
                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();
            }
            
            await Task.CompletedTask;
        }
    }
}
