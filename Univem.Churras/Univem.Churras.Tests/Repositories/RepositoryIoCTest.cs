using Kernel.Domain.Repositories;
using Kernel.Infra;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using Univem.Churras.Domain.Model.Entities;
using Univem.Churras.Domain.Model.Enums;
using Univem.Churras.Domain.Model.ValueObjects;

namespace Univem.Churras.Tests.Repositories
{
    [TestClass]
    public class RepositoryIoCTest
    {
        private readonly ISessionFactory _factory;

        public RepositoryIoCTest()
        {
            _factory = IoC.Get<ISessionFactory>();
        }

        [TestMethod]
        public async Task Insert_Colega_Test()
        {
            using (var session = _factory.OpenSession())
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
            using (ISessionRepository session = _factory.OpenSession())
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
