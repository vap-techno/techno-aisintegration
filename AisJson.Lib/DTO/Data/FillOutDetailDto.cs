using Newtonsoft.Json;

namespace AisJson.Lib.DTO.Data
{
    public class FillOutDetailDto
    {
        /// <summary>
        /// [SID] Идентификатор секции в контексте заданного задания
        /// </summary>
        [JsonProperty(PropertyName = "SID", Required = Required.Always)]
        public string Sid { get; set; }

        /// <summary>
        /// [LNP] Номер поста (план)
        /// </summary>
        [JsonProperty(PropertyName = "LNP", Required = Required.Always)]
        public string Lnp { get; set; }

        /// <summary>
        /// [SN] Номер секции АЦ
        /// </summary>
        [JsonProperty(PropertyName = "SN", Required = Required.Always)]
        public int Sn { get; set; }

        /// <summary>
        /// [PN] Наименование продукта
        /// </summary>
        [JsonProperty(PropertyName = "PN", Required = Required.Always)]
        public string Pn { get; set; }

        /// <summary>
        /// [TN] Номер резервуара с готовым продуктом или базовым топливом
        /// </summary>
        [JsonProperty(PropertyName = "TN", Required = Required.Always)]
        public string Tn { get; set; }

        /// <summary>
        /// [PVP] Объем продукта (план)
        /// </summary>
        [JsonProperty(PropertyName = "PVP", Required = Required.Always)]
        public double Pvp { get; set; }

        /// <summary>
        /// [PMP] Масса продукта (план)
        /// </summary>
        [JsonProperty(PropertyName = "PMP", Required = Required.Default)]
        public double? Pmp { get; set; } = 0;
    }
}