using System;
using AisJson.Lib.DTO.Abstract;
using AisJson.Lib.DTO.Data;
using Newtonsoft.Json;

namespace AisJson.Lib.DTO.Task
{
    public class FillOutTaskDto : IRequestDto
    {
        public string Cmd { get; set; }
        public string Cid { get; set; }
        public bool Validate()
        {
            var isValid = true;

            Data.Details.ForEach(item =>
            {
                if (item.Pmp == null && Data.Mm == 3)
                {
                    isValid = false;
                }

            });

            return isValid;
        }

        /// <summary>
        /// [DATA] Объект данных команды
        /// </summary>
        [JsonProperty(PropertyName = "DATA", Required = Required.Always)]
        public FillOutDataDto Data { get; set; }

    }
}