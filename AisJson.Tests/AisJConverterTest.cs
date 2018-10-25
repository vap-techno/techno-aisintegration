using System;
using System.Collections.Generic;
using System.IO;
using AisJson.Lib.DTO.Abstract;
using AisJson.Lib.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog;
using Serilog.Exceptions;

namespace AisJson.Tests
{
    [TestClass]
    public class AisJConverterTest
    {

        public AisJConverterTest()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.WithExceptionDetails()
                .CreateLogger();
        }
        
        
        // Налив в АЦ
        [TestMethod]
        public void Deserialize_1FillIn_Valid()
        {
            // Arrange
            string fileName = "FillInValidCmd.json";
            string path = Path.Combine(Environment.CurrentDirectory, @"..\..\", fileName);

            // Act
            string json = File.ReadAllText(path);
            List<IRequestDto> lst = AisJConverter.Deserialize(json, Log.Logger);

            // Assert
            Assert.IsTrue(lst[0].Validate());
        }

        [TestMethod]
        public void Deserialize_1FillIn_NotValid()
        {
            // Arrange
            // deserialize JSON directly from a file
            string fileName = "FillInInValidCmd.json";
            string path = Path.Combine(Environment.CurrentDirectory, @"..\..\", fileName);

            // Act
            string json = File.ReadAllText(path);
            List<IRequestDto> lst = AisJConverter.Deserialize(json, Log.Logger);

            // Assert
            Assert.IsFalse(lst[0].Validate());
        }

        // Налив АЦ КМХ
        [TestMethod]
        public void Deserialize_1FillInMc_Valid()
        {
            // Arrange
            string fileName = "FillInMcValidCmd.json";
            string path = Path.Combine(Environment.CurrentDirectory, @"..\..\", fileName);

            // Act
            string json = File.ReadAllText(path);
            List<IRequestDto> lst = AisJConverter.Deserialize(json, Log.Logger);

            // Assert
            Assert.IsTrue(lst[0].Validate());
        }

        [TestMethod]
        public void Deserialize_1FillInMc_NotValid()
        {
            // Arrange
            string fileName = "FillInMcInValidCmd.json";
            string path = Path.Combine(Environment.CurrentDirectory, @"..\..\", fileName);

            // Act

            string json = File.ReadAllText(path);
            List<IRequestDto> lst = AisJConverter.Deserialize(json, Log.Logger);

            // Assert
            Assert.IsFalse(lst[0].Validate());
        }

        // Слив из АЦ
        [TestMethod]
        public void Deserialize_1FillOut_Valid()
        {
            // Arrange
            // deserialize JSON directly from a file
            string fileName = "FillOutValidCmd.json";
            string path = Path.Combine(Environment.CurrentDirectory, @"..\..\", fileName);

            // Act
            string json = File.ReadAllText(path);
            List<IRequestDto> lst = AisJConverter.Deserialize(json, Log.Logger);

            // Assert
            Assert.IsTrue(lst[0].Validate());
        }

        [TestMethod]
        public void Deserialize_1FillOut_NotValid()
        {
            // Arrange
            // deserialize JSON directly from a file
            string fileName = "FillOutInValidCmd.json";
            string path = Path.Combine(Environment.CurrentDirectory, @"..\..\", fileName);

            // Act
            string json = File.ReadAllText(path);
            List<IRequestDto> lst = AisJConverter.Deserialize(json, Log.Logger);

            // Assert
            Assert.IsFalse(lst[0].Validate());
        }

        // Отмена
        [TestMethod]
        public void Deserialize_1Cancel_Valid()
        {
            // Arrange
            string fileName = "CancelValidCmd.json";
            string path = Path.Combine(Environment.CurrentDirectory, @"..\..\", fileName);

            // Act
            string json = File.ReadAllText(path);
            List<IRequestDto> lst = AisJConverter.Deserialize(json, Log.Logger);

            // Assert
            Assert.IsTrue(lst[0].Validate());
        }

        [TestMethod]
        public void Deserialize_1Cancel_NotValid()
        {
            // Arrange
            string fileName = "CancelInValidCmd.json";
            string path = Path.Combine(Environment.CurrentDirectory, @"..\..\", fileName);

            // Act
            string json = File.ReadAllText(path);
            List<IRequestDto> lst = AisJConverter.Deserialize(json, Log.Logger);

            // Assert
            Assert.IsFalse(lst[0].Validate());
        }

        // Статус
        [TestMethod]
        public void Deserialize_1Status_Valid()
        {
            // Arrange
            string fileName = "StatusValidCmd.json";
            string path = Path.Combine(Environment.CurrentDirectory, @"..\..\", fileName);

            // Act
            string json = File.ReadAllText(path);
            List<IRequestDto> lst = AisJConverter.Deserialize(json, Log.Logger);

            // Assert
            Assert.IsTrue(lst[0].Validate());
        }

        [TestMethod]
        public void Deserialize_1Status_NotValid()
        {
            // Arrange
            string fileName = "StatusInValidCmd.json";
            string path = Path.Combine(Environment.CurrentDirectory, @"..\..\", fileName);

            // Act
            string json = File.ReadAllText(path);
            List<IRequestDto> lst = AisJConverter.Deserialize(json, Log.Logger);

            // Assert
            Assert.IsFalse(lst[0].Validate());
        }

        // Неизвестная команда
        [TestMethod]
        public void Deserialize_UnknownCmd_EmptyCmdList()
        {
            // Arrange
            string json = @"[{
                'CMD':'WRONG CMD', 
                'CID': '1234', 
            }
            ]";

            // Act
            var list = AisJConverter.Deserialize(json, Log.Logger);

            // Assert
            Assert.AreEqual(0, list.Count);
        }

        // Нет поля CID
        [TestMethod]
        public void Deserialize_NoCidfieldInJson_EmptyCmdList()
        {
            // Arrange
            string json = @"[{
                'CMD':'STATUS', 
                'IDs':['1','2','3','4'],
                'TNo': 'Bla bla bla'
            }
            ]";

            // Act
            var list = AisJConverter.Deserialize(json, Log.Logger);

            // Assert
            Assert.AreEqual(0, list.Count);
            

        }
    }
}
