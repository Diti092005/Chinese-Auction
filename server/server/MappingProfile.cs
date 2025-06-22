using server.Models.DTO;
using server.Models;
using AutoMapper;
using server.DAL.intefaces;


namespace server
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<DonorDto, Donor>();
            CreateMap<Donor, DonorDtoResult>();
            CreateMap<Donor, DonorDtoTheen>();
            
            CreateMap<User, UserDtoTheen>();
            
            CreateMap<CategoryDto, Category>();

            CreateMap<GiftDto, Gift>();
            CreateMap<Gift, GiftDtoResult>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));
            CreateMap<Gift, GiftDtoTheen>();

            CreateMap<Ticket, TicketDtoTheen>();

            CreateMap<RegisterDto, User>();

            CreateMap<TicketDto, Ticket>()
                .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => DateOnly.FromDateTime( DateTime.Today)));
            CreateMap<Ticket, TicketDtoResult>();
        }
    }
}
