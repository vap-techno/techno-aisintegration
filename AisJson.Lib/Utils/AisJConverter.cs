using System;
using System.Collections.Generic;
using AisJson.Lib.DTO;
using AisJson.Lib.DTO.Abstract;
using AisJson.Lib.DTO.Data;
using AisJson.Lib.DTO.Task;
using Newtonsoft.Json.Linq;


namespace AisJson.Lib.Utils
{
    public static class AisJConverter
    {
        /// <summary>
        /// Создает список команд от АИС ТПС
        /// </summary>
        /// <param name="json"> Строка JSON </param>
        /// <returns></returns>
        public static List<IRequestDto> Deserialize(string json)
        {
            // TODO: Встаить обработку исключений
            JArray res = JArray.Parse(json);
            List<IRequestDto> lst = new List<IRequestDto>();

            foreach (var t in res)
            {
                var c = (string) t["CMD"];

                switch (c)
                {
                    case "STATUS":
                        lst.Add(t.ToObject<StatusTaskDto>());
                        break;

                    case "FILL_IN":
                        lst.Add(t.ToObject<FillInTaskDto>());
                        break;

                    case "FILL_IN_MC":
                        lst.Add(t.ToObject<FillInMcTaskDto>());
                        break;

                    case "FILL_OUT":
                        lst.Add(t.ToObject<FillOutTaskDto>());
                        break;

                    case "CANCEL":
                        lst.Add(t.ToObject<CancelTaskDto>());
                        break;

                    default:
                        throw new Exception("Unknown command");
                }
            }

            return lst;
        }
    }
}
