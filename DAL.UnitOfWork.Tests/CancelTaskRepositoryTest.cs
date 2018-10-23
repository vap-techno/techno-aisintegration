using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DAL.UnitOfWork.Tests
{
    [TestClass]
    public class CancelTaskRepositoryTest
    {
        const string conString =
            @"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=TestDapper;Data Source=.\SQLEXPRESS";

        private readonly CancelTaskRepository _rep;

        public static string GenerateCid()
        {
            var d = DateTime.Now;
            return $"{d.Year}{d.Month}{d.Day}{d.Hour}{d.Minute}{d.Second}";
        }

        public CancelTaskRepositoryTest()
        {
            _rep = new CancelTaskRepository(conString);
        }

        [TestMethod]
        public void Create_1CancelCmd_NoZeroId()
        {
            // Arrange
            // TODO: Заменить на Mock
            var task = new CancelTask()
            {
                Cid = GenerateCid(),
                Ids = new List<string> { "1", "2", "3" }
            };


            // Act
            var id = _rep.Create(task);

            // Assert
            Assert.IsTrue(id > 0);

        }

        [TestMethod]
        public void GetItem_GetIdsFromLastId_Ids()
        {
            // Arrange

            // Act
            var tasks = _rep.GetAll();
            var task = _rep.GetItem(tasks.LastOrDefault()?.CancelTaskId ?? 0);

            // Assert
            Assert.IsTrue(task.Ids != null);
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
        public void Update_ChangeIds_IdsChanged()
        {
            // Arrange

            // Act
            var tasks = _rep.GetAll();
            
            var task = tasks.Last();
            task.Ids = new List<string>() { "1", "2", "99" };

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

            if (task != null) res = _rep.Delete(task.CancelTaskId);

            // Assert
            Assert.IsTrue(res);

        }
    }
}
