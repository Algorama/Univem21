using Empresa.Churras.Domain.Model.Entities;
using Empresa.Churras.Domain.Model.Enums;
using Empresa.Churras.Domain.Model.ValueObjects;
using Empresa.Churras.Infra;
using Kernel.Domain.Model.Settings;
using Kernel.Infra.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Empresa.Churras.Tests.Repositories
{
    [TestClass]
    public class RepositoryTest
    {
        private ChurrasContext _context;

        public RepositoryTest()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.Development.json");

            var configuration = builder.Build();

            var appSettings = configuration.GetSection("AppSettings").Get<AppSettings>();

            _context = new ChurrasContext(appSettings);

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }

        [TestMethod]
        public async Task Insert_Colega_Test()
        {
            using (var session = new SessionRepository(_context))
            {
                var repo = session.GetRepository<Colega>();

                var tiao = new Colega
                {
                    Key = 1,
                    Nome = "Joca",
                    Endereco = new Endereco
                    {
                        Cidade = "Piracicaba",
                        UF = "SP",
                        Descricao = "Casa do Joca"
                    }
                };

                await repo.Insert(tiao);

                session.SaveChanges();
            }
        }

        [TestMethod]
        public async Task Insert_Evento_Test()
        {
            using (var session = new SessionRepository(_context))
            {
                var repoColega = session.GetRepository<Colega>();
                var repoEvento = session.GetRepository<Evento>();

                var pardinho = new Colega
                {
                    Key = 2,
                    Nome = "Pardinho",
                    Endereco = new Endereco
                    {
                        Cidade = "Piracicaba",
                        UF = "SP",
                        Descricao = "Chácara do Pardinho"
                    }
                };

                await repoColega.Insert(pardinho);

                var tiao = new Colega
                {
                    Key = 3,
                    Nome = "Tião",
                    Endereco = new Endereco
                    {
                        Cidade = "Piracicaba",
                        UF = "SP",
                        Descricao = "Chácara do Tião"
                    }
                };
                await repoColega.Insert(tiao);

                var evento = new Evento
                {
                    Key = 1,
                    DonoDaCasa = pardinho,
                    Tipo = TipoEvento.Churras,
                    Nome = "Churras na Chácara do Pardinho",
                    Dia = new DateTime(2021, 2, 28),
                    Periodo = new Periodo
                    {
                        Inicio = new DateTime(2021, 2, 28, 12, 0, 0),
                        Fim = new DateTime(2021, 2, 28, 12, 0, 0)
                    }
                };

                evento.ConfirmarPresenca(pardinho);
                evento.ConfirmarPresenca(tiao);

                await repoEvento.Insert(evento);

                session.SaveChanges();
            }
        }
    }
}
