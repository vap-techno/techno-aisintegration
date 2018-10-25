﻿using System;
using Newtonsoft.Json;

namespace AisJson.Lib.DTO.Status
{
    public class FillOutStatusDetailDetailDto
    {
        /// <summary>
        /// [SID] Идентификатор секции в контексте данного задания
        /// </summary>
        [JsonProperty(PropertyName = "SID", Required = Required.Always)]
        public string Sid { get; set; }

        /// <summary>
        /// [SN] Номер секции АЦ
        /// </summary>
        [JsonProperty(PropertyName = "SN", Required = Required.Always)]
        public int Sn { get; set; }

        /// <summary>
        /// [LNF] Номер поста (факт)
        /// </summary>
        [JsonProperty(PropertyName = "LNF", Required = Required.Always)]
        public string Lnf { get; set; }

        /// <summary>
        /// [LS] Код состояния поста слива в АСН
        /// </summary>
        [JsonProperty(PropertyName = "LS", Required = Required.Always)]
        public int Ls { get; set; }

        /// <summary>
        /// [FS] Код статуса слива из секции в АСН
        /// </summary>
        [JsonProperty(PropertyName = "FS", Required = Required.Always)]
        public int Fs { get; set; }

        /// <summary>
        /// [RM] Сообщение о результате работы с данной секцией
        /// </summary>
        [JsonProperty(PropertyName = "RM", Required = Required.Default)]
        public string Rm { get; set; }

        /// <summary>
        /// [PV1] Суммарное показание расходмера до налива готового проудкта
        /// </summary>
        [JsonProperty(PropertyName = "PV1", Required = Required.Default)]
        public double Pv1 { get; set; }

        /// <summary>
        /// [PV2] Суммарное показание расходомера после налива готового продукта
        /// </summary>
        [JsonProperty(PropertyName = "PV2", Required = Required.Default)]
        public double Pv2 { get; set; }

        /// <summary>
        /// [PVF] Объем готового/производимого продукта (факт)
        /// </summary>
        [JsonProperty(PropertyName = "PVF", Required = Required.Default)]
        public double Pvf { get; set; }

        /// <summary>
        /// [PMF] Масса готового/производимого продукта(факт)
        /// </summary>
        [JsonProperty(PropertyName = "PMF", Required = Required.Default)]
        public double Pmf { get; set; }

        /// <summary>
        /// [P] Избыточное давление по наливной линии с готовым/производимым
        /// </summary>
        [JsonProperty(PropertyName = "P", Required = Required.Default)]
        public double P { get; set; }

        /// <summary>
        /// [PTF] Температура готового/производимого продукта
        /// </summary>
        [JsonProperty(PropertyName = "PTF", Required = Required.Default)]
        public double Ptf { get; set; }

        /// <summary>
        /// [PRF] Плотность готового/производимого продукта (факт)
        /// </summary>
        [JsonProperty(PropertyName = "PRF", Required = Required.Default)]
        public double Prf { get; set; }

        /// <summary>
        /// [TADJ] Температура приведения плотности (факт 15 или 20 С)
        /// </summary>
        [JsonProperty(PropertyName = "TAdj", Required = Required.Default)]
        public double Tadj { get; set; }

        /// <summary>
        /// [PRAdjF] Приведенная плотность готового/производимого продукта (факт)
        /// </summary>
        [JsonProperty(PropertyName = "PRAdjF", Required = Required.Default)]
        public double PrAdjF { get; set; }

        /// <summary>
        /// [DT1] Дата и время начала налива
        /// </summary>
        [JsonProperty(PropertyName = "DT1", Required = Required.Always)]
        public DateTime Dt1 { get; set; }

        /// <summary>
        /// [DT2] Дата и время окончания налива
        /// </summary>
        [JsonProperty(PropertyName = "DT2", Required = Required.Always)]
        public DateTime Dt2 { get; set; }

        /// <summary>
        /// [PSpd] Скорость слива
        /// </summary>
        [JsonProperty(PropertyName = "FSpd", Required = Required.Default)]
        public double PSpd { get; set; }

        /// <summary>
        /// [tRest] Остаток времени слива
        /// </summary>
        [JsonProperty(PropertyName = "tRest", Required = Required.Default)]
        public TimeSpan TimeRest { get; set; }
    }
}