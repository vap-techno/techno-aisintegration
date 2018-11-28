using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using AisJson.Lib.DTO.Abstract;
using AisJson.Lib.DTO.Response;
using AisJson.Lib.DTO.Status;


namespace Newtonsoft.Tests
{
    [TestClass]
    public class JsonConvertTest
    {
        [TestMethod]
        public void ToObject_1TimeField_DateTimeField()
        {
            // Arrange
            var dt = new DateTime(2018,11,26,01,01,02);

            // Act
            JsonSerializerSettings formatSettings = new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Local,
                DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFK"
            };

            string json = JsonConvert.SerializeObject(dt, formatSettings);
            
            // Assert
            Assert.AreEqual(@"2018-11-26T01:01:02+07:00", json);
        }

        [TestMethod]
        public void ToObject_1TimeFieldJson_SameFieldJson()
        {
            // Arrange
            //string json = "{\"TDT\":\"2018-11-10T15:29:49+07:00\"}";
            string json = "{\"TDT\":\"2018-11-10T15:29:49\"}";


            JsonSerializerSettings formatSettings = new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Local,
                DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFK"
            };


            // Act
            var obj = JObject.Parse(json).ToObject<DtoTestObject>();
            string json2 = JsonConvert.SerializeObject(obj, formatSettings);

            // Assert
            Assert.AreEqual(json, json2);
        }


    }
}
