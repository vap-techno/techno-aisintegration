using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Dapper.Contrib.Extensions;
using DAL.Entity;

namespace DAL.UnitOfWork
{
    public class FillInDetailRepository : IRepository<FillInDetail>
    {

        public readonly string ConString; // Строка подключения к базе данных

        public FillInDetailRepository(string con)
        {
            ConString = con;
        }

        public FillInDetail GetItem(long id)
        {
            FillInDetail item;

            string query =
                $@"SELECT * FROM FillInDetail WHERE FillInDetailId = {id}";

            using (var connection = new SqlConnection(ConString))
            {
                item = connection.Query<FillInDetail>(query).First();
            }

            return item;
        }

        public IList<FillInDetail> GetItems(long taskId)
        {
            string query = $@"SELECT * FROM FillInDetail WHERE FillInTaskId = {taskId}";

            IList<FillInDetail> items;

            using (var connection = new SqlConnection(ConString))
            {
                var list = connection.Query<FillInDetail>(query).ToList();
                items = list;
            }

            return items;

        }

        public IList<FillInDetail> GetAll()
        {
            string query =
                $@"SELECT * FROM FillInDetail";

            IList<FillInDetail> itemList;

            using (var connection = new SqlConnection(ConString))
            {
                itemList = connection.Query<FillInDetail>(query).ToList();
            }

            return itemList;
        }

        public long Create(FillInDetail item)
        {
            long id;

            using (var connection = new SqlConnection(ConString))
            {
                id = connection.Insert<FillInDetail>(item);
            }

            return id;
        }

        public bool Update(FillInDetail item)
        {

            // Ищем заданную заявку, предполагая что Cid - уникальный идентификатор
            // Сравнение по ключу Id не используем, т.к. заявка может придти
            // из АИС, а в ней идентификатор не задается
            var details = GetAll();
            var detail = (from d in details
                where (item.Sid == d.Sid)
                select d).FirstOrDefault();

            bool result;

            using (var connection = new SqlConnection(ConString))
            {
                var d = item;
                if (detail != null) d.FillInDetailId = detail.FillInDetailId;
                result = connection.Update(d);
            }

            return result;
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