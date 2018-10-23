using System.Collections.Generic;
using AisJson.Lib.DTO.Abstract;
using Newtonsoft.Json;

namespace AisJson.Lib.DTO.Data
{
    public class CancelDataDto : IRequestDataDto
    {
        
        /// <summary>
        /// [IDs] Идентификаторы заданий АИС ТПС
        /// </summary>
        [JsonProperty("IDs")]
        public List<string> Ids { get; set; }
    }
}