using AutoMapper;
using DTCWaitingList.Models;
using DTCWaitingList.Views;

namespace DTCWaitingList.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Appointment, AppointmentView>()
                .ForMember(dest => dest.ReasonId, opt => opt.MapFrom(src => src.ReasonId.ToString())).ReverseMap()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate)).ReverseMap();
        }
    }
}
