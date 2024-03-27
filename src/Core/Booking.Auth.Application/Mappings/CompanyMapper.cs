using AutoMapper;
using Otus.Booking.Common.Booking.Contracts.Company.Models;
using Otus.Booking.Common.Booking.Contracts.Company.Requests;
using Otus.Booking.Common.Booking.Contracts.Company.Responses;

namespace Booking.Auth.Application.Mappings;

public sealed class CompanyMapper : Profile
{
    public CompanyMapper()
    {
        // Create
        CreateMap<CreateCompany, Domain.Entities.Company>();
        CreateMap<Domain.Entities.Company, CreateCompanyResult>();
        
        // Read
        CreateMap<Domain.Entities.Company, CompanyGettingDto>();
        CreateMap<Domain.Entities.Company, GetCompanyResult>();
        
        // Update
        CreateMap<UpdateCompany, Domain.Entities.Company>();
        CreateMap<Domain.Entities.Company, UpdateCompanyResult>();
    }
}