using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AisJson.Lib.DTO.Abstract;
using AisJson.Lib.DTO.Status;
using AisJson.Lib.DTO.Task;
using AisJson.Lib.Utils;
using DAL.Entity.Abstract;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoMapper;
using DAL.Entity;
using DAL.Core.TaskMapper;
using DAL.Entity.Status;
using Serilog;
using Serilog.Exceptions;

namespace DAL.Core.Tests
{

    [TestClass]
    public class TaskMapperTest
    {
        private TaskMapper.TaskMapper _mapper;


        public TaskMapperTest()
        {

            _mapper = new TaskMapper.TaskMapper();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.WithExceptionDetails()
                .CreateLogger();



        }

        [TestMethod]
        public void FillInMapping_1DTO_1Entity()
        {
            // Arrange
            string fileName = "FillInValidCmd.json";
            string path = Path.Combine(Environment.CurrentDirectory, @"..\..\", fileName);
           
            
            // Act
            string json = File.ReadAllText(path);
            List<IRequestDto> taskListDto = AisJConverter.Deserialize(json, Log.Logger);
            var task = Mapper.Map<FillInTask>(taskListDto[0]);
            
            // Assert
            Assert.IsTrue(task.Details.Count > 0 && task.Details[2].Pmp > 0);
        }

        [TestMethod]
        public void FillInMcMapping_1DTO_1Entity()
        {
            // Arrange
            string fileName = "FillInMcValidCmd.json";
            string path = Path.Combine(Environment.CurrentDirectory, @"..\..\", fileName);
           

            // Act
            string json = File.ReadAllText(path);
            List<IRequestDto> taskListDto = AisJConverter.Deserialize(json, Log.Logger);
            var task = Mapper.Map<FillInMcTask>(taskListDto[0]);

            // Assert
            Assert.AreEqual("Пост1", task.Lnp);
        }

        [TestMethod]
        public void FillOutMapping_1DTO_1Entity()
        {
            // Arrange
            string fileName = "FillOutValidCmd.json";
            string path = Path.Combine(Environment.CurrentDirectory, @"..\..\", fileName);
            

            // Act
            string json = File.ReadAllText(path);
            List<IRequestDto> taskListDto = AisJConverter.Deserialize(json, Log.Logger);
            var task = Mapper.Map<FillOutTask>(taskListDto[0]);

            // Assert
            Assert.IsTrue(task.Details.Count > 0 && task.Details[0].Pn == "АИ98");
        }

        [TestMethod]
        public void FIllInMapping_1EntityTask_1EntityStatusDetail()
        {
            // Arrange
            string fileName = "FillInValidCmd.json";
            string path = Path.Combine(Environment.CurrentDirectory, @"..\..\", fileName);
            

            // Act
            string json = File.ReadAllText(path);
            List<IRequestDto> taskListDto = AisJConverter.Deserialize(json, Log.Logger);
            List<FillInTask> taskList = new List<FillInTask>();
            taskListDto.ForEach(item => taskList.Add(Mapper.Map<FillInTask>(item)));

            List<FillInStatusDetail> dList = new List<FillInStatusDetail>();
            taskList.ForEach(item => dList.Add(Mapper.Map<FillInStatusDetail>(item)));

            // Assert
            Assert.AreEqual(3, dList[0].Details.Count);

        }

        [TestMethod]
        public void FIllInMcMapping_1EntityTask_1EntityStatusDetail()
        {
            // Arrange
            string fileName = "FillInMcValidCmd.json";
            string path = Path.Combine(Environment.CurrentDirectory, @"..\..\", fileName);
            

            // Act
            string json = File.ReadAllText(path);
            List<IRequestDto> taskListDto = AisJConverter.Deserialize(json, Log.Logger);
            List<FillInMcTask> taskList = new List<FillInMcTask>();
            taskListDto.ForEach(item => taskList.Add(Mapper.Map<FillInMcTask>(item)));

            List<FillInMcStatusDetail> dList = new List<FillInMcStatusDetail>();
            taskList.ForEach(item => dList.Add(Mapper.Map<FillInMcStatusDetail>(item)));

            // Assert
            Assert.AreEqual("", dList[0].Lnf);

        }

        [TestMethod]
        public void FIllOutMapping_1EntityTask_1EntityStatusDetail()
        {
            // Arrange
            string fileName = "FillOutValidCmd.json";
            string path = Path.Combine(Environment.CurrentDirectory, @"..\..\", fileName);
            

            // Act
            string json = File.ReadAllText(path);
            List<IRequestDto> taskListDto = AisJConverter.Deserialize(json, Log.Logger);
            List<FillOutTask> taskList = new List<FillOutTask>();
            taskListDto.ForEach(item => taskList.Add(Mapper.Map<FillOutTask>(item)));

            List<FillOutStatusDetail> dList = new List<FillOutStatusDetail>();
            taskList.ForEach(item => dList.Add(Mapper.Map<FillOutStatusDetail>(item)));

            // Assert
            Assert.AreEqual(1, dList[0].Details.Count);

        }

        [TestMethod]
        public void FIllInMapping_1StatusDetail_1StatusDetailDto()
        {
            // Arrange
            string fileName = "FillInValidCmd.json";
            string path = Path.Combine(Environment.CurrentDirectory, @"..\..\", fileName);
            

            string json = File.ReadAllText(path);
            List<IRequestDto> taskListDto = AisJConverter.Deserialize(json, Log.Logger);
            List<FillInTask> taskList = new List<FillInTask>();
            taskListDto.ForEach(item => taskList.Add(Mapper.Map<FillInTask>(item)));

            List<FillInStatusDetail> sList = new List<FillInStatusDetail>();
            taskList.ForEach(item => sList.Add(Mapper.Map<FillInStatusDetail>(item)));

            // Act
            var sListDto = new List<FillInStatusDetail>();
            sList.ForEach(item => sListDto.Add(Mapper.Map<FillInStatusDetail>(item)));

            // Assert
            Assert.AreEqual(3, sListDto[0].Details.Count);

        }

        [TestMethod]
        public void FIllInMcMapping_1StatusDetail_1StatusDetailDto()
        {
            // Arrange
            string fileName = "FillInMcValidCmd.json";
            string path = Path.Combine(Environment.CurrentDirectory, @"..\..\", fileName);
            

            string json = File.ReadAllText(path);
            List<IRequestDto> taskListDto = AisJConverter.Deserialize(json, Log.Logger);
            List<FillInMcTask> taskList = new List<FillInMcTask>();
            taskListDto.ForEach(item => taskList.Add(Mapper.Map<FillInMcTask>(item)));

            List<FillInMcStatusDetail> sList = new List<FillInMcStatusDetail>();
            taskList.ForEach(item => sList.Add(Mapper.Map<FillInMcStatusDetail>(item)));

            // Act
            var sListDto = new List<FillInMcStatusDetail>();
            sList.ForEach(item => sListDto.Add(Mapper.Map<FillInMcStatusDetail>(item)));

            // Assert
            Assert.AreEqual("", sListDto[0].Lnf);

        }

        [TestMethod]
        public void FillOutMapping_1StatusDetail_1StatusDetailDto()
        {
            // Arrange
            string fileName = "FillOutValidCmd.json";
            string path = Path.Combine(Environment.CurrentDirectory, @"..\..\", fileName);
            

            string json = File.ReadAllText(path);
            List<IRequestDto> taskListDto = AisJConverter.Deserialize(json, Log.Logger);
            List<FillOutTask> taskList = new List<FillOutTask>();
            taskListDto.ForEach(item => taskList.Add(Mapper.Map<FillOutTask>(item)));

            List<FillOutStatusDetail> sList = new List<FillOutStatusDetail>();
            taskList.ForEach(item => sList.Add(Mapper.Map<FillOutStatusDetail>(item)));

            // Act
            var sListDto = new List<FillOutStatusDetail>();
            sList.ForEach(item => sListDto.Add(Mapper.Map<FillOutStatusDetail>(item)));

            // Assert
            Assert.AreEqual(1, sListDto[0].Details.Count);

        }

    }
}
