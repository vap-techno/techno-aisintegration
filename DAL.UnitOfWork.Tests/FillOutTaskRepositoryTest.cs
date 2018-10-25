using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AisJson.Lib.DTO.Abstract;
using AisJson.Lib.Utils;
using AutoMapper;
using DAL.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog;
using Serilog.Exceptions;

namespace DAL.UnitOfWork.Tests
{
    [TestClass]
    public class FillOutTaskRepositoryTest
    {

        const string conString =
            @"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=TestDapper;Data Source=.\SQLEXPRESS";

        Core.TaskMapper.TaskMapper mapper = new Core.TaskMapper.TaskMapper();

        private readonly FIllOutTaskRepository _rep;

        public FillOutTaskRepositoryTest()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.WithExceptionDetails()
                .CreateLogger();

            _rep = new FIllOutTaskRepository(conString, Log.Logger);
        }

        public static string GenerateCid()
        {
            var d = DateTime.Now;
            return $"{d.Year}{d.Month}{d.Day}{d.Hour}{d.Minute}{d.Second}";
        }

        [TestMethod]
        public void Create_1Task_NoZeroId()
        {
            // Arrange
            string fileName = "FillOutValidCmd.json";
            string path = Path.Combine(Environment.CurrentDirectory, @"..\..\", fileName);

            string json = File.ReadAllText(path);
            List<IRequestDto> taskListDto = AisJConverter.Deserialize(json, Log.Logger);
            var task = Mapper.Map<FillOutTask>(taskListDto[0]);

            // Act
            var id = _rep.Create(task);

            // Assert
            Assert.IsTrue(id > 0);
        }

        [TestMethod]
        public void GetItem_GetIdsFromLastId_DetailsCountHigherZero()
        {
            // Arrange

            // Act
            var tasks = _rep.GetAll();
            var task = _rep.GetItem(tasks.LastOrDefault()?.FillOutTaskId ?? 0);

            // Assert
            Assert.IsTrue(task.Details != null && task.Details.Count > 0);
        }

        [TestMethod]
        public void GetAll_GetCountOfTasks_HigherZero()
        {
            // Arrange

            // Act
            var tasks = _rep.GetAll();

            // Assert
            Assert.IsTrue(tasks != null && tasks.Count > 0);
        }

        // 
        [TestMethod]
        public void Update_ChangeOn_OnChanged()
        {
            // Arrange

            // Act
            var tasks = _rep.GetAll();

            var task = tasks.Last();
            task.On = "Василий Алибабаевич";

            var res = _rep.Update(task);

            // Assert
            Assert.IsTrue(res);

        }

        // 
        [TestMethod]
        public void Update_ChangeFs_FsChanged()
        {
            // Arrange

            // Act
            var tasks = _rep.GetAll();

            var task = tasks.Last();
            task.Details[0].Fs = 2;

            var res = _rep.Update(task);

            // Assert
            Assert.IsTrue(res);

        }

        [TestMethod]
        public void Delete_Delete1Item_CountChanged()
        {
            // Arrange
            var tasks = _rep.GetAll();
            var task = tasks.LastOrDefault();
            var res = false;

            // Act

            if (task != null) res = _rep.Delete(task.FillOutTaskId);

            // Assert
            Assert.IsTrue(res);

        }
    }
}