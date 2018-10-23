using System;
using System.Collections.Generic;
using Dapper.Contrib.Extensions;
using DAL.Entity.Abstract;

namespace DAL.Entity
{
    [Table("StatusTask")]
    public class StatusTask : IAisRequest
    {
        /// <summary>
        /// Идентификатор записи в таблице
        /// </summary>
        [Key]
        public long StatusTaskId { get; set; }

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
        [Write(false)]
        public List<string> Ids { get; set; }
    }
}