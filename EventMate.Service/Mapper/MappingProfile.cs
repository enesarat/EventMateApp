using AutoMapper;
using EventMate.Core.DTO.Concrete.Category;
using EventMate.Core.DTO.Concrete.City;
using EventMate.Core.DTO.Concrete.Event;
using EventMate.Core.DTO.Concrete.Role;
using EventMate.Core.DTO.Concrete.Ticket;
using EventMate.Core.DTO.Concrete.User;
using EventMate.Core.Model.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EventMate.Service.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category,CategoryDto>().ReverseMap();
            CreateMap<CategoryCreateDto,Category>();
            CreateMap<CategoryUpdateDto,Category>();

            CreateMap<EventCreateDto, Event>();
            CreateMap<EventUpdateDto, Event>();
            CreateMap<Event, EventDto>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City.Name))
            .ReverseMap();

            CreateMap<City, CityDto>().ReverseMap();
            CreateMap<CityCreateDto, City>();
            CreateMap<CityUpdateDto, City>();

            CreateMap<Role, RoleDto>().ReverseMap();
            CreateMap<RoleCreateDto, Role>();
            CreateMap<RoleUpdateDto, Role>();

            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.Name))
                .ReverseMap();
            CreateMap<UserCreateDto, User>();
            CreateMap<UserUpdateDto, User>();
            CreateMap<UserUpdateAsAdminDto, User>();

            CreateMap<TicketCreateDto, Ticket>();
            CreateMap<TicketUpdateDto, Ticket>();
            CreateMap<Ticket, TicketDto>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User.Email))
            .ForMember(dest => dest.Event, opt => opt.MapFrom(src => src.Event.Name))
            .ReverseMap();
        }
        
    }
}
