using System.Collections.Generic;
using AisJson.Lib.DTO;
using AisJson.Lib.DTO.Data;
using AisJson.Lib.DTO.Status;
using AisJson.Lib.DTO.Task;
using AutoMapper;
using DAL.Entity;
using DAL.Entity.Status;

namespace DAL.Core.TaskMapper.Profiles
{
    public class FillInProfile : Profile
    {
        public FillInProfile()
        {
            // FIllInTaskDto <===> FillInTask
            CreateMap<FillInTaskDto, FillInTask>()
                .ForMember(d => d.AisTaskId, opt => opt.MapFrom(src => src.Data.Id))
                .ForMember(d => d.Tdt, opt => opt.MapFrom(src => src.Data.Tdt))
                .ForMember(d => d.Tno, opt => opt.MapFrom(src => src.Data.Tno))
                .ForMember(d => d.Pn, opt => opt.MapFrom(src => src.Data.Pn))
                .ForMember(d => d.On, opt => opt.MapFrom(src => src.Data.On))
                .ForMember(d => d.Dn, opt => opt.MapFrom(src => src.Data.Dn))
                .ForMember(d => d.Mm, opt => opt.MapFrom(src => src.Data.Mm))
                .ForPath(d => d.Details, opt => opt.MapFrom(src => src.Data.Details));

            CreateMap<FillInDetailDto, FillInDetail>();

            // FIllInStatusDetail <===> FillInStatusDetailDto
            CreateMap<FillInStatusDetail, FillInStatusDetailDto>();

            // FillInStatusDetailDetail <===> FillInStatusDetailDetailDto
            CreateMap<FillInStatusDetailDetail, FillInStatusDetailDetailDto>();

            // FillInTask ===> FillInStatusDetail
            CreateMap<FillInDetail, FillInStatusDetailDetail>();


            CreateMap<FillInTask, FillInStatusDetail>()
                .ForMember(d => d.Details, opt => opt.MapFrom(src => src.Details));
            

        }
    }
}