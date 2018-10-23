using Newtonsoft.Json;

namespace AisJson.Lib.DTO.Data
{
    /// <summary>
    /// Детализирующие данные налива в АЦ
    /// </summary>
    public class FillInDetailDto
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
        /// [FM] Способ налива
        /// </summary>
        [JsonProperty(PropertyName = "FM", Required = Required.Always)]
        public int Fm { get; set; }

        /// <summary>
        /// [PPF] Признак производства продукта
        /// </summary>
        [JsonProperty(PropertyName = "PPF", Required = Required.Default)]
        public int Ppf { get; set; } = 0;

        /// <summary>
        /// [PN] Наименование готового/производимого продукта
        /// </summary>
        [JsonProperty(PropertyName = "PN", Required = Required.Always)]
        public string Pn { get; set; }

        /// <summary>
        /// [BFN] Наименование базового топлива
        /// </summary>
        [JsonProperty(PropertyName = "BFN", Required = Required.Default)]
        public string Bfn { get; set; } = "";

        /// <summary>
        /// [AN] Наименование присадки
        /// </summary>
        [JsonProperty(PropertyName = "AN", Required = Required.Default)]
        public string An { get; set; } = "";

        /// <summary>
        /// [TN] Номер резервуара с готовым продуктом или базовым топливом
        /// </summary>
        [JsonProperty(PropertyName = "TN", Required = Required.Always)]
        public string Tn { get; set; }

        /// <summary>
        /// [ATN] Номер резервуара с присадкой
        /// </summary>
        [JsonProperty(PropertyName = "ATN", Required = Required.Default)]
        public string Atn { get; set; } = "";

        /// <summary>
        /// [PVP] Объем готового/производимого продукта (план)
        /// </summary>
        [JsonProperty(PropertyName = "PVP", Required = Required.Always)]
        public double Pvp { get; set; }

        /// <summary>
        /// [BFVP] Объем базового топлива (план)
        /// </summary>
        [JsonProperty(PropertyName = "BFVP", Required = Required.Default)]
        public double? Bfvp { get; set; } = 0;

        /// <summary>
        /// [AVP] Объем присадки (план)
        /// </summary>
        [JsonProperty(PropertyName = "AVP", Required = Required.Default)]
        public double? Avp { get; set; } = 0;

        /// <summary>
        /// [PMP] Масса готового/производимогого продукта (план)
        /// </summary>
        [JsonProperty(PropertyName = "PMP", Required = Required.Default)]
        public double? Pmp { get; set; } = 0;

        /// <summary>
        /// [BFMP] Масса базового топлива (план)
        /// </summary>
        [JsonProperty(PropertyName = "BFMP", Required = Required.Default)]
        public double? Bfmp { get; set; } = 0;

        /// <summary>
        /// [AMP] Масса присадки (план)
        /// </summary>
        [JsonProperty(PropertyName = "AMP", Required = Required.Default)]
        public double? Amp { get; set; } = 0;


    }
}