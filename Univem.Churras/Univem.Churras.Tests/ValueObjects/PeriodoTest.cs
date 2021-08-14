using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Univem.Churras.Domain.Model.ValueObjects;

namespace Univem.Churras.Tests.ValueObjects
{
    [TestClass]
    public class PeriodoTest
    {
        [TestMethod]
        public void QuantoDuraEmHoras_Test()
        {
            var periodo = new Periodo
            {
                Inicio = new DateTime(2021, 02, 28, 12, 0, 0),
                Fim = new DateTime(2021, 02, 28, 18, 0, 0)
            };

            int horas = periodo.QuantoDuraEmHoras();

            Assert.AreEqual(6, horas);
        }

        [TestMethod]
        public void FaltaQuantoTempoParaComecar_Test()
        {
            var dtInicio = DateTime.Now.AddDays(3).AddHours(5);
            var dtFim = dtInicio.AddHours(6);

            var periodo = new Periodo
            {
                Inicio = dtInicio,
                Fim = dtFim
            };

            string quantoFalta = periodo.QuantoFaltaParaComecar();

            Assert.AreEqual("Começa em 3 dias e 4 horas", quantoFalta);
        }

        [TestMethod]
        public void FaltaQuantoTempoParaComecar_Horas_Test()
        {
            var dtInicio = DateTime.Now.AddHours(5);
            var dtFim = dtInicio.AddHours(2);

            var periodo = new Periodo
            {
                Inicio = dtInicio,
                Fim = dtFim
            };

            string quantoFalta = periodo.QuantoFaltaParaComecar();

            Assert.AreEqual("Começa em 4 horas", quantoFalta);
        }
    }
}
