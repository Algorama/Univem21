using Empresa.Churras.Domain.Model.Entities;
using Empresa.Churras.Domain.Model.ValueObjects;
using Empresa.Churras.Domain.Services;
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
            IoC.Container.Verify();

            var settings = IoC.Get<AppSettings>();
            var dbContext = IoC.Get<ChurrasContext>();

            if (settings.NoSqlDbSettings.AccountEndpoint == "https://localhost:8081")
            {
                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();
            }

            var colegaService = IoC.Get<ColegaService>();
            var colegaLogado = new Colega
            {
                Nome = "Colega de Teste",
                Endereco = new Endereco
                {
                    Descricao = "Endereço de Teste"
                }
            };
            await colegaService.Insert(colegaLogado);
        }
    }
}
