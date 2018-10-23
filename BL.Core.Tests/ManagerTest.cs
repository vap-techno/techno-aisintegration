using System;
using System.Collections.Generic;
using AisJson.Lib.DTO.Data;
using AisJson.Lib.DTO.Task;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BL.Core;

namespace BL.Core.Tests
{
    [TestClass]
    public class ManagerTest
    {
        const string conString =
            @"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=TestDapper;Data Source=.\SQLEXPRESS";

        readonly Uri opcUrl = new Uri("opc.tcp://localhost:55000");

        public Manager manager;

        public static string GenerateCid()
        {
            var d = DateTime.Now;
            return $"{d.Year}{d.Month}{d.Day}{d.Hour}{d.Minute}{d.Second}";
        }

        public ManagerTest()
        {
            ManagerOptions options = new ManagerOptions()
            {
                DbConString = conString,
                OpcServerUri = opcUrl
            };

            manager = new Manager(options);
        }


        [TestMethod]
        public void HandleStatusTask_1Request_ValidResponse()
        {
            // Arrange
            var request = new StatusTaskDto()
            {
                Cmd = "STATUS",
                Cid = GenerateCid(),
                Data = new StatusDataDto()
                {
                    Ids = new List<string>() {"201810202054", "201810202055"}
                }

            };

            


            // Act

            // Assert
        }
    }
}
