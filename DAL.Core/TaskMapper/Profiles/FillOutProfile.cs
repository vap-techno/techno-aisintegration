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
    public class FillOutProfile : Profile
    {
        public FillOutProfile()
        {
            CreateMap<FillOutTaskDto, FillOutTask>()
                .ForMember(d => d.AisTaskId, opt => opt.MapFrom(src => src.Data.Id))
                .ForMember(d => d.Tdt, opt => opt.MapFrom(src => src.Data.Tdt))
                .ForMember(d => d.Tno, opt => opt.MapFrom(src => src.Data.Tno))
                .ForMember(d => d.Mm, opt => opt.MapFrom(src => src.Data.Mm))
                .ForMember(d => d.On, opt => opt.MapFrom(src => src.Data.On))
                .ForPath(d => d.Details, opt => opt.MapFrom(src => src.Data.Details));

            CreateMap<FillOutDetailDto, FillOutDetail>();

            // FIllOutStatusDetail <===> FillOutStatusDetailDto
            CreateMap<FillOutStatusDetail, FillOutStatusDetailDto>();

            // FillOutStatusDetailDetail <===> FillOutStatusDetailDetailDto
            CreateMap<FillOutStatusDetailDetail, FillOutStatusDetailDetailDto>();

            // FillOutTask ===> FillOutStatusDetail
            CreateMap<FillOutTask, FillOutStatusDetail>();
            CreateMap<FillOutDetail, FillOutStatusDetailDetail>();
        }
    }
}
