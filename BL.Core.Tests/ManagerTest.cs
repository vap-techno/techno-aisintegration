using System;
using System.Collections.Generic;
using System.IO;
using AisJson.Lib.DTO.Data;
using AisJson.Lib.DTO.Task;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AisOpcClient;
using AisOpcClient.Lib;
using BL.Core;
using DAL.Core.TaskMapper;
using Serilog;
using Serilog.Exceptions;

namespace BL.Core.Tests
{

    // TODO: Доделать тесты
    [TestClass]
    public class ManagerTest
    {
        const string conString =
            @"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=TestDapper;Data Source=.\SQLEXPRESS";

        readonly Uri opcUrl = new Uri("opc.tcp://localhost:55000");

        public Manager manager;

        public OpcService opcService;

        public static string GenerateCid()
        {
            var d = DateTime.Now;
            return $"{d.Year}{d.Month}{d.Day}{d.Hour}{d.Minute}{d.Second}";
        }

        public ManagerTest()
        {

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.WithExceptionDetails()
                .CreateLogger();

            var taskMapper = new TaskMapper();

            opcService = new OpcService(opcUrl, Log.Logger);

            //manager = new Manager(conString, taskMapper, opcService, Log.Logger);
        }


        [TestMethod]
        public void HandleStatusTask_1Request_ResponseWithoutNullableFields()
        {
            // Arrange
            string conString = $@"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=AisIntegrationDb;Data Source=.\SQLEXPRESS";
            var taskMapper = new TaskMapper();
            manager = new BL.Core.Manager(conString, taskMapper, Log.Logger);

            string fileName = "StatusValidCmd.json";
            string path = Path.Combine(Environment.CurrentDirectory, @"..\..\", fileName);

            // Act
            string json = File.ReadAllText(path);
            string request = manager.HandleRequest(json);

            // Assert
            Assert.IsFalse(request.Contains("TAdj"));
        }
    }
}
