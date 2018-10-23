﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Dapper.Contrib.Extensions;
using DAL.Entity;
using Newtonsoft.Json;

namespace DAL.UnitOfWork
{
    public class StatusTaskRepository : IRepository<StatusTask>
    {

        public readonly string ConString; // Строка подключения к базе данных

        public StatusTaskRepository(string con)
        {
            ConString = con;
        }

        public StatusTask GetItem(long id)
        {
            StatusTask item;

            string sqlQueryTask =
                $@"SELECT StatusTaskId, Ts, Cid FROM StatusTask WHERE StatusTask.StatusTaskId = {id}";

            using (var connection = new SqlConnection(ConString))
            {
                var taskM = connection.Query<StatusTaskMediator>(sqlQueryTask).ToList().First();
                item = GetStatusTask(taskM);
            }

            return item;
        }

        public IList<StatusTask> GetAll()
        {
            List<StatusTask> items = new List<StatusTask>();

            string sqlQueryTask =
                $@"SELECT StatusTaskId, Ts, Cid, Ids FROM StatusTask";

            using (var connection = new SqlConnection(ConString))
            {
                var taskList = connection.Query<StatusTaskMediator>(sqlQueryTask).ToList();
                taskList.ForEach(t => items.Add(GetStatusTask(t)));
            }

            return items;
        }

        public long Create(StatusTask item)
        {
            long id;

            using (var connection = new SqlConnection(ConString))
            {
                item.Ts = DateTime.Now;
                var itemM = GetStatusTaskMediator(item);
                id = connection.Insert(itemM);
            }

            return id;

        }

        public bool Update(StatusTask item)
        {
            var itemM = GetStatusTaskMediator(item);

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
                    t.StatusTaskId = task.StatusTaskId;
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
        private StatusTask GetStatusTask(StatusTaskMediator task)
        {
            var item = new StatusTask
            {
                StatusTaskId = task.StatusTaskId,
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
        private StatusTaskMediator GetStatusTaskMediator(StatusTask task)
        {
            var item = new StatusTaskMediator()
            {
                StatusTaskId = task.StatusTaskId,
                Cid = task.Cid,
                Ts = task.Ts,
                Ids = JsonConvert.SerializeObject(task.Ids)
            };

            return item;
        }

    }
}