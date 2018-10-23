using System;
using AisJson.Lib.DTO;
using AisJson.Lib.DTO.Task;
using AutoMapper;
using DAL.Entity;

namespace DAL.Core.TaskMapper.Profiles
{
    public class StatusProfile : Profile
    {
        public StatusProfile()
        {
            CreateMap<StatusTaskDto, StatusTask>()
                .ForMember(d => d.Ids, opt => opt.MapFrom(src => src.Data.Ids));


        }
    }
}