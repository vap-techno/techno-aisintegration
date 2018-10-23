using System;
using AisJson.Lib.DTO;
using AisJson.Lib.DTO.Response;
using AisJson.Lib.DTO.Task;
using AutoMapper;
using DAL.Entity;

namespace DAL.Core.TaskMapper.Profiles
{
    public class CancelProfile : Profile
    {
        public CancelProfile()
        {
            CreateMap<CancelTaskDto, CancelTask>()
                .ForMember(d => d.Ids, opt => opt.MapFrom(src => src.Data.Ids));

            CreateMap<CancelResponse, CancelResponseDto>();
        }
    }
}