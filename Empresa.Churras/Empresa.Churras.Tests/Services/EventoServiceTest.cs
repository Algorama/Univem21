using Empresa.Churras.Domain.Model.Entities;
using Empresa.Churras.Domain.Model.Enums;
using Empresa.Churras.Domain.Model.ValueObjects;
using Empresa.Churras.Domain.Services;
using FluentAssertions;
using Kernel.Domain.Model.Providers;
using Kernel.Infra;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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
        }

        [TestMethod]
        public async Task Insert_DonoCasaCriaEvento_Test()
        {
            var dataEvento = DateTime.Today.AddDays(7);
            var evento = new Evento
            {
                Nome = "Churras da Firma",
                Tipo = TipoEvento.Churras,
                Dia = dataEvento,
                Periodo = new Periodo
                {
                    Inicio = dataEvento.AddHours(12),
                    Fim = dataEvento.AddHours(18)
                }
            };

            await _service.Insert(evento);

            var token = await _userProvider.GetToken();

            evento.Key.Should().BeGreaterThan(0);
            evento.DonoDaCasaKey.Should().Be(token.Key);

            Console.WriteLine(evento);
        }

        
    }
}