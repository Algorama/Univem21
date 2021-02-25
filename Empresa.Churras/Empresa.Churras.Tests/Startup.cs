using Empresa.Churras.Infra;
using Kernel.Domain.Model.Dtos;
using Kernel.Domain.Model.Providers;
using Kernel.Domain.Model.Settings;
using Kernel.Infra;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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
            IoC.Register<IUserProvider, TestUserProvider>();

            var settings = IoC.Get<AppSettings>();
            var dbContext = IoC.Get<ChurrasContext>();

            if (settings.NoSqlDbSettings.AccountEndpoint == "https://localhost:8081")
            {
                dbContext.Database.EnsureDeleted();
                Console.WriteLine("Database Deletado");

                dbContext.Database.EnsureCreated();
                Console.WriteLine("Database Criado");

                // Seed
            }
            
            await Task.CompletedTask;
        }
    }

    public class TestUserProvider : IUserProvider
    {
        public async Task<Token> GetToken()
        {
            var token = new Token
            {
                Key = 1,
                Email = "test",
                Name = "test",
                ExpiresIn = DateTime.Now.AddHours(8)
            };
            return await Task.FromResult(token);
        }

        public Task RemoveToken()
        {
            throw new NotImplementedException();
        }

        public Task SaveTokenString(string tokenString)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsAuthenticated()
        {
            throw new System.NotImplementedException();
        }
    }
}
