using Kernel.Domain.Model.Settings;
using Kernel.Infra.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Univem.Churras.Domain.Model.Entities;
using Univem.Churras.Domain.Model.ValueObjects;
using Univem.Churras.Infra;

namespace Univem.Churras.Tests
{
    [TestClass]
    public class RepositoryTest
    {
        private ChurrasContext _context;

        public RepositoryTest()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.Development.json");

            var config = builder.Build();

            var appSettings = config.GetSection("AppSettings").Get<AppSettings>();

            _context = new ChurrasContext(appSettings);

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }

        [TestMethod]
        public async Task Insert_Colega_Test()
        {
            using(var session = new SessionRepository(_context))
            {
                var repo = session.GetRepository<Colega>();

                var tiao = new Colega
                {
                    Key = 1,
                    Nome = "Tião Carrero",
                    Endereco = new Endereco 
                    { 
                        Cidade = "Piracicaba",
                        UF = "SP",
                        Descricao = "Casa do Tião"
                    }
                };

                await repo.Insert(tiao);

                session.SaveChanges();
            }
        }
    }
}
