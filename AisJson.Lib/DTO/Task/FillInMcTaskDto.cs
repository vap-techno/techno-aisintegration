using AisJson.Lib.DTO.Abstract;
using AisJson.Lib.DTO.Data;
using Newtonsoft.Json;

namespace AisJson.Lib.DTO.Task
{
    public class FillInMcTaskDto: IRequestDto
    {
        public string Cmd { get; set; }
        public string Cid { get; set; }
        public bool Validate()
        {

            if (Data.Pmp == null && Data.Mm == 3)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// [DATA] Объект данных команды
        /// </summary>
        [JsonProperty(PropertyName = "DATA", Required = Required.Always)]
        public FillInMcDataDto Data { get; set; }

    }
}