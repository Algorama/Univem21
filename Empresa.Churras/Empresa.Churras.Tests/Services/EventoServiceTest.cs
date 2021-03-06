using Empresa.Churras.Domain.Model.Entities;
using Empresa.Churras.Domain.Model.Enums;
using Empresa.Churras.Domain.Model.ValueObjects;
using Empresa.Churras.Domain.Services;
using FluentAssertions;
using Kernel.Domain.Model.Providers;
using Kernel.Domain.Model.Validation;
using Kernel.Infra;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Empresa.Churras.Tests.Services
{
    [TestClass]
    public class EventoServiceTest
    {
        private static IUserProvider _userProvider;

        private static EventoService _service;

        private static Colega _colega1;
        private static Colega _colega2;
        private static Colega _colega3;

        private static Evento _evento1;
        private static Evento _evento2;
        private static Evento _evento3;

        [ClassInitialize]
        public static async Task Setup(TestContext context)
        {
            _userProvider = IoC.Get<IUserProvider>();
            _service = IoC.Get<EventoService>();

            var colegaService = IoC.Get<ColegaService>();

            _colega1 = new Colega { Nome = "Colega 001", Endereco = new Endereco { Descricao = "Endereco 001" } };
            _colega2 = new Colega { Nome = "Colega 002", Endereco = new Endereco { Descricao = "Endereco 002" } };
            _colega3 = new Colega { Nome = "Colega 003", Endereco = new Endereco { Descricao = "Endereco 003" } };

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
        public async Task Insert_DonoCasaCriaEvento_Test()
        {
            var evento = GetFakeEvento();

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
                var evento = GetFakeEvento(null, DateTime.Today.AddDays(14));
                await _service.Insert(evento);
                Assert.Fail();

                Assert.Fail("Deixou incluir um evento sem nome");
            }
            catch (ValidatorException ex)
            {
                ex.Errors.Count.Should().Be(1);
                ex.Errors.Should().Contain(x => x.Message == "É necessário dar um Nome ao Evento");

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

                Assert.Fail("Deixou incluir um evento com nome maior que 100 chars");
            }
            catch (ValidatorException ex)
            {
                ex.Errors.Count.Should().Be(1);
                ex.Errors.Should().Contain(x => x.Message == "Nome do Evento deve ter no máximo 100 caracteres");

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
                await _service.Insert(evento1);

                var evento2 = GetFakeEvento();
                await _service.Insert(evento2);

                Assert.Fail("Deixou incluir dois eventos no mesmo Dia!");
            }
            catch (ValidatorException ex)
            {
                ex.Errors.Count.Should().Be(1);
                ex.Errors.Should().Contain(x => x.Message == "Já existe um Evento nessa Dia!");

                foreach (var error in ex.Errors)
                    Console.WriteLine(error.Message);
            }
        }

        [TestMethod]
        public async Task Update_Test()
        {
            Console.WriteLine(_evento1);
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
            fromDb.DonoDaCasa.Should().NotBeNull();
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
        public async Task ConfirmarPresenca_Test()
        {
            await _service.ConfirmarPresenca(_evento3.Key, "Cerveja e Picanha");

            var fromDb = await _service.Get(_evento3.Key);

            fromDb.ColegasConfirmados.Count.Should().Be(1);

            foreach(var confirmacao in fromDb.ColegasConfirmados)
                Console.WriteLine(confirmacao);
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

        private static Evento GetFakeEvento(string name = "Churras da Firma", DateTime? dia = null)
        {
            if(dia == null)
                dia = DateTime.Today.AddDays(7);

            var evento = new Evento
            {
                Nome = name,
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