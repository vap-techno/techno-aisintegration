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
    public class FillOutDetailRepository : IRepository<FillOutDetail>
    {

        public readonly string ConString; // Строка подключения к базе данных
        private readonly ILogger _l;

        public FillOutDetailRepository(string con, ILogger logger)
        {
            ConString = con;
            _l = logger;
        }

        public FillOutDetail GetItem(long id)
        {
            FillOutDetail item;

            string query =
                $@"SELECT * FROM FillOutDetail WHERE FillOutDetailId = {id}";

            try
            {
                using (var connection = new SqlConnection(ConString))
                {
                    item = connection.Query<FillOutDetail>(query).First();
                }

                return item;
            }
            catch (Exception e)
            {
                _l.Error(e, "Ошибка соединения с базой данных");
                return null;
            }
        }

        public IList<FillOutDetail> GetItems(long taskId)
        {
            string query = $@"SELECT * FROM FillOutDetail WHERE FillOutTaskId = {taskId}";

            IList<FillOutDetail> items;

            try
            {
                using (var connection = new SqlConnection(ConString))
                {
                    var list = connection.Query<FillOutDetail>(query).ToList();
                    items = list;
                }

                return items;
            }
            catch (Exception e)
            {
                _l.Error(e, "Ошибка соединения с базой данных");
                return null;
            }

        }

        public IList<FillOutDetail> GetAll()
        {
            string query =
                $@"SELECT * FROM FillOutDetail";

            IList<FillOutDetail> itemList;

            try
            {
                using (var connection = new SqlConnection(ConString))
                {
                    itemList = connection.Query<FillOutDetail>(query).ToList();
                }

                return itemList;
            }
            catch (Exception e)
            {
                _l.Error(e, "Ошибка соединения с базой данных");
                return null;
            }
        }

        public long Create(FillOutDetail item)
        {
            long id;
            item.Ts = DateTime.Now;

            try
            {
                using (var connection = new SqlConnection(ConString))
                {
                    id = connection.Insert<FillOutDetail>(item);
                }

                return id;
            }
            catch (Exception e)
            {
                _l.Error(e, "Ошибка соединения с базой данных");
                return 0;
            }
        }

        public bool Update(FillOutDetail item)
        {
            // Ищем записи в таблицах, предполагая что Sid и TaskId
            // - составной уникальный идентификатор
            // Сравнение по ключу Id не используем, т.к. заявка может придти
            // из АИС, а в ней идентификатор не задается
            var details = GetAll();
            var detail = (from d in details
                where (item.FillOutDetailId == d.FillOutDetailId && item.Sid == d.Sid)
                select d).FirstOrDefault();

            bool result;

            try
            {
                using (var connection = new SqlConnection(ConString))
                {

                    var d = item;
                    if (detail != null) d.FillOutDetailId = detail.FillOutDetailId;
                    result = connection.Update(d);
                }

                return result;
            }
            catch (Exception e)
            {
                _l.Error(e, "Ошибка соединения с базой данных");

                return false;
            }
        }

        public bool Update(List<FillOutDetail> items)
        {
            if (items == null)
            {
                _l.Debug("Попытка обновления нулевого набора записей в FillInDetail");
                return false;
            }

            bool result = false;

            try
            {
                // Обновляем как есть, предполагая, что ключ Id уже задан
                using (var connection = new SqlConnection(ConString))
                {
                    result = connection.Update(items);
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

            // Извлекаем запись в таблице
            var item = GetItem(id);
            if (item == null) return false;

            // Удаляем
            using (var connection = new SqlConnection(ConString))
            {
                res = connection.Delete(item);
            }

            return res;
        }
    }
}