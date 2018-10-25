using System;
using System.Collections.Generic;
using AisJson.Lib.DTO.Abstract;
using AisJson.Lib.DTO.Task;
using AisJson.Lib.DTO;
using AisJson.Lib.DTO.Data;
using Newtonsoft.Json.Linq;
using Serilog;


namespace AisJson.Lib.Utils
{
    public static class AisJConverter
    {
        /// <summary>
        /// Создает список команд от АИС ТПС
        /// </summary>
        /// <param name="json">Строка команды в формате JSON</param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public static List<IRequestDto> Deserialize(string json, ILogger logger)
        {
            var lst = new List<IRequestDto>();

            try
            {
                var res = JArray.Parse(json);

                foreach (var t in res)
                {
                    var c = (string)t["CMD"];

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
                            logger.Warning("Неизвестная команда {c}", c);
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                logger.Warning("Неверный JSON-формат команды {e}", e);
            }
            return lst;
        }
    }
}
