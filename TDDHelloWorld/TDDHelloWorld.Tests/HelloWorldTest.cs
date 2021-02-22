using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TDDHelloWorld.Domain;

namespace TDDHelloWorld.Tests
{
    [TestClass]
    public class HelloWorldTest
    {
        [TestMethod]
        public void DigaHello_Test()
        {
            var msg = HelloWorld.DigaHello();
            Assert.AreEqual("Hello World!", msg);

            Console.WriteLine(msg);
        }
    }
}
