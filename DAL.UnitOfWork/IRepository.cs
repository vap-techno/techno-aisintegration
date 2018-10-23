using System.Collections.Generic;

namespace DAL.UnitOfWork
{
    public interface IRepository<T> where T : class
    {
        
        /// <summary>
        /// Получить объект
        /// </summary>
        /// <param name="id">ИД</param>
        /// <returns>Объект</returns>
        T GetItem(long id);

        /// <summary>
        /// Получить все объекты
        /// </summary>
        /// <returns>Список объектов</returns>
        IList<T> GetAll();

        /// <summary>
        /// Создать запись в БД
        /// </summary>
        /// <param name="item">Объект</param>
        /// <returns>ИД новой записи</returns>
        long Create(T item);

        /// <summary>
        /// Изменить запись в БД
        /// </summary>
        /// <param name="item">Объект (ищет по Cid)</param>
        /// <returns></returns>
        bool Update(T item);

        /// <summary>
        /// Удалить запись из БД
        /// </summary>
        /// <param name="id">ИД записи в БД</param>
        /// <returns></returns>
        bool Delete(long id);

    }
}