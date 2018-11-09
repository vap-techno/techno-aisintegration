using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DAL.InMemory;

namespace DAL.InMemory.Tests
{
    [TestClass]
    public class MemoryExchangeTest
    {
        private readonly MemoryExchange _mmf = new MemoryExchange(MemoryFileName.Cmd);

        [TestMethod]
        public void Write_XXX_isTrue()
        {
            // Arrange
            string value = "XXX";

            // Act
            bool res = _mmf.Write(value);

            // Assert
            Assert.IsTrue(res);

        }

        [TestMethod]
        public void Read_mmf_isEqualTest()
        {
            // Arrange
            string value = "Test";

            // Act
            bool res = _mmf.Write(value);
            var content = _mmf.Read();

            // Assert
            Assert.AreEqual("Test", content.Content);
        }
    }
}
