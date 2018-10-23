using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Dapper.Contrib.Extensions;
using DAL.Entity;
using Serilog;

namespace DAL.UnitOfWork
{
    public class FIllOutTaskRepository : IRepository<FillOutTask>
    {

        public readonly string ConString; // Строка подключения к базе данных
        private readonly ILogger _l;

        public FIllOutTaskRepository(string con, ILogger logger)
        {
            ConString = con;
            _l = logger;
        }

        public IList<FillOutTask> GetAll()
        {
            List<FillOutTask> taskList;

            string sqlQueryTask =
                @"SELECT * FROM FillOutTask INNER JOIN FillOutDetail ON FillOutDetail.FillOutTaskId = FillOutTask.FillOutTaskId";

            try
            {
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
            catch (Exception e)
            {

                _l.Error(e, "Ошибка соединения с базой данных");
                return null;
            }
        }

        public FillOutTask GetItem(long id)
        {
            FillOutTask item;

            string sqlQueryTask =
                $@"SELECT * FROM FillOutTask INNER JOIN FillOutDetail ON FillOutDetail.FillOutTaskId = FillOutTask.FillOutTaskId WHERE FillOutTask.FillOutTaskId = {id}";

            try
            {
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
            catch (Exception e)
            {

                _l.Error(e, "Ошибка соединения с базой данных");
                return null;
            }

        }

        public FillOutTask GetItem(string aisTaskId)
        {
            FillOutTask item;

            string sqlQueryTask =
                $@"SELECT * FROM FillOutTask INNER JOIN FillOutDetail ON FillOutDetail.FillOutTaskId = FillOutTask.FillOutTaskId WHERE FillOutTask.AisTaskId = '{aisTaskId}'";

            try
            {
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
            catch (Exception e)
            {
                _l.Error(e, "Ошибка соединения с базой данных");
                return null;
            }
        }

        public long Create(FillOutTask item)
        {
            long id;
            item.Ts = DateTime.Now;

            try
            {
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
            catch (Exception e)
            {
                _l.Error(e, "Ошибка соединения с базой данных");
                return 0;
            }
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

            try
            {
                // Обновляем запись, берем Id из найденной записи в БД,
                // присваиваем ее новые данные, т.к. АИС не знает о ID внутренней базы
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
            catch (Exception e)
            {

                _l.Error(e, "Ошибка соединения с базой данных");
                return false;
            }
        }

        public bool Delete(long id)
        {
            bool res = false;
            
            // Извлекаем запись в таблице FillOutTask
            var task = GetItem(id);
            if (task == null) return false;

            try
            {
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
            catch (Exception e)
            {

                _l.Error(e, "Ошибка соединения с базой данных");
                return false;
            }
        }
    }
}
