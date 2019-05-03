using System;
using Dapper.Contrib.Extensions;

namespace DAL.Entity
{
    [Table("FillInDetail")]
    public class FillInDetail
    {

        /// <summary>
        /// Идентификатор записи в таблице
        /// </summary>
        [Key]
        public long FillInDetailId { get; set; }

        /// <summary>
        /// Дата добавления записи в таблицу
        /// </summary>
        public DateTime Ts { get; set; }

        /// <summary>
        /// Идентификатор записи в таблице заявок
        /// </summary>
        public long FillInTaskId { get; set; }

        #region Task

        /// <summary>
        /// [SID] Идентификатор секции в контексте заданного задания
        /// </summary>
        public string Sid { get; set; }

        /// <summary>
        /// [LNP] Номер поста (план)
        /// </summary>
        public string Lnp { get; set; }

        /// <summary>
        /// [SN] Номер секции АЦ
        /// </summary>
        public int Sn { get; set; }

        /// <summary>
        /// [FM] Способ налива
        /// </summary>
        public int Fm { get; set; }

        /// <summary>
        /// [PPF] Признак производства продукта
        /// </summary>
        public int Ppf { get; set; }

        /// <summary>
        /// [PN] Наименование готового/производимого продукта
        /// </summary>
        public string Pn { get; set; }

        /// <summary>
        /// [BFN] Наименование базового топлива
        /// </summary>
        public string Bfn { get; set; }

        /// <summary>
        /// [AN] Наименование присадки
        /// </summary>
        public string An { get; set; }

        /// <summary>
        /// [TN] Номер резервуара с готовым продуктом или базовым топливом
        /// </summary>
        public string Tn { get; set; }

        /// <summary>
        /// [ATN] Номер резервуара с присадкой
        /// </summary>
        public string Atn { get; set; }

        /// <summary>
        /// [PVP] Объем готового/производимого продукта (план)
        /// </summary>
        public double Pvp { get; set; }

        /// <summary>
        /// [BFVP] Объем базового топлива (план)
        /// </summary>
        public double Bfvp { get; set; }

        /// <summary>
        /// [AVP] Объем присадки (план)
        /// </summary>
        public double Avp { get; set; }

        /// <summary>
        /// [PMP] Масса готового/производимогого продукта (план)
        /// </summary>
        public double Pmp { get; set; }

        /// <summary>
        /// [BFMP] Масса базового топлива (план)
        /// </summary>
        public double Bfmp { get; set; }

        /// <summary>
        /// [AMP] Масса присадки (план)
        /// </summary>
        public double Amp { get; set; }

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
        /// [BFV1] Суммарное показание расходомера до налива базового топлива
        /// </summary>
        public double? Bfv1 { get; set; }

        /// <summary>
        /// [BFV2] Суммарное показание расходомера после налива базового топлива
        /// </summary>
        public double? Bfv2 { get; set; }

        /// <summary>
        /// [AV1] Суммарное показание расходомера до налива присадки 
        /// </summary>
        public double? Av1 { get; set; }

        /// <summary>
        /// [AV2] Суммарное показание расходомера до налива присадки 
        /// </summary>
        public double? Av2 { get; set; }

        /// <summary>
        /// [PVF] Объем готового/производимого продукта (факт)
        /// </summary>
        public double? Pvf { get; set; }

        /// <summary>
        /// [BFVF] Объем базового топлива (факт)
        /// </summary>
        public double? Bfvf { get; set; }

        /// <summary>
        /// Объем присадки (факт)
        /// </summary>
        public double? Afv { get; set; }

        /// <summary>
        /// [P] Избыточное давление по наливной линии с готовым/производимым
        /// </summary>
        public double? P { get; set; }

        /// <summary>
        /// [PMF] Масс готового/производимого продукта(факт)
        /// </summary>
        public double Pmf { get; set; }

        /// <summary>
        /// [BFMF] Масса базового топлива (факт)
        /// </summary>
        public double? Bfmf { get; set; }

        /// <summary>
        /// [AMF] Масса присадки (факт)
        /// </summary>
        public double? Amf { get; set; }

        /// <summary>
        /// [PTF] Температура готового/производимого продукта
        /// </summary>
        public double Ptf { get; set; }

        /// <summary>
        /// [BFTF] Температура базового топлива (факт)
        /// </summary>
        public double? Bftf { get; set; }

        /// <summary>
        /// [ATF] Температура присадки (факт)
        /// </summary>
        public double? Atf { get; set; }

        /// <summary>
        /// [PRF] Плотность готового/производимого продукта (факт)
        /// </summary>
        public double Prf { get; set; }

        /// <summary>
        /// [BFRF] Плотность базового топлива (факт)
        /// </summary>
        public double? Bfrf { get; set; }

        /// <summary>
        /// [ARF] Плотность присадки (факт)
        /// </summary>
        public double? Arf { get; set; }

        /// <summary>
        /// [TADJ] Температура приведения плотности (факт 15 или 20 С)
        /// </summary>
        public double? Tadj { get; set; }

        /// <summary>
        /// [PRAdjF] Приведенная плотность готового/производимого продукта (факт)
        /// </summary>
        public double? PrAdjF { get; set; }

        /// <summary>
        /// [BFAdjF] Приведенная плотность базового топлива (факт)
        /// </summary>
        public double? BfAdjf { get; set; }

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
        public double? FSpd { get; set; }

        /// <summary>
        /// [tRest] Остаток времени налива
        /// </summary>
        public int? TimeRest { get; set; }
        #endregion
    }
}