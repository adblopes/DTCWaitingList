using AutoMapper;
using DTCWaitingList.Database.Models;
using DTCWaitingList.Models;

namespace DTCWaitingList.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Patient, PatientView>()
                .ForMember(dest => dest.Reason, opt => opt.MapFrom(src => src.Reason!.ReasonName))
                .ForMember(dest => dest.PatientDays, opt => opt.MapFrom<PatientDayToStringListResolver>())
                .ForMember(dest => dest.PatientTimes, opt => opt.MapFrom<PatientTimeToStringListResolver>());

            CreateMap<PatientView, Patient>()
                .ForMember(dest => dest.Reason, opt => opt.Ignore())
                .ForMember(dest => dest.ReasonId, opt => opt.MapFrom<ReasonToReasonIdResolver>())
                .ForMember(dest => dest.PatientDays, opt => opt.MapFrom<StringToPatientDayListResolver>())
                .ForMember(dest => dest.PatientTimes, opt => opt.MapFrom<StringToPatientTimeListResolver>());

            CreateMap<Patient, PatientHistory>()
                .ForMember(dest => dest.DeletedDate, opt => opt.MapFrom(src => DateTime.Now));
        }
    }
}
