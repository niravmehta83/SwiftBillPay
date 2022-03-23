using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BingHousing_BO;
using BingHousingMVC.Models;

namespace BingHousingMVC
{
    public static class AutoMapperConfigurator
    {
        public static void Configure()
        {
			

            Mapper.CreateMap<Payee, PayeeModel>().ForMember(dest => dest.Payee, option => option.MapFrom(src => src.Payee1));
            Mapper.CreateMap<PayeeModel, Payee>().ForMember(dest => dest.Payee1, option => option.MapFrom(src => src.Payee));
            Mapper.CreateMap<UserModel, UserDetail>().ReverseMap();

            Mapper.CreateMap<CustomerModel, Customer>()
                   .ForMember(dest => dest.NextBillDate, option => option.MapFrom(src => Convert.ToDateTime(src.NextBillDate).Date
                       )).ForMember(dest => dest.IntervalTypeId, option => option.MapFrom(src => Convert.ToInt32(src.IntervalTypeID)));

            Mapper.CreateMap<Customer, CustomerModel>()
                   .ForMember(dest => dest.NextBillDate, option => option.MapFrom(src => Convert.ToDateTime(src.NextBillDate).ToString("MM-dd-yyyy")))
                   .ForMember(dest => dest.IntervalTypeID, option => option.MapFrom(src => src.IntervalTypeId.ToString()));
            

        }

    }
}
