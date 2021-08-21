using FluentAssertions;
using Kernel.Domain.Model.Providers;
using Kernel.Domain.Model.Validation;
using Kernel.Infra;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using Univem.Churras.Domain.Model.Entities;
using Univem.Churras.Domain.Model.Enums;
using Univem.Churras.Domain.Model.ValueObjects;
using Univem.Churras.Domain.Services;

namespace Univem.Churras.Tests.Services
{
    [TestClass]
    public class EventoServiceTest
    {
        private static EventoService _service;
        private static IUserProvider _userProvider;

        private static Colega _colega1;
        private static Colega _colega2;
        private static Colega _colega3;

        private static Evento _evento1;
        private static Evento _evento2;
        private static Evento _evento3;

        [ClassInitialize]
        public static async Task Setup(TestContext context)
        {
            _service = IoC.Get<EventoService>();
            _userProvider = IoC.Get<IUserProvider>();

            _colega1 = new Colega { Nome = "Colega 001", Endereco = new Endereco { Descricao = "Endereço 001" } };
            _colega2 = new Colega { Nome = "Colega 002", Endereco = new Endereco { Descricao = "Endereço 002" } };
            _colega3 = new Colega { Nome = "Colega 003", Endereco = new Endereco { Descricao = "Endereço 003" } };

            var colegaService = IoC.Get<ColegaService>();

            await colegaService.Insert(_colega1);
            await colegaService.Insert(_colega2);
            await colegaService.Insert(_colega3);

            _evento1 = GetFakeEvento("Churras 1", DateTime.Today.AddDays(8));
            _evento2 = GetFakeEvento("Churras 2", DateTime.Today.AddDays(16));
            _evento3 = GetFakeEvento("Churras 3", DateTime.Today.AddDays(24));

            await _service.Insert(_evento1);
            await _service.Insert(_evento2);
            await _service.Insert(_evento3);
        }

        [TestMethod]
        public async Task Insert_Test()
        {
            var evento = GetFakeEvento("Novo Evento de Teste", DateTime.Today.AddDays(42));
            await _service.Insert(evento);

            var token = await _userProvider.GetToken();

            evento.Key.Should().BeGreaterThan(0);
            evento.DonoDaCasaKey.Should().Be(token.Key);

            Console.WriteLine(evento);
        }

        [TestMethod]
        public async Task Validation_Required_Test()
        {
            try
            {
                var evento = new Evento();
                await _service.Insert(evento);

                Assert.Fail("Deixou incluir um Evento sem Nome e/ou sem Dono!");
            }
            catch (ValidatorException ex)
            {
                ex.Errors.Count.Should().Be(1);

                ex.Errors.Should().Contain(x => x.Message == "Nome do Evento é Obrigatório!");

                foreach (var error in ex.Errors)
                    Console.WriteLine(error.Message);
            }
        }

        [TestMethod]
        public async Task Validation_StringLength_Test()
        {
            try
            {
                var evento = GetFakeEvento(
                    "0123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789X",
                    DateTime.Today.AddDays(21));

                await _service.Insert(evento);

                Assert.Fail("Deixou incluir um Evento com nome maior que 100 chars!");
            }
            catch (ValidatorException ex)
            {
                ex.Errors.Count.Should().Be(1);
                ex.Errors.Should().Contain(x => x.Message == "Nome do Evento deve ter no máximo 100 caracteres!");

                foreach (var error in ex.Errors)
                    Console.WriteLine(error.Message);
            }
        }

        [TestMethod]
        public async Task Validation_DoisEventosNoMesmoDia_Test()
        {
            try
            {
                var evento1 = GetFakeEvento();
                var evento2 = GetFakeEvento();

                await _service.Insert(evento1);
                await _service.Insert(evento2);

                Assert.Fail("Deixou incluir dois Eventos no mesmo Dia!");
            }
            catch (ValidatorException ex)
            {
                ex.Errors.Count.Should().Be(1);
                ex.Errors.Should().Contain(x => x.Message == "Já existe um Evento nesse Dia!");

                foreach (var error in ex.Errors)
                    Console.WriteLine(error.Message);
            }
        }

        [TestMethod]
        public async Task Update_Test()
        {
            _evento1.Nome = "ALTERADO";
            await _service.Update(_evento1);

            var fromDb = await _service.Get(_evento1.Key);
            fromDb.Nome.Should().Be("ALTERADO");

            Console.WriteLine(fromDb);
        }

        [TestMethod]
        public async Task Delete_Test()
        {
            await _service.Delete(_evento2);

            var fromDb = await _service.Get(_evento2.Key);
            fromDb.Should().BeNull();
        }

        [TestMethod]
        public async Task Get_Test()
        {
            var fromDb = await _service.Get(_evento3.Key);
            fromDb.Should().NotBeNull();
            Console.WriteLine(fromDb);
        }

        [TestMethod]
        public async Task List_Eventos_Futuros_Test()
        {
            // TODO: Refatorar quando migrar para .net6.0
            // Existe um Bug no EFCore 5, que lança exception se usar a
            // seguinte expression:
            //      x => x.Dia > DateTime.Today
            // A explicação do problema está aqui:
            // https://github.com/dotnet/efcore/issues/18589
            // Foi corrigido para a versão 6, mas por enquanto
            // basta fazer assim:
            var today = DateTime.Today;
            var fromDb = await _service.List(x => x.Dia > today);
            ////////////////////////////////////////////////////////

            fromDb.Count.Should().BeGreaterThan(1);
            foreach (var x in fromDb)
                Console.WriteLine(x);
        }

        [TestMethod]
        public async Task ConfirmarPresenca_Test()
        {
            await _service.ConfirmarPresenca(_evento3.Key, "Cerveja e Picanha");

            var fromDb = await _service.Get(_evento3.Key);
            fromDb.ColegasConfirmados.Count.Should().Be(1);
            fromDb.ColegasConfirmados.Should().Contain(x => x.VaiLevar == "Cerveja e Picanha");

            Console.WriteLine(fromDb);
        }

        [TestMethod]
        public async Task CancelarPresenca_Test()
        {
            var novoEvento = GetFakeEvento("Novo Evento", DateTime.Today.AddDays(3));
            await _service.Insert(novoEvento);

            await _service.ConfirmarPresenca(novoEvento.Key, "fome");

            await _service.CancelarPresenca(novoEvento.Key);

            var fromDb = await _service.Get(novoEvento.Key);
            fromDb.ColegasConfirmados.Count.Should().Be(0);
        }

        private static Evento GetFakeEvento(string nome = "Churras da Firma", DateTime? dia = null)
        {
            if (dia == null)
                dia = DateTime.Today.AddDays(7);

            var evento = new Evento
            {
                Nome = nome,
                Tipo = TipoEvento.Churras,
                Dia = dia.Value,
                Periodo = new Periodo
                {
                    Inicio = dia.Value.AddHours(12),
                    Fim = dia.Value.AddHours(18)
                }
            };
            return evento;
        }
    }
}
