using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Dapper.Contrib.Extensions;
using DAL.Entity;

namespace DAL.UnitOfWork
{
    public class FIllOutTaskRepository : IRepository<FillOutTask>
    {

        public readonly string ConString; // Строка подключения к базе данных

        public FIllOutTaskRepository(string con)
        {
            ConString = con;
        }

        public IList<FillOutTask> GetAll()
        {
            List<FillOutTask> taskList;

            string sqlQueryTask =
                @"SELECT * FROM FillOutTask INNER JOIN FillOutDetail ON FillOutDetail.FillOutTaskId = FillOutTask.FillOutTaskId";

            using (var connection = new SqlConnection(ConString))
            {
                var taskDict = new Dictionary<long, FillOutTask>();

                var list = connection.Query<FillOutTask, FillOutDetail, FillOutTask>(sqlQueryTask, (task, taskDetail) =>
                {
                    if (!taskDict.TryGetValue(task.FillOutTaskId, out var taskEntry))
                    {
                        taskEntry = task;
                        taskEntry.Details = new List<FillOutDetail>();
                        taskDict.Add(taskEntry.FillOutTaskId, taskEntry);
                    }

                    taskEntry.Details.Add(taskDetail);

                    return taskEntry;
                }, splitOn: "FillOutTaskId,FillOutDetailId").Distinct().ToList();


                taskList = list;
            }

            return taskList;
        }

        public FillOutTask GetItem(long id)
        {
            FillOutTask item;

            string sqlQueryTask =
                $@"SELECT * FROM FillOutTask INNER JOIN FillOutDetail ON FillOutDetail.FillOutTaskId = FillOutTask.FillOutTaskId WHERE FillOutTask.FillOutTaskId = {id}";

            using (var connection = new SqlConnection(ConString))
            {
                var taskDict = new Dictionary<long, FillOutTask>();

                var list = connection.Query<FillOutTask, FillOutDetail, FillOutTask>(sqlQueryTask, (task, taskDetail) =>
                {
                    if (!taskDict.TryGetValue(task.FillOutTaskId, out var taskEntry))
                    {
                        taskEntry = task;
                        taskEntry.Details = new List<FillOutDetail>();
                        taskDict.Add(taskEntry.FillOutTaskId, taskEntry);
                    }

                    taskEntry.Details.Add(taskDetail);

                    return taskEntry;
                }, splitOn: "FillOutTaskId,FillOutDetailId").Distinct().ToList();


                item = list.First();
            }

            return item;

        }

        public FillOutTask GetItem(string aisTaskId)
        {
            FillOutTask item;

            string sqlQueryTask =
                $@"SELECT * FROM FillOutTask INNER JOIN FillOutDetail ON FillOutDetail.FillOutTaskId = FillOutTask.FillOutTaskId WHERE FillOutTask.AisTaskId = '{aisTaskId}'";

            using (var connection = new SqlConnection(ConString))
            {
                var taskDict = new Dictionary<long, FillOutTask>();

                var list = connection.Query<FillOutTask, FillOutDetail, FillOutTask>(sqlQueryTask, (task, taskDetail) =>
                {
                    if (!taskDict.TryGetValue(task.FillOutTaskId, out var taskEntry))
                    {
                        taskEntry = task;
                        taskEntry.Details = new List<FillOutDetail>();
                        taskDict.Add(taskEntry.FillOutTaskId, taskEntry);
                    }

                    taskEntry.Details.Add(taskDetail);

                    return taskEntry;
                }, splitOn: "FillOutTaskId,FillOutDetailId").Distinct().ToList();


                item = list.FirstOrDefault();
            }

            return item;
        }

        public long Create(FillOutTask item)
        {
            long id;
            item.Ts = DateTime.Now;

            using (var connection = new SqlConnection(ConString))
            {
                id = connection.Insert<FillOutTask>(item);
                foreach (var d in item.Details)
                {
                    d.FillOutTaskId = id;
                    d.Ts = DateTime.Now;
                    var dId = connection.Insert<FillOutDetail>(d);
                }
            }
            
            return id;
        }

        public bool Update(FillOutTask item)
        {
            // Ищем ID в таблице, которая содержит заданный CID
            // TODO: Запроc пойдет только с маленькой базой

            var tasks = GetAll();
            var task = (from t in tasks
                where t.Cid == item.Cid
                select t).FirstOrDefault();

            bool result;

            using (var connection = new SqlConnection(ConString))
            {
                var t = item;
                if (task != null)
                {
                    t.FillOutTaskId = task.FillOutTaskId;
                    result = connection.Update(t);
                }
                else
                {
                    result = false;
                }
            }

            return result;
        }

        public bool Delete(long id)
        {
            bool res = false;
            
            // Извлекаем запись в таблице FillOutTask
            var task = GetItem(id);
            if (task == null) return false;

            // Вначале удаляем записи в FillOutDetail, затем можно удалить запись из FillOutTask
            using (var connection = new SqlConnection(ConString))
            {
                var isSuccess = connection.Delete(task.Details);
                if (isSuccess)
                {
                    res = connection.Delete(task);
                }
            }

            return res;
        }
    }
}
