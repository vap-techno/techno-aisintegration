using Newtonsoft.Json;

namespace AisJson.Lib.DTO.Abstract
{
    public interface IRequestDto
    {
        /// <summary>
        /// [CMD] Тип команды
        /// </summary>
        [JsonProperty(PropertyName = "CMD", Required = Required.Always)]
        string Cmd { get; set; }

        /// <summary>
        /// [CID] Идентификатор команды
        /// </summary>
        [JsonProperty(PropertyName = "CID", Required = Required.Always)]
        string Cid { get; set; }

        /// <summary>
        /// Валидация задачи
        /// </summary>
        /// <returns></returns>
        bool Validate();




    }
}
