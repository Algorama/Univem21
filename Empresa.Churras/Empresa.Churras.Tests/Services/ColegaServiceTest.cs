using Empresa.Churras.Domain.Model.Entities;
using Empresa.Churras.Domain.Services;
using FluentAssertions;
using Kernel.Domain.Model.Validation;
using Kernel.Infra;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Empresa.Churras.Tests.Services
{
    [TestClass]
    public class ColegaServiceTest
    {
        private static ColegaService _service;

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            _service = IoC.Get<ColegaService>();
        }

        [TestMethod]
        public async Task Insert_Validatio_Required_Test()
        {
            try
            {
                var colega = new Colega();
                await _service.Insert(colega);

                Assert.Fail();
            }
            catch(ValidatorException ex)
            {
                ex.Errors.Count.Should().Be(2);

                ex.Errors.Should().Contain(x => x.Message == "Nome é Obrigatório!");
                ex.Errors.Should().Contain(x => x.Message == "Descrição do Endereço é Obrigatória");

                foreach (var error in ex.Errors)
                    Console.WriteLine(error.Message);
            }
        }
    }
}
