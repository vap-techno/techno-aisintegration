using System;
using AisJson.Lib.DTO.Abstract;
using Newtonsoft.Json;

namespace AisJson.Lib.DTO.Data
{
    public class FillInMcDataDto: IRequestDataDto
    {

        /// <summary>
        /// [ID] Идентификатор задания АИС ТПС
        /// </summary>
        [JsonProperty(PropertyName = "ID", Required = Required.Always)]
        public string Id { get; set; }

        /// <summary>
        /// [TDT] Дата и время задания АИС ТПС
        /// </summary>
        [JsonProperty(PropertyName = "TDT", Required = Required.Always)]
        public DateTime Tdt { get; set; }

        /// <summary>
        /// [TNO] Номер задания АИС ТПС
        /// </summary>
        [JsonProperty(PropertyName = "TNo", Required = Required.Always)]
        public string Tno { get; set; }

        /// <summary>
        /// [ON] ФИО товарного оператора
        /// </summary>
        [JsonProperty(PropertyName = "ON", Required = Required.Always)]
        public string On { get; set; }

        /// <summary>
        /// [MM] Способ измерения в АИС ТПС
        /// </summary>
        [JsonProperty(PropertyName = "MM", Required = Required.Always)]
        public int Mm { get; set; }

        /// <summary>
        /// [LNP] Номер поста (план)
        /// </summary>
        [JsonProperty(PropertyName = "LNP", Required = Required.Always)]
        public string Lnp { get; set; }

        /// <summary>
        /// [PN] Наименование продукта
        /// </summary>
        [JsonProperty(PropertyName = "PN", Required = Required.Always)]
        public string Pn { get; set; }

        /// <summary>
        /// [TN] Номер резервуара
        /// </summary>
        [JsonProperty(PropertyName = "TN", Required = Required.Always)]
        public string Tn { get; set; }

        /// <summary>
        /// [PVP] Объем продукта (план)
        /// </summary>
        [JsonProperty(PropertyName = "PVP", Required = Required.Always)]
        public double Pvp { get; set; }

        /// <summary>
        /// [PMP] Масса  продукта (план)
        /// </summary>
        [JsonProperty(PropertyName = "PMP", Required = Required.Default)]
        public double? Pmp { get; set; } = 0;

    }
}