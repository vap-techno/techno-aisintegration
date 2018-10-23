using System;
using AisJson.Lib.DTO.Abstract;
using AisJson.Lib.DTO.Data;
using Newtonsoft.Json;

namespace AisJson.Lib.DTO.Task
{
    public class FillInTaskDto: IRequestDto
    {
        public string Cmd { get; set; }
        public string Cid { get; set; }

        public bool Validate()
        {
            
            var isValid = true;

            Data.Details.ForEach(item =>
            {
                // Если признак производства ФТ (фирм. топлива) продукта указан, то поля BFN, AN, ATN, BFVP, AVP, BFMP, AMP должны присуствовать
                if (item.Ppf > 1)
                { 
                    isValid = false;
                    return;
                }
                else if ((item.Ppf == 1)
                    && (item.Bfn == ""
                        || item.An == ""
                        || item.Atn == ""
                        || item.Bfvp <= double.Epsilon || item.Bfvp == null
                        || item.Avp <= double.Epsilon || item.Avp == null
                        || item.Bfmp <= double.Epsilon || item.Bfmp == null
                        || item.Amp <= double.Epsilon || item.Amp == null))
                {
                    isValid = false;
                    return;
                }
            });

            return isValid;
        }

        /// <summary>
        /// [DATA] Объект данных команды
        /// </summary>
        [JsonProperty(PropertyName = "DATA", Required = Required.Always)]
        public FillInDataDto Data { get; set; }

    }
}