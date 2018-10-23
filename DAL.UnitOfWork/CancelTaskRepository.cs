using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using Dapper;
using Dapper.Contrib.Extensions;
using DAL.Entity;
using Newtonsoft.Json;


namespace DAL.UnitOfWork
{
    public class CancelTaskRepository : IRepository<CancelTask>
    {

        public readonly string ConString; // Строка подключения к базе данных

        public CancelTaskRepository(string con)
        {
            ConString = con;
        }

        public CancelTask GetItem(long id)
        {
            CancelTask item;

            string sqlQueryTask =
                $@"SELECT CancelTaskId, Ts, Cid, Ids FROM CancelTask WHERE CancelTask.CancelTaskId = {id}";

            using (var connection = new SqlConnection(ConString))
            {
                var taskM = connection.Query<CancelTaskMediator>(sqlQueryTask).ToList().FirstOrDefault();
                item = taskM == null ? null : GetCancelTask(taskM);
            }

            return item;
        }

        public IList<CancelTask> GetAll()
        {
            List<CancelTask> items = new List<CancelTask>();

            string sqlQueryTask =
                $@"SELECT CancelTaskId, Ts, Cid, Ids FROM CancelTask";

            using (var connection = new SqlConnection(ConString))
            {
                var taskList = connection.Query<CancelTaskMediator>(sqlQueryTask).ToList();
                taskList.ForEach( t => items.Add(GetCancelTask(t)));
            }

            return items;
        }

        public long Create(CancelTask item)
        {
            long id;
            item.Ts = DateTime.Now;

            using (var connection = new SqlConnection(ConString))
            {
                var itemM = GetCancelTaskMediator(item);
                id = connection.Insert(itemM);
            }

            return id;
        }

        public bool Update(CancelTask item)
        {
            var itemM = GetCancelTaskMediator(item);

            // Ищем заданную заявку, предполагая что Cid - уникальный идентификатор
            // Сравнение по ключу Id не используем, т.к. заявка может придти
            // из АИС, а в ней идентификатор не задается
            var tasks = GetAll();
            var task = (from t in tasks
                where (itemM.Cid == t.Cid)
                select t).FirstOrDefault();

            bool result;

            using (var connection = new SqlConnection(ConString))
            {
                var t = itemM;
                if (task != null)
                {
                    t.CancelTaskId = task.CancelTaskId;
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

            // Извлекаем запись в таблице
            var item = GetItem(id);
            if (item == null) return false;

            // Производим удаление
            using (var connection = new SqlConnection(ConString))
            {
                res = connection.Delete(item);
            }

            return res;
        }

        /// <summary>
        /// Преобразует промежуточный класс, вычитываемый из БД в класс заявки на отмену
        /// </summary>
        /// <param name="task">Заявка</param>
        /// <returns></returns>
        private CancelTask GetCancelTask(CancelTaskMediator task)
        {
            var item = new CancelTask
            {
                CancelTaskId = task.CancelTaskId,
                Cid = task.Cid,
                Ts = task.Ts,
                Ids = JsonConvert.DeserializeObject<List<string>>(task.Ids)
            };

            return item;
        }

        /// <summary>
        /// Преобразеут класс заявки на отмену в промежуточный для отправки в БД
        /// </summary>
        /// <param name="task">Заявка</param>
        /// <returns></returns>
        private CancelTaskMediator GetCancelTaskMediator(CancelTask task)
        {
            var item = new CancelTaskMediator()
            {
                CancelTaskId = 0,
                Cid = task.Cid,
                Ts = DateTime.Now,
                Ids = JsonConvert.SerializeObject(task.Ids)
            };

            return item;
        }

    }
}