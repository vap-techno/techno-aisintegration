using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AisJson.Lib.DTO;
using AisJson.Lib.DTO.Abstract;
using AisJson.Lib.DTO.Response;
using AisJson.Lib.DTO.Status;
using AisJson.Lib.DTO.Task;
using AutoMapper;
using DAL.Core.TaskMapper.Profiles;
using DAL.Entity;
using DAL.Entity.Abstract;
using DAL.Entity.Status;

namespace DAL.Core.TaskMapper
{
    public class TaskMapper
    {

        static TaskMapper()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new FillInProfile());
                cfg.AddProfile(new FillInMcProfile());
                cfg.AddProfile(new FillOutProfile());
                cfg.AddProfile(new CancelProfile());
                cfg.AddProfile(new StatusProfile());
            });
        }

        
        /// <summary>
        /// Возвращает DTO команды на отмену заявки
        /// </summary>
        /// <param name="resp"></param>
        /// <returns></returns>
        public CancelResponseDto GetCancelResponseDto(CancelResponse resp)
        {
            

            if (resp.Ts == null)
            {
                var sr = new StatusResponse()
                {
                    Id = resp.Id,
                    Cid = resp.Cid,
                    Sc = 4,
                    Rm = "TS задан как null"
                };

                resp.Ts = sr;
            }

            var dto = new CancelResponseDto {
                Id = resp.Id,
                Cid = resp.Cid,
                R = resp.R,
                Rm = resp.Rm,
                Ts = GetStatusResponseDto(resp.Ts as StatusResponse)
            };


            return dto;
        }

        /// <summary>
        /// Возвращает DTO команды на получение статуса заявки
        /// </summary>
        /// <param name="resp"></param>
        /// <returns></returns>
        public StatusResponseDto GetStatusResponseDto(StatusResponse resp)
        {
            if (resp == null) return null;

            var dto = new StatusResponseDto()
            {
                Id = resp.Id,
                Cid = resp.Cid,
                Sc = resp.Sc,
                Rm = resp.Rm
                
            };

            if (resp.Sd == null)
            {
                dto.Sd = new EmptyStatusDetailDto();
                return dto;
            }

            switch (resp.Sd.GetType().Name)
            {
                case ("FillInStatusDetail"):
                    dto.Sd = Mapper.Map<FillInStatusDetailDto>(resp.Sd);
                    break;

                case ("FillInMcStatusDetail"):
                    dto.Sd = Mapper.Map<FillInMcStatusDetailDto>(resp.Sd);
                    break;

                case ("FillOutStatusDetail"):
                    dto.Sd = Mapper.Map<FillOutStatusDetailDto>(resp.Sd);
                    break;
                default:
                    dto.Sd = new EmptyStatusDetailDto();
                    break;
            }
            return dto;
        }
    }
}