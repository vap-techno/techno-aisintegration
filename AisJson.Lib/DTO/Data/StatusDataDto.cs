using System.Collections.Generic;
using AisJson.Lib.DTO.Abstract;
using Newtonsoft.Json;

namespace AisJson.Lib.DTO.Data
{
    public class StatusDataDto : IRequestDataDto
    {
        /// <summary>
        /// [IDs] Идентификаторы заданий АИС ТПС
        /// </summary>
        [JsonProperty(PropertyName = "IDs", Required = Required.Always)]
        public List<string> Ids { get; set; }
    }
}