using System;
using System.Collections.Generic;
using AisJson.Lib.DTO.Abstract;
using Newtonsoft.Json;

namespace AisJson.Lib.DTO.Data
{
    public class FillInDataDto: IRequestDataDto
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
        /// [PN] Гос. номер АЦ
        /// </summary>
        [JsonProperty(PropertyName = "PN", Required = Required.Always)]
        public string Pn { get; set; }

        /// <summary>
        /// [DN] ФИО водителя
        /// </summary>
        [JsonProperty(PropertyName = "DN", Required = Required.Always)]
        public string Dn { get; set; }

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
        /// [Details] Детализирующие данные
        /// </summary>
        [JsonProperty(PropertyName = "Details", Required = Required.Always)]
        public List<FillInDetailDto> Details { get; set; }

    }
}