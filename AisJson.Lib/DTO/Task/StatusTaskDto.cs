using AisJson.Lib.DTO.Abstract;
using AisJson.Lib.DTO.Data;
using Newtonsoft.Json;

namespace AisJson.Lib.DTO.Task
{
    public class StatusTaskDto : IRequestDto

    {
        public string Cmd { get; set; }
        public string Cid { get; set; }

        public bool Validate()
        {
            var isValid = true;

            Data.Ids.ForEach(item =>
            {
                if (item == "" || item == "0") isValid = false;
            });

            return isValid;
        }

        /// <summary>
        /// [DATA] Объект данных команды
        /// </summary>
        [JsonProperty(PropertyName = "DATA", Required = Required.Always)]
        public StatusDataDto Data { get; set; }

    }
}