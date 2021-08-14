using FluentAssertions;
using Kernel.Domain.Model.Validation;
using Kernel.Infra;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using Univem.Churras.Domain.Model.Entities;
using Univem.Churras.Domain.Model.ValueObjects;
using Univem.Churras.Domain.Services;

namespace Univem.Churras.Tests.Services
{
    [TestClass]
    public class ColegaServiceTest
    {
        private static ColegaService _service;

        private static Colega _colega1;
        private static Colega _colega2;
        private static Colega _colega3;

        [ClassInitialize]
        public static async Task Setup(TestContext context)
        {
            _service = IoC.Get<ColegaService>();

            _colega1 = new Colega { Nome = "Colega 001", Endereco = new Endereco { Descricao = "Endereço 001" } };
            _colega2 = new Colega { Nome = "Colega 002", Endereco = new Endereco { Descricao = "Endereço 002" } };
            _colega3 = new Colega { Nome = "Colega 003", Endereco = new Endereco { Descricao = "Endereço 003" } };

            await _service.Insert(_colega1);
            await _service.Insert(_colega2);
            await _service.Insert(_colega3);
        }

        [TestMethod]
        public async Task Validation_Required_Test()
        {
            try
            {
                var colega = new Colega();
                await _service.Insert(colega);

                Assert.Fail("Deixou incluir um Colega sem Nome e/ou sem Endereço!");
            }
            catch (ValidatorException ex)
            {
                ex.Errors.Count.Should().Be(2);

                ex.Errors.Should().Contain(x => x.Message == "Nome do Colega é Obrigatório!");
                ex.Errors.Should().Contain(x => x.Message == "Descrição do Endereço é Obrigatória!");

                foreach (var error in ex.Errors)
                    Console.WriteLine(error.Message);
            }
        }

        [TestMethod]
        public async Task Validation_StringLength_Test()
        {
            try
            {
                var colega = new Colega
                {
                    Nome = "01234567890123456789012345678901234567890123456789X",
                    Endereco = new Endereco
                    {
                        Descricao = "Teste 123"
                    }
                };
                await _service.Insert(colega);

                Assert.Fail("Deixou incluir um Colega com nome maior que 50 chars!");
            }
            catch (ValidatorException ex)
            {
                ex.Errors.Count.Should().Be(1);
                ex.Errors.Should().Contain(x => x.Message == "Tamanho máximo do nome é 50 caracteres!");

                foreach (var error in ex.Errors)
                    Console.WriteLine(error.Message);
            }
        }

        [TestMethod]
        public async Task Insert_Test()
        {
            var colega = new Colega
            {
                Nome = "Tião",
                Endereco = new Endereco
                {
                    Descricao = "Teste 123"
                }
            };
            await _service.Insert(colega);

            colega.Key.Should().BeGreaterThan(0);
            Console.WriteLine(colega);
        }

        [TestMethod]
        public async Task Update_Test()
        {
            _colega1.Nome = "ALTERADO";
            await _service.Update(_colega1);

            var fromDb = await _service.Get(_colega1.Key);
            fromDb.Nome.Should().Be("ALTERADO");

            Console.WriteLine(fromDb);
        }

        [TestMethod]
        public async Task Delete_Test()
        {
            await _service.Delete(_colega2);

            var fromDb = await _service.Get(_colega2.Key);
            fromDb.Should().BeNull();
        }

        [TestMethod]
        public async Task Get_Test()
        {
            var fromDb = await _service.Get(_colega3.Key);
            fromDb.Should().NotBeNull();
            Console.WriteLine(fromDb);
        }

        [TestMethod]
        public async Task List_Test()
        {
            var fromDb = await _service.List();
            fromDb.Count.Should().BeGreaterThan(1);
            foreach(var x in fromDb)
                Console.WriteLine(x);
        }
    }
}
