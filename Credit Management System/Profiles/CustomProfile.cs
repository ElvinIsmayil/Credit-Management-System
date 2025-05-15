using AutoMapper;
using Credit_Management_System.Models;
using Credit_Management_System.ViewModels.Merchant;

namespace Credit_Management_System.Profiles
{
    public class CustomProfile : Profile
    {
        public CustomProfile()
        {
            CreateMap<Merchant, MerchantVM>().ReverseMap();
            CreateMap<Merchant, MerchantCreateVM>().ReverseMap();
            CreateMap<Merchant, MerchantUpdateVM>().ReverseMap();
            CreateMap<Merchant, MerchantDetailVM>().ReverseMap();
        }
    }
}
