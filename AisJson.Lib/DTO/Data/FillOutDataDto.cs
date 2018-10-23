using System;
using System.Collections.Generic;
using AisJson.Lib.DTO.Abstract;
using Newtonsoft.Json;

namespace AisJson.Lib.DTO.Data
{
    public class FillOutDataDto : IRequestDataDto
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
        /// [Details] Детализирующие данные
        /// </summary>
        [JsonProperty(PropertyName = "Details", Required = Required.Always)]
        public List<FillOutDetailDto> Details { get; set; }


    }
}