using AutoMapper;
using Server.Models;
using Server.Models.DTO;

namespace Server
{

    namespace Server.Profiles
    {
        public class AppProfile : Profile
        {
            public AppProfile()
            {
                CreateMap<GiftDTO, Gift>();
                CreateMap<CategoryDTO, Category>();
                CreateMap<DonorDTO, Donor>();
                CreateMap< UserDTO, User>();
                CreateMap<TicketDTO, Ticket>();
                CreateMap<RegisterDTO, User>();

            }
        }
    }
}
