using System;
using DAL.Entity.Abstract;

namespace DAL.Entity.Status
{
    public class FillInMcStatusDetail : IStatusDetail
    {
        /// <summary>
        /// [LNF] Номер поста (факт)
        /// </summary>
        public string Lnf { get; set; }

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
        public string Rm { get; set; }

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
        public DateTime Dt1 { get; set; }

        /// <summary>
        /// [DT2] Дата и время окончания налива
        /// </summary>
        public DateTime Dt2 { get; set; }

        /// <summary>
        /// [FSpd] Скорость налива
        /// </summary>
        public double FSpd { get; set; }

        /// <summary>
        /// [tRest] Остаток времени налива
        /// </summary>
        public int TimeRest { get; set; }
    }
}