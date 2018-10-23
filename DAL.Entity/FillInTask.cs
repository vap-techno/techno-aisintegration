using System;
using System.Collections.Generic;
using Dapper.Contrib.Extensions;
using DAL.Entity.Abstract;


namespace DAL.Entity
{
    [Table("FillInTask")]
    public class FillInTask : IAisRequest
    {

        /// <summary>
        /// Идентификатор записи в таблице
        /// </summary>
        [Key]
        public long FillInTaskId { get; set; }

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
        /// [PN] Гос. номер АЦ
        /// </summary>
        public string Pn { get; set; }

        /// <summary>
        /// [DN] ФИО водителя
        /// </summary>
        public string Dn { get; set; }

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
        public List<FillInDetail> Details { get; set; }
    }
}