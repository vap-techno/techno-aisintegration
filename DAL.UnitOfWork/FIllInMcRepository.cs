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
    public class FIllInMcTaskRepository : IRepository<FillInMcTask>
    {

        public readonly string ConString; // Строка подключения к базе данных
        private readonly ILogger _l;

        public FIllInMcTaskRepository(string con, ILogger logger)
        {
            ConString = con;
            _l = logger;
        }

        public IList<FillInMcTask> GetAll()
        {
            List<FillInMcTask> taskList;

            string sqlQueryTask =
                @"SELECT * FROM FillInMcTask";

            try
            {
                using (var connection = new SqlConnection(ConString))
                {
                    var list = connection.Query<FillInMcTask>(sqlQueryTask).ToList();
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

        public FillInMcTask GetItem(long id)
        {
            FillInMcTask item;

            string sqlQueryTask =
                $@"SELECT * FROM FillInMcTask WHERE FillInMcTask.FillInMcTaskId = {id}";

            try
            {
                using (var connection = new SqlConnection(ConString))
                {
                    var list = connection.Query<FillInMcTask>(sqlQueryTask).ToList();
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

        public FillInMcTask GetItem(string aisTaskId)
        {
            FillInMcTask item;

            string sqlQueryTask =
                $@"SELECT * FROM FillInMcTask WHERE FillInMcTask.AisTaskId = '{aisTaskId}'";

            try
            {
                using (var connection = new SqlConnection(ConString))
                {
                    var list = connection.Query<FillInMcTask>(sqlQueryTask).ToList();
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

        public long Create(FillInMcTask item)
        {
            long id;
            item.Ts = DateTime.Now;

            try
            {
                using (var connection = new SqlConnection(ConString))
                {
                    id = connection.Insert<FillInMcTask>(item);
                }

                return id;
            }
            catch (Exception e)
            {

                _l.Error(e, "Ошибка соединения с базой данных");
                return 0;
            }
        }

        public bool Update(FillInMcTask item)
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
                        t.FillInMcTaskId = task.FillInMcTaskId;
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
            
            // Извлекаем запись в таблице FillInTask
            var task = GetItem(id);
            if (task == null) return false;

            try
            {
                using (var connection = new SqlConnection(ConString))
                {
                    res = connection.Delete(task);
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
