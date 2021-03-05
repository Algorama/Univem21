using Empresa.Churras.Domain.Model.Entities;
using Kernel.Domain.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using Empresa.Churras.Domain.Model.ValueObjects;
using System;

namespace Empresa.Churras.Tests.Entities
{
    [TestClass]
    public class ColegaTest
    {
        [TestMethod]
        public void Nome_Required_Test()
        {
            var colega = new Colega 
            { 
                //Nome = "",
                //Endereco = new Endereco
                //{
                //    Descricao = "Teste"
                //}
            };

            var result = new ValidatorResult();
            result.ValidateAnnotations(colega);

            result.Errors.Should().Contain(x => x.Message == "Nome é Obrigatório!");

            foreach(var error in result.Errors)
                Console.WriteLine(error.Message);
        }
    }
}
