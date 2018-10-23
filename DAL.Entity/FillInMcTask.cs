using System;
using Dapper.Contrib.Extensions;
using DAL.Entity.Abstract;

namespace DAL.Entity
{
    [Table("FillInMcTask")]
    public class FillInMcTask : IAisRequest
    {
        /// <summary>
        /// Идентификатор записи в таблице
        /// </summary>
        [Key]
        public long FillInMcTaskId { get; set; }

        /// <summary>
        /// Дата добавления записи в таблицу
        /// </summary>
        public DateTime Ts { get; set; }

        #region Task
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
        /// [LNP] Номер поста (план)
        /// </summary>
        public string Lnp { get; set; }

        /// <summary>
        /// [PN] Наименование продукта
        /// </summary>
        public string Pn { get; set; }

        /// <summary>
        /// [TN] Номер резервуара
        /// </summary>
        public string Tn { get; set; }

        /// <summary>
        /// [PVP] Объем продукта (план)
        /// </summary>
        public double Pvp { get; set; }

        /// <summary>
        /// [PMP] Масса  продукта (план)
        /// </summary>
        public double Pmp { get; set; }

        #endregion

        #region Status

        /// <summary>
        /// [LNF] Номер поста (факт)
        /// </summary>
        public string Lnf { get; set; } = "";

        /// <summary>
        /// [LS] Код состояния поста налива в АСН
        /// </summary>
        public int Ls { get; set; }

        /// <summary>
        /// [FS] Код статуса налива в секцию в АСН
        /// </summary>
        public int Fs { get; set; }

        /// <summary>
        /// [RM] Сообщение о результате работы с данной секцией
        /// </summary>
        public string Rm { get; set; } = "";

        /// <summary>
        /// [PV1] Суммарное показание расходмера до налива готового проудкта
        /// </summary>
        public double Pv1 { get; set; }

        /// <summary>
        /// [PV2] Суммарное показание расходомера после налива готового продукта
        /// </summary>
        public double Pv2 { get; set; }

        /// <summary>
        /// [PVF] Объем готового/производимого продукта (факт)
        /// </summary>
        public double Pvf { get; set; }

        /// <summary>
        /// [PMF] Масс готового/производимого продукта(факт)
        /// </summary>
        public double Pmf { get; set; }

        /// <summary>
        /// [PTF] Температура готового/производимого продукта
        /// </summary>
        public double Ptf { get; set; }

        /// <summary>
        /// [PRF] Плотность готового/производимого продукта (факт)
        /// </summary>
        public double Prf { get; set; }

        /// <summary>
        /// [TADJ] Температура приведения плотности (факт 15 или 20 С)
        /// </summary>
        public double Tadj { get; set; }

        /// <summary>
        /// [PRAdjF] Приведенная плотность готового/производимого продукта (факт)
        /// </summary>
        public double PrAdjF { get; set; }

        /// <summary>
        /// [DT1] Дата и время начала налива
        /// </summary>
        public DateTime Dt1 { get; set; } = new DateTime(1970,1,1);

        /// <summary>
        /// [DT2] Дата и время окончания налива
        /// </summary>
        public DateTime Dt2 { get; set; } = new DateTime(1970, 1, 1);

        /// <summary>
        /// [FSpd] Скорость налива
        /// </summary>
        public double FSpd { get; set; }

        /// <summary>
        /// [tRest] Остаток времени налива
        /// </summary>
        public TimeSpan TimeRest { get; set; }
        #endregion
    }
}