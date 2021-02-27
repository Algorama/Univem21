using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Univem.Churras.Domain.Model.Entities;

namespace Univem.Churras.Tests
{
    [TestClass]
    public class EventoTest
    {
        [TestMethod]
        public void ConfirmarPresenca_Test()
        {
            var colega = new Colega
            {
                Key = 1,
                Nome = "Tião"
            };

            var evento = new Evento { Nome = "Churras de fim de ano" };

            evento.ConfirmarPresenca(colega);

            var confirmacao = evento.ColegasConfirmados.FirstOrDefault();
            Assert.AreEqual(colega.Nome, confirmacao.ColegaNome);
        }

        [TestMethod]
        public void CancelarPresenca_Test()
        {
            var colega = new Colega
            {
                Key = 1,
                Nome = "Tião"
            };

            var evento = new Evento { Nome = "Churras de fim de ano" };

            evento.ConfirmarPresenca(colega);
            evento.CancelarPresenca(colega);

            Assert.AreEqual(0, evento.ColegasConfirmados.Count);
        }
    }
}
