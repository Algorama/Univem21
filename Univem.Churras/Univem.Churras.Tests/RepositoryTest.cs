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
using Univem.Churras.Domain.Model.Enums;
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
            using (var session = new SessionRepository(_context))
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

        [TestMethod]
        public async Task Insert_Evento_Test()
        {
            using (var session = new SessionRepository(_context))
            {
                var repoColega = session.GetRepository<Colega>();
                var repoEvento = session.GetRepository<Evento>();

                var tiao = new Colega
                {
                    Key = 1001,
                    Nome = "Tião Carrero",
                    Endereco = new Endereco
                    {
                        Cidade = "Piracicaba",
                        UF = "SP",
                        Descricao = "Casa do Tião"
                    }
                };

                var pardinho = new Colega
                {
                    Key = 1002,
                    Nome = "Pardinho",
                    Endereco = new Endereco
                    {
                        Cidade = "Limeira",
                        UF = "SP",
                        Descricao = "Sítio do Pardinho"
                    }
                };

                await repoColega.Insert(tiao);
                await repoColega.Insert(pardinho);

                var evento = new Evento
                {
                    Key = 1001,
                    DonoDaCasa = tiao,
                    Tipo = TipoEvento.Churras,
                    Nome = "Churras na casa do Tião",
                    Dia = new DateTime(2021, 8, 21),
                    Periodo = new Periodo
                    {
                        Inicio = new DateTime(2021, 8, 21, 12, 0, 0),
                        Fim = new DateTime(2021, 8, 21, 12, 0, 0)
                    }
                };

                evento.ConfirmarPresenca(tiao);
                evento.ConfirmarPresenca(pardinho);

                await repoEvento.Insert(evento);

                session.SaveChanges();
            }
        }
    }
}
