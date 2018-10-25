using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Dapper;
using Dapper.Contrib.Extensions;
using DAL.Entity;
using Serilog;

namespace DAL.UnitOfWork
{
    public class FIllInTaskRepository : IRepository<FillInTask>
    {

        public readonly string ConString; // Строка подключения к базе данных
        private readonly ILogger _l;

        public FIllInTaskRepository(string con, ILogger logger)
        {
            ConString = con;
            _l = logger;
        }

        public IList<FillInTask> GetAll()
        {
            List<FillInTask> taskList;

            string sqlQueryTask =
                @"SELECT * FROM FillInTask INNER JOIN FillInDetail ON FillInDetail.FillInTaskId = FillInTask.FillInTaskId";

            try
            {
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
            catch (Exception e)
            {

                _l.Error(e, "Ошибка соединения с базой данных");
                return null;
            }
        }

        public FillInTask GetItem(long id)
        {
            FillInTask item;

            string sqlQueryTask =
                $@"SELECT * FROM FillInTask INNER JOIN FillInDetail ON FillInDetail.FillInTaskId = FillInTask.FillInTaskId WHERE FillInTask.FillInTaskId = {id}";

            try
            {
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
            catch (Exception e)
            {

                _l.Error(e, "Ошибка соединения с базой данных");
                return null;
            }

        }

        public FillInTask GetItem(string aisTaskId)
        {
            FillInTask item;

            string sqlQueryTask =
                $@"SELECT * FROM FillInTask INNER JOIN FillInDetail ON FillInDetail.FillInTaskId = FillInTask.FillInTaskId WHERE FillInTask.AisTaskId ='{aisTaskId}'";

            try
            {
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
            catch (Exception e)
            {

                _l.Error(e, "Ошибка соединения с базой данных");
                return null;
            }
        }

        public long Create(FillInTask item)
        {
            long id;
            item.Ts = DateTime.Now;

            try
            {
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
            catch (Exception e)
            {

                _l.Error(e, "Ошибка соединения с базой данных");
                return 0;
            }
        }

        public bool Update(FillInTask item)
        {
            // Ищем ID в таблице, которая содержит заданный CID
            // TODO: Пойдет только с маленькой базой, либо выводить в асинхрон

            var tasks = GetAll();
            var task = (from t in tasks
                where t.Cid == item.Cid
                select t).FirstOrDefault();


            bool result = false;

            try
            {
                // Обновляем запись, берем Id из найденной записи в БД,
                // присваиваем ее новые данные, т.к. АИС не знает о ID внутренней базы
                // Берем Id для Detail также из найденных записей в БД
                using (var connection = new SqlConnection(ConString))
                {
                    var t = item;
                    if (task != null)
                    {
                        t.FillInTaskId = task.FillInTaskId;

                        // Подменяем в принятых данных FillInTaskId и FillInDetailId
                        // для таблицы FillInDetail, т.к. данные могли придти с АИС, а она
                        // про эти поля не знает
                        // TODO: Переделать передачу репозитория через DI
                        var repD = new FillInDetailRepository(ConString, _l);
                        var details = repD.GetItems(task.FillInTaskId);

                        t.Details.ForEach(d =>
                        {
                            var detail = (from entity in details
                                where entity.Sid == d.Sid
                                select entity).FirstOrDefault();

                            d.FillInTaskId = task.FillInTaskId;
                            if (detail != null) d.FillInDetailId = detail.FillInDetailId;
                        });

                        result = connection.Update(t) && repD.Update(t.Details);
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
                return result;
            }
        }

        public bool Delete(long id)
        {
            bool res = false;

            // Извлекаем запись в таблице FillInTask
            var task = GetItem(id);
            if (task == null) return false;

            try
            {
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
            catch (Exception e)
            {

                _l.Error(e, "Ошибка соединения с базой данных");
                return false;
            }
        }
    }
}
