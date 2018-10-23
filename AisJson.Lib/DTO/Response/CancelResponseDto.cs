using AisJson.Lib.DTO.Abstract;
using Newtonsoft.Json;

namespace AisJson.Lib.DTO.Response
{
    public class CancelResponseDto : IResponseDto
    {
        /// <summary>
        /// [ID] Идентификатор задания АИС ТПС
        /// </summary>
        [JsonProperty("ID")]
        public string Id { get; set; }

        /// <summary>
        /// [CID] Идентификатор команды, в ответ на которую предоставляется Результат отмены задания
        /// </summary>
        [JsonProperty("CID")]
        public string Cid { get; set; }

        /// <summary>
        /// [R] Признак отсутствия ошибок при попытке отмены задания.
        /// Невозможность отмены по объективными причинам, указанным в п. 6.6, не считается ошибкой.
        /// </summary>
        [JsonProperty("R")]
        public bool R { get; set; }

        /// <summary>
        /// [RM] Сообщение о результате отмены задания
        /// (может содержать причины возникающих при отмене задания ошибок и т.п.)
        /// </summary>
        [JsonProperty("RM")]
        public string Rm { get; set; }

        /// <summary>
        /// [TS] Статус задания
        /// </summary>
        [JsonProperty("TS")]
        public IStatusDetailDto Ts { get; set; }
    }
}