using System;
using System.Collections.Generic;
using Dapper.Contrib.Extensions;

namespace DAL.Entity
{
    [Table("CancelTask")]
    public class CancelTaskMediator
    {
        
        /// <summary>
        /// Идентификатор записи в таблице
        /// </summary>
        [Key]
        public long CancelTaskId { get; set; }

        /// <summary>
        /// Дата добавления записи в таблицу
        /// </summary>
        public DateTime Ts { get; set; }

        /// <summary>
        /// [CID] Идентификатор команды
        /// </summary>
        public string Cid { get; set; }

        /// <summary>
        /// [IDs] Идентификаторы заданий АИС ТПС
        /// </summary>
        public string Ids { get; set; }
    }
}