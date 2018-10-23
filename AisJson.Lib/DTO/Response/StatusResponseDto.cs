using AisJson.Lib.DTO.Abstract;
using Newtonsoft.Json;

namespace AisJson.Lib.DTO.Response
{
    public class StatusResponseDto : IResponseDto
    {

        /// <summary>
        /// [ID] Идентификатор задания в АИС ТПС
        /// </summary>
        [JsonProperty("ID")]
        public string Id { get; set; }

        /// <summary>
        /// [CID] Идентификатор команды, в ответ на которую предоставляется статус
        /// </summary>
        [JsonProperty("CID")]
        public string Cid { get; set; }

        /// <summary>
        /// [SC] Код статуса задания в АСН
        /// </summary>
        [JsonProperty("SC")]
        public int Sc { get; set; }

        /// <summary>
        /// [RM] Сообщение о результате выполнения задания
        /// </summary>
        [JsonProperty("RM")]
        public string Rm { get; set; }

        /// <summary>
        /// [SD] Статус задания 
        /// </summary>
        [JsonProperty("SD")]
        public IStatusDetailDto Sd { get; set; }
        
    }
}
