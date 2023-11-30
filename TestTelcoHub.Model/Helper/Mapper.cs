using AutoMapper;
using TestTelcoHub.Model.DTOs;
using TestTelcoHub.Model.Model;

namespace TestTelcoHub.Model.Helper
{
    public class Mapper : Profile
    {
        public Mapper() 
        {
            CreateMap<Plan, PlanDTO>().ReverseMap();
            CreateMap<Address,AddressDTO>().ReverseMap();
            CreateMap<Contacts,ContactsDTO>().ReverseMap();
            CreateMap<CustomerAgreements, CustomerAgreementsDTO>().ReverseMap();
            CreateMap<Customer, CustomerDTO>().ReverseMap();
            CreateMap<Distributor,DistributorDTO>().ReverseMap();
            CreateMap<Expiration, ExpirationDTO>().ReverseMap();
            CreateMap<ExternalReference, ExternalReferenceDTO>().ReverseMap();
            CreateMap<TermsAndConditions, TermsAndConditionsDTO>().ReverseMap();
            CreateMap<MonmentType, MonmentTypeDTO>().ReverseMap();
            CreateMap<ApprovalCode, ApprovalCodeDTO>().ReverseMap();
            CreateMap<PlanDTO, PurchaseHistory>()
            .ForMember(dest => dest.StatusSubscription, opt => opt.MapFrom(src => SubscriptionStatus.Active))
            .ForMember(dest => dest.ExactMoment, opt => opt.MapFrom(src => src.ExpirationDTO!.ExactMoment))
            .ForMember(dest => dest.AfterMoment, opt => opt.MapFrom(src => src.ExpirationDTO!.AfterMoment))
            .ForMember(dest => dest.PeriodCount, opt => opt.MapFrom(src => src.ExpirationDTO!.PeriodCount))
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.BillingPlan))
            .ForMember(dest => dest.Nodes, opt => opt.MapFrom(src => src.Quantity))
            .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Customer!.Contacts!.CompanyName))
            .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Customer!.Address!.Country));
        }
    }
}
