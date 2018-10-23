using System;
using System.Collections.Generic;
using Dapper.Contrib.Extensions;
using DAL.Entity.Abstract;

namespace DAL.Entity
{
    [Table("FillOutTask")]
    public class FillOutTask : IAisRequest
    {
        /// <summary>
        /// Идентификатор записи в таблице
        /// </summary>
        [Key]
        public long FillOutTaskId { get; set; }

        /// <summary>
        /// Дата добавления записи в таблицу
        /// </summary>
        public DateTime Ts { get; set; }

        /// <summary>
        /// [CID] Идентификатор команды
        /// </summary>
        public string Cid { get; set; }

        /// <summary>
        /// [ID] Идентификатор задания АИС ТПС
        /// </summary>
        public string AisTaskId { get; set; }

        /// <summary>
        /// [TDT] Дата и время задания АИС ТПС
        /// </summary>
        public DateTime Tdt { get; set; }

        /// <summary>
        /// [TNO] Номер задания АИС ТПС
        /// </summary>
        public string Tno { get; set; }

        /// <summary>
        /// [ON] ФИО товарного оператора
        /// </summary>
        public string On { get; set; }

        /// <summary>
        /// [MM] Способ измерения в АИС ТПС
        /// </summary>
        public int Mm { get; set; }

        /// <summary>
        /// [Details] Детализирующие данные
        /// </summary>
        [Write(false)]
        public List<FillOutDetail> Details { get; set; }
    }
}