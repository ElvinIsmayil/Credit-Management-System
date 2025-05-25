using AutoMapper;
using Credit_Management_System.Areas.Admin.ViewModels.User;
using Credit_Management_System.Models;
using Credit_Management_System.ViewModels;
using Credit_Management_System.ViewModels.Branch;
using Credit_Management_System.ViewModels.Category;
using Credit_Management_System.ViewModels.Merchant;
using Credit_Management_System.ViewModels.Product;

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

            CreateMap<Branch, BranchVM>()
      .ForMember(dest => dest.MerchantName, opt => opt.MapFrom(src => src.Merchant != null ? src.Merchant.Name : string.Empty));


            CreateMap<Branch, BranchCreateVM>().ReverseMap();

            CreateMap<Branch, BranchUpdateVM>().ReverseMap();

            CreateMap<Branch, BranchDetailVM>()
                .ForMember(dest => dest.MerchantName, opt => opt.MapFrom(src => src.Merchant != null ? src.Merchant.Name : string.Empty))
                .ReverseMap();




            CreateMap<Category, ParentCategoryVM>();

            CreateMap<Category, CategoryVM>()
                .ForMember(dest => dest.SubCategories, opt => opt.MapFrom(src => src.SubCategories));

            CreateMap<Category, CategoryCreateVM>().ReverseMap();
            CreateMap<Category, CategoryUpdateVM>().ReverseMap();
            CreateMap<Category, CategoryDetailVM>().ReverseMap();

            CreateMap<Product, ProductVM>().ReverseMap();
            CreateMap<Product, ProductCreateVM>().ReverseMap();
            CreateMap<Product, ProductUpdateVM>().ReverseMap();
            CreateMap<Product, ProductDetailVM>().ReverseMap();

            CreateMap<AppUser, UserVM>().ReverseMap();
            CreateMap<AppUser, UserCreateVM>().ReverseMap();
            CreateMap<AppUser, UserUpdateVM>().ReverseMap();
            CreateMap<AppUser, UserDetailVM>().ReverseMap();



        }
    }
}
