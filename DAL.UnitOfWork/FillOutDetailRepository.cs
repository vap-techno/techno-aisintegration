using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Dapper.Contrib.Extensions;
using DAL.Entity;

namespace DAL.UnitOfWork
{
    public class FillOutDetailRepository : IRepository<FillOutDetail>
    {

        public readonly string ConString; // Строка подключения к базе данных

        public FillOutDetailRepository(string con)
        {
            ConString = con;
        }

        public FillOutDetail GetItem(long id)
        {
            FillOutDetail item;

            string query =
                $@"SELECT * FROM FillOutDetail WHERE FillOutDetailId = {id}";

            using (var connection = new SqlConnection(ConString))
            {
                item = connection.Query<FillOutDetail>(query).First();
            }

            return item;
        }

        public IList<FillOutDetail> GetItems(long taskId)
        {
            string query = $@"SELECT * FROM FillOutDetail WHERE FillOutTaskId = {taskId}";

            IList<FillOutDetail> items;

            using (var connection = new SqlConnection(ConString))
            {
                var list = connection.Query<FillOutDetail>(query).ToList();
                items = list;
            }

            return items;

        }

        public IList<FillOutDetail> GetAll()
        {
            string query =
                $@"SELECT * FROM FillOutDetail";

            IList<FillOutDetail> itemList;

            using (var connection = new SqlConnection(ConString))
            {
                itemList = connection.Query<FillOutDetail>(query).ToList();
            }

            return itemList;
        }

        public long Create(FillOutDetail item)
        {
            long id;

            using (var connection = new SqlConnection(ConString))
            {
                id = connection.Insert<FillOutDetail>(item);
            }

            return id;
        }

        public bool Update(FillOutDetail item)
        {
            var details = GetAll();
            var detail = (from d in details
                where (item.FillOutDetailId == d.FillOutDetailId && item.Sid == d.Sid)
                select d).FirstOrDefault();

            bool result;

            using (var connection = new SqlConnection(ConString))
            {

                var d = item;
                if (detail != null) d.FillOutDetailId = detail.FillOutDetailId;
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