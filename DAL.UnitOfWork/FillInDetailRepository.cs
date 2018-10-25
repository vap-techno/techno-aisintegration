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
    public class FillInDetailRepository : IRepository<FillInDetail>
    {

        public readonly string ConString; // Строка подключения к базе данных
        private readonly ILogger _l;

        public FillInDetailRepository(string con, ILogger logger)
        {
            ConString = con;
            _l = logger;
        }

        public FillInDetail GetItem(long id)
        {
            FillInDetail item;

            string query =
                $@"SELECT * FROM FillInDetail WHERE FillInDetailId = {id}";

            try
            {
                using (var connection = new SqlConnection(ConString))
                {
                    item = connection.Query<FillInDetail>(query).First();
                }

                return item;
            }
            catch (Exception e)
            {
                _l.Error(e, "Ошибка соединения с базой данных");
                return null;
            }
        }

        public IList<FillInDetail> GetItems(long taskId)
        {
            string query = $@"SELECT * FROM FillInDetail WHERE FillInTaskId = {taskId}";

            IList<FillInDetail> items;

            try
            {
                using (var connection = new SqlConnection(ConString))
                {
                    var list = connection.Query<FillInDetail>(query).ToList();
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

        public IList<FillInDetail> GetAll()
        {
            string query =
                $@"SELECT * FROM FillInDetail";

            IList<FillInDetail> itemList;

            try
            {
                using (var connection = new SqlConnection(ConString))
                {
                    itemList = connection.Query<FillInDetail>(query).ToList();
                }

                return itemList;
            }
            catch (Exception e)
            {
                _l.Error(e, "Ошибка соединения с базой данных");
                return null;
            }
        }

        public long Create(FillInDetail item)
        {
            long id;
            item.Ts = DateTime.Now;

            try
            {
                using (var connection = new SqlConnection(ConString))
                {
                    id = connection.Insert<FillInDetail>(item);
                }

                return id;
            }
            catch (Exception e)
            {
                _l.Error(e, "Ошибка соединения с базой данных");
                return 0;
            }
        }

        public bool Update(FillInDetail item)
        {

            // Ищем записи в таблицах, предполагая что Sid и TaskId
            // - составной уникальный идентификатор
            // Сравнение по ключу Id не используем, т.к. заявка может придти
            // из АИС, а в ней идентификатор не задается
            var details = GetAll();
            var detail = (from d in details
                where (item.Sid == d.Sid) && (item.FillInTaskId == d.FillInTaskId)
                select d).FirstOrDefault();

            bool result = false;

            try
            {
                using (var connection = new SqlConnection(ConString))
                {
                    var d = item;
                    if (detail != null)
                    { 
                        d.FillInDetailId = detail.FillInDetailId;
                        result = connection.Update(d);
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

        public bool Update(List<FillInDetail> items)
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

            try
            {
                // Удаляем
                using (var connection = new SqlConnection(ConString))
                {
                    res = connection.Delete(item);
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