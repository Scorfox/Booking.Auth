using AutoMapper;
using Otus.Booking.Common.Booking.Contracts.Company.Models;
using Otus.Booking.Common.Booking.Contracts.Company.Requests;
using Otus.Booking.Common.Booking.Contracts.Company.Responses;

namespace Booking.Auth.Application.Mappings;

public sealed class CompanyMapper : Profile
{
    public CompanyMapper()
    {
        CreateMap<CreateCompany, Domain.Entities.Company>();
        CreateMap<Domain.Entities.Company, CreateCompanyResult>();
        
        CreateMap<UpdateCompany, Domain.Entities.Company>();
        CreateMap<Domain.Entities.Company, UpdateCompanyResult>();

        CreateMap<FullCompanyDto, Domain.Entities.Company>();
        CreateMap<Domain.Entities.Company, FullCompanyDto>();

        CreateMap<GetCompanyById, Domain.Entities.Company>();
        CreateMap<Domain.Entities.Company, GetCompanyResult>();
    }
}