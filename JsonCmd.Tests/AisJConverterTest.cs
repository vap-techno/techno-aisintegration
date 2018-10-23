using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using AisJson.Lib.DTO;
using AisJson.Lib.DTO.Abstract;
using AisJson.Lib.DTO.Task;
using AisJson.Lib.Utils;
using Xunit;
using Newtonsoft.Json;

namespace JsonCmd.Tests
{
    public class AisJConverterTest
    {

        // Налив в АЦ
        [Fact]
        public void Deserialize_1FillIn_Valid()
        {
            // Arrange
            string fileName = "FillInValidCmd.json";
            string path = Path.Combine(Environment.CurrentDirectory, @"..\..\..\", fileName);

            // Act
            string json = File.ReadAllText(path);
            List<IRequestDto> lst = AisJConverter.Deserialize(json);

            // Assert
            Assert.True(lst[0].Validate());
        }

        [Fact]
        public void Deserialize_1FillIn_NotValid()
        {
            // Arrange
            // deserialize JSON directly from a file
            string fileName = "FillInInValidCmd.json";
            string path = Path.Combine(Environment.CurrentDirectory, @"..\..\..\", fileName);

            // Act
            string json = File.ReadAllText(path);
            List<IRequestDto> lst = AisJConverter.Deserialize(json);

            // Assert
            Assert.False(lst[0].Validate());
        }

        // Налив АЦ КМХ
        [Fact]
        public void Deserialize_1FillInMc_Valid()
        {
            // Arrange
            string fileName = "FillInMcValidCmd.json";
            string path = Path.Combine(Environment.CurrentDirectory, @"..\..\..\", fileName);

            // Act
            string json = File.ReadAllText(path);
            List<IRequestDto> lst = AisJConverter.Deserialize(json);

            // Assert
            Assert.True(lst[0].Validate());
        }

        [Fact]
        public void Deserialize_1FillInMc_NotValid()
        {
            // Arrange
            string fileName = "FillInMcInValidCmd.json";
            string path = Path.Combine(Environment.CurrentDirectory, @"..\..\..\", fileName);

            // Act

            string json = File.ReadAllText(path);
            List<IRequestDto> lst = AisJConverter.Deserialize(json);

            // Assert
            Assert.False(lst[0].Validate());
        }

        // Слив из АЦ
        [Fact]
        public void Deserialize_1FillOut_Valid()
        {
            // Arrange
            // deserialize JSON directly from a file
            string fileName = "FillOutValidCmd.json";
            string path = Path.Combine(Environment.CurrentDirectory, @"..\..\..\", fileName);

            // Act
            string json = File.ReadAllText(path);
            List<IRequestDto> lst = AisJConverter.Deserialize(json);

            // Assert
            Assert.True(lst[0].Validate());
        }

        [Fact]
        public void Deserialize_1FillOut_NotValid()
        {
            // Arrange
            // deserialize JSON directly from a file
            string fileName = "FillOutInValidCmd.json";
            string path = Path.Combine(Environment.CurrentDirectory, @"..\..\..\", fileName);

            // Act
            string json = File.ReadAllText(path);
            List<IRequestDto> lst = AisJConverter.Deserialize(json);

            // Assert
            Assert.False(lst[0].Validate());
        }

        // Отмена
        [Fact]
        public void Deserialize_1Cancel_Valid()
        {
            // Arrange
            string fileName = "CancelValidCmd.json";
            string path = Path.Combine(Environment.CurrentDirectory, @"..\..\..\", fileName);

            // Act
            string json = File.ReadAllText(path);
            List<IRequestDto> lst = AisJConverter.Deserialize(json);

            // Assert
            Assert.True(lst[0].Validate());
        }

        [Fact]
        public void Deserialize_1Cancel_NotValid()
        {
            // Arrange
            string fileName = "CancelInValidCmd.json";
            string path = Path.Combine(Environment.CurrentDirectory, @"..\..\..\", fileName);

            // Act
            string json = File.ReadAllText(path);
            List<IRequestDto> lst = AisJConverter.Deserialize(json);

            // Assert
            Assert.False(lst[0].Validate());
        }

        // Статус
        [Fact]
        public void Deserialize_1Status_Valid()
        {
            // Arrange
            string fileName = "StatusValidCmd.json";
            string path = Path.Combine(Environment.CurrentDirectory, @"..\..\..\", fileName);

            // Act
            string json = File.ReadAllText(path);
            List<IRequestDto> lst = AisJConverter.Deserialize(json);

            // Assert
            Assert.True(lst[0].Validate());
        }

        [Fact]
        public void Deserialize_1Status_NotValid()
        {
            // Arrange
            string fileName = "StatusInValidCmd.json";
            string path = Path.Combine(Environment.CurrentDirectory, @"..\..\..\", fileName);

            // Act
            string json = File.ReadAllText(path);
            List<IRequestDto> lst = AisJConverter.Deserialize(json);

            // Assert
            Assert.False(lst[0].Validate());
        }

        // Неизвестная команда
        [Fact]
        public void Deserialize_UnknownCmd_Exception()
        {
            // Arrange
            string json = @"[{
                'CMD':'WRONG CMD', 
                'CID': '1234', 
            }
            ]";

            // Act
            Exception ex = Assert.Throws<Exception>(() => AisJConverter.Deserialize(json));

            // Assert
            Assert.Equal("Unknown command", ex.Message);
        }

        // Нет поля CID
        [Fact]
        public void Deserialize_NoCidfieldInJson_Exception()
        {
            // Arrange
            string json = @"[{
                'CMD':'STATUS', 
                'IDs':['1','2','3','4'],
                'TNo': 'Bla bla bla'
            }
            ]";

            // Act
            Action act = () => AisJConverter.Deserialize(json);

            // Assert
            Assert.Throws<JsonSerializationException>(act);

        }

    }
}
