using AutoMapper;
using Otus.Booking.Common.Booking.Contracts.Filial.Models;
using Otus.Booking.Common.Booking.Contracts.Filial.Requests;
using Otus.Booking.Common.Booking.Contracts.Filial.Responses;

namespace Booking.Auth.Application.Mappings;

public sealed class FilialMapper : Profile
{
    public FilialMapper()
    {
        // Create
        CreateMap<CreateFilial, Domain.Entities.Filial>();
        CreateMap<Domain.Entities.Filial, CreateFilialResult>();
        
        // Read
        CreateMap<Domain.Entities.Filial, GetFilialResult>();
        CreateMap<Domain.Entities.Filial, FilialGettingDto>();
        
        // Update
        CreateMap<UpdateFilial, Domain.Entities.Filial>();
        CreateMap<Domain.Entities.Filial, UpdateFilialResult>();
    }
}