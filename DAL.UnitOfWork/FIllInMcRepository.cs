using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Dapper.Contrib.Extensions;
using DAL.Entity;

namespace DAL.UnitOfWork
{
    public class FIllInMcTaskRepository : IRepository<FillInMcTask>
    {

        public readonly string ConString; // Строка подключения к базе данных

        public FIllInMcTaskRepository(string con)
        {
            ConString = con;
        }

        public IList<FillInMcTask> GetAll()
        {
            List<FillInMcTask> taskList;

            string sqlQueryTask =
                @"SELECT * FROM FillInMcTask";

            using (var connection = new SqlConnection(ConString))
            {
                var list = connection.Query<FillInMcTask>(sqlQueryTask).ToList();
                taskList = list;
            }

            return taskList;
        }

        public FillInMcTask GetItem(long id)
        {
            FillInMcTask item;

            string sqlQueryTask =
                $@"SELECT * FROM FillInMcTask WHERE FillInMcTask.FillInMcTaskId = {id}";

            using (var connection = new SqlConnection(ConString))
            {
                var list = connection.Query<FillInMcTask>(sqlQueryTask).ToList();
                item = list.First();
            }

            return item;

        }

        public FillInMcTask GetItem(string aisTaskId)
        {
            FillInMcTask item;

            string sqlQueryTask =
                $@"SELECT * FROM FillInMcTask WHERE FillInMcTask.AisTaskId = '{aisTaskId}'";

            using (var connection = new SqlConnection(ConString))
            {
                var list = connection.Query<FillInMcTask>(sqlQueryTask).ToList();
                item = list.FirstOrDefault();
            }

            return item;
        }

        public long Create(FillInMcTask item)
        {
            long id;
            item.Ts = DateTime.Now;

            using (var connection = new SqlConnection(ConString))
            {
                id = connection.Insert<FillInMcTask>(item);
            }
            
            return id;
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

        public bool Delete(long id)
        {
            bool res = false;
            
            // Извлекаем запись в таблице FillInTask
            var task = GetItem(id);
            if (task == null) return false;

            using (var connection = new SqlConnection(ConString))
            {
                res = connection.Delete(task);
            }

            return res;
        }
    }
}
