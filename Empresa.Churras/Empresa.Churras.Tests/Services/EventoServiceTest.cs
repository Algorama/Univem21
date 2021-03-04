using Empresa.Churras.Domain.Services;
using Kernel.Infra;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Empresa.Churras.Tests.Services
{
    [TestClass]
    public class EventoServiceTest
    {
        private static EventoService _service;

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            _service = IoC.Get<EventoService>();
        }

        //[TestMethod]
        //public void 
    }
}
