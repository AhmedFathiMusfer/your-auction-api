using AutoMapper;
using your_auction_api.Models;
using your_auction_api.Models.Dto;


namespace your_auction_api.MapppingConfig
{
    public class Mapping : Profile
    {
        public Mapping()
        {


            //Mapping The User
            CreateMap<UserDTO, ApplicationUser>().ReverseMap();
        }

    }
}
