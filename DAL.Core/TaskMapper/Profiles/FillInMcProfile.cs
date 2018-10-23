using System;
using AisJson.Lib.DTO;
using AisJson.Lib.DTO.Data;
using AisJson.Lib.DTO.Status;
using AisJson.Lib.DTO.Task;
using AutoMapper;
using DAL.Entity;
using DAL.Entity.Status;

namespace DAL.Core.TaskMapper.Profiles
{
    public class FillInMcProfile : Profile
    {
        public FillInMcProfile()
        {
            CreateMap<FillInMcTaskDto, FillInMcTask>()
                .ForMember(d => d.AisTaskId, opt => opt.MapFrom(src => src.Data.Id))
                .ForMember(d => d.Tdt, opt => opt.MapFrom(src => src.Data.Tdt))
                .ForMember(d => d.Tno, opt => opt.MapFrom(src => src.Data.Tno))
                .ForMember(d => d.On, opt => opt.MapFrom(src => src.Data.On))
                .ForMember(d => d.Mm, opt => opt.MapFrom(src => src.Data.Mm))
                .ForMember(d => d.Lnp, opt => opt.MapFrom(src => src.Data.Lnp))
                .ForMember(d => d.Pn, opt => opt.MapFrom(src => src.Data.Pn))
                .ForMember(d => d.Tn, opt => opt.MapFrom(src => src.Data.Tn))
                .ForMember(d => d.Pvp, opt => opt.MapFrom(src => src.Data.Pvp))
                .ForMember(d => d.Pmp, opt => opt.MapFrom(src => src.Data.Pmp))
                ;
                

            CreateMap<FillInMcStatusDetail, FillInMcStatusDetailDto>();


            



        }
    }
}