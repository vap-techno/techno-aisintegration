using System.Collections.Generic;
using AisJson.Lib.DTO.Abstract;
using Newtonsoft.Json;

namespace AisJson.Lib.DTO.Status
{
    public class FillInStatusDetailDto : IStatusDetailDto
    {
        /// <summary>
        /// [Details] Детализирующие данные
        /// </summary>
        [JsonProperty(PropertyName = "Details", Required = Required.Always)]
        public List<FillInStatusDetailDetailDto> Details { get; set; }
    }
}