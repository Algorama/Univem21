using FluentAssertions;
using Kernel.Domain.Model.Dtos;
using Kernel.Domain.Model.Helpers;
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

            _colega1 = new Colega { Nome = "Colega 001", Email = "colega1", Senha = "123", Endereco = new Endereco { Descricao = "Endereço 001" } };
            _colega2 = new Colega { Nome = "Colega 002", Email = "colega2", Senha = "123", Endereco = new Endereco { Descricao = "Endereço 002" } };
            _colega3 = new Colega { Nome = "Colega 003", Email = "colega3", Senha = "123", Endereco = new Endereco { Descricao = "Endereço 003" } };

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
                ex.Errors.Count.Should().Be(4);

                ex.Errors.Should().Contain(x => x.Message == "Nome do Colega é Obrigatório!");
                ex.Errors.Should().Contain(x => x.Message == "Descrição do Endereço é Obrigatória!");
                ex.Errors.Should().Contain(x => x.Message == "E-Mail do Colega é Obrigatório!");
                ex.Errors.Should().Contain(x => x.Message == "Senha é Obrigatória!");

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
                    Email = "teste",
                    Senha = "123",
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
                Email = "tiao",
                Senha = "123",
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

        [TestMethod]
        public async Task Senha_Crypto_Test()
        {
            var colega = new Colega
            {
                Nome = "Tião",
                Email = "tiao",
                Senha = "123",
                Endereco = new Endereco
                {
                    Descricao = "Teste 123"
                }
            };
            await _service.Insert(colega);

            var senhaCrypto = CryptoHelper.ComputeHashMd5("123");

            colega.Senha.Should().Be(senhaCrypto);
            Console.WriteLine(colega.Senha);
        }

        [TestMethod]
        public async Task Senha_Update_Test()
        {
            var colega = new Colega
            {
                Nome = "Tião",
                Email = "tiao",
                Senha = "123",
                Endereco = new Endereco
                {
                    Descricao = "Teste 123"
                }
            };
            await _service.Insert(colega);

            colega.Senha = "XXXXXX";
            await _service.Update(colega);

            var senhaCrypto = CryptoHelper.ComputeHashMd5("123");
            colega.Senha.Should().Be(senhaCrypto);

            Console.WriteLine(colega.Senha);
        }

        [TestMethod]
        public async Task Login_Ok_Test()
        {
            var loginRequest = new LoginRequest
            {
                Email = "colega1",
                Senha = "123"
            };

            var token = await _service.Login(loginRequest);

            token.Should().NotBeNull();
            Console.WriteLine(token.Name);
        }

        [TestMethod]
        public async Task Login_SenhaErrada_Test()
        {
            try
            {
                var loginRequest = new LoginRequest
                {
                    Email = "colega1",
                    Senha = "XXXXXXX"
                };

                var token = await _service.Login(loginRequest);

                Assert.Fail();
            }
            catch (ValidatorException ex)
            {
                ex.Errors.Should().Contain(x => x.Message == "Login Inválido!");
            }
        }

        [TestMethod]
        public async Task Login_EmailErrado_Test()
        {
            try
            {
                var loginRequest = new LoginRequest
                {
                    Email = "colega1XYZ",
                    Senha = "123"
                };

                var token = await _service.Login(loginRequest);

                Assert.Fail();
            }
            catch (ValidatorException ex)
            {
                ex.Errors.Should().Contain(x => x.Message == "Login Inválido!");
            }
        }

        [TestMethod]
        public async Task Login_Required_Test()
        {
            try
            {
                var loginRequest = new LoginRequest();

                var token = await _service.Login(loginRequest);

                Assert.Fail();
            }
            catch (ValidatorException ex)
            {
                ex.Errors.Count.Should().Be(2);
                ex.Errors.Should().Contain(x => x.Message == "Email é obrigatório");
                ex.Errors.Should().Contain(x => x.Message == "Senha inválida");

                foreach(var error in ex.Errors)
                    Console.WriteLine(error.Message);
            }
        }

        [TestMethod]
        public async Task TrocaSenha_Test()
        {
            var colega = new Colega
            {
                Nome = "Colega para troca de senha",
                Email = "trocaSenha@email",
                Senha = "123",
                Endereco = new Endereco
                {
                    Descricao = "Teste 123"
                }
            };
            await _service.Insert(colega);

            var trocaSenhaRequest = new TrocaSenhaRequest
            {
                Email = "trocaSenha@email",
                SenhaAntiga = "123",
                SenhaNova = "321",
                SenhaNovaConfirma = "321"
            };

            await _service.TrocaSenha(trocaSenhaRequest);

            var senhaHash = CryptoHelper.ComputeHashMd5("321");
            var fromDb = await _service.Get(colega.Key);
            fromDb.Senha.Should().Be(senhaHash);
        }
    }
}
