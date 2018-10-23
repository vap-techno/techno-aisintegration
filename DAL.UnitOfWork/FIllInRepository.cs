using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Dapper.Contrib.Extensions;
using DAL.Entity;

namespace DAL.UnitOfWork
{
    public class FIllInTaskRepository : IRepository<FillInTask>
    {

        public readonly string ConString; // Строка подключения к базе данных

        public FIllInTaskRepository(string con)
        {
            ConString = con;
        }

        public IList<FillInTask> GetAll()
        {
            List<FillInTask> taskList;

            string sqlQueryTask =
                @"SELECT * FROM FillInTask INNER JOIN FillInDetail ON FillInDetail.FillInTaskId = FillInTask.FillInTaskId";

            using (var connection = new SqlConnection(ConString))
            {
                var taskDict = new Dictionary<long, FillInTask>();

                var list = connection.Query<FillInTask, FillInDetail, FillInTask>(sqlQueryTask, (task, taskDetail) =>
                {
                    if (!taskDict.TryGetValue(task.FillInTaskId, out var taskEntry))
                    {
                        taskEntry = task;
                        taskEntry.Details = new List<FillInDetail>();
                        taskDict.Add(taskEntry.FillInTaskId, taskEntry);
                    }

                    taskEntry.Details.Add(taskDetail);

                    return taskEntry;
                }, splitOn: "FillInTaskId,FillInDetailId").Distinct().ToList();


                taskList = list;
            }

            return taskList;
        }

        public FillInTask GetItem(long id)
        {
            FillInTask item;

            string sqlQueryTask =
                $@"SELECT * FROM FillInTask INNER JOIN FillInDetail ON FillInDetail.FillInTaskId = FillInTask.FillInTaskId WHERE FillInTask.FillInTaskId = {id}";

            using (var connection = new SqlConnection(ConString))
            {
                var taskDict = new Dictionary<long, FillInTask>();

                var list = connection.Query<FillInTask, FillInDetail, FillInTask>(sqlQueryTask, (task, taskDetail) =>
                {
                    if (!taskDict.TryGetValue(task.FillInTaskId, out var taskEntry))
                    {
                        taskEntry = task;
                        taskEntry.Details = new List<FillInDetail>();
                        taskDict.Add(taskEntry.FillInTaskId, taskEntry);
                    }

                    taskEntry.Details.Add(taskDetail);

                    return taskEntry;
                }, splitOn: "FillInTaskId,FillInDetailId").Distinct().ToList();


                item = list.First();
            }

            return item;

        }

        public FillInTask GetItem(string aisTaskId)
        {
            FillInTask item;

            string sqlQueryTask =
                $@"SELECT * FROM FillInTask INNER JOIN FillInDetail ON FillInDetail.FillInTaskId = FillInTask.FillInTaskId WHERE FillInTask.AisTaskId ='{aisTaskId}'";

            using (var connection = new SqlConnection(ConString))
            {
                var taskDict = new Dictionary<long, FillInTask>();

                var list = connection.Query<FillInTask, FillInDetail, FillInTask>(sqlQueryTask, (task, taskDetail) =>
                {
                    if (!taskDict.TryGetValue(task.FillInTaskId, out var taskEntry))
                    {
                        taskEntry = task;
                        taskEntry.Details = new List<FillInDetail>();
                        taskDict.Add(taskEntry.FillInTaskId, taskEntry);
                    }

                    taskEntry.Details.Add(taskDetail);

                    return taskEntry;
                }, splitOn: "FillInTaskId,FillInDetailId").Distinct().ToList();


                item = list.FirstOrDefault();
            }

            return item;
        }

        public long Create(FillInTask item)
        {
            long id;
            item.Ts = DateTime.Now;

            using (var connection = new SqlConnection(ConString))
            {
                id = connection.Insert<FillInTask>(item);
                foreach (var d in item.Details)
                {
                    d.FillInTaskId = id;
                    d.Ts = DateTime.Now;
                    var dId = connection.Insert<FillInDetail>(d);
                }
            }
            
            return id;
        }

        public bool Update(FillInTask item)
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
                    t.FillInTaskId = task.FillInTaskId;
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
            
            // Извлекаем запись в таблице FillInTask
            var task = GetItem(id);
            if (task == null) return false;

            // Вначале удаляем записи в FillInDetail, затем можно удалить запись из FillInTask
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
