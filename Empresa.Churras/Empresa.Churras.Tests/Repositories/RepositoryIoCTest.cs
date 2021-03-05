using Empresa.Churras.Domain.Model.Entities;
using Empresa.Churras.Domain.Model.Enums;
using Empresa.Churras.Domain.Model.ValueObjects;
using Kernel.Domain.Repositories;
using Kernel.Infra;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Empresa.Churras.Tests.Repositories
{
    [TestClass]
    public class RepositoryIoCTest
    {
        private ISessionFactory _factory;

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
                    Key = 100001,
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
            using (var session = _factory.OpenSession())
            {
                var repoColega = session.GetRepository<Colega>();
                var repoEvento = session.GetRepository<Evento>();

                var pardinho = new Colega
                {
                    Key = 200002,
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
                    Key = 300003,
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
                    Key = 100001,
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
