using AutoMapper;
using Otus.Booking.Common.Booking.Contracts.Filial.Models;
using Otus.Booking.Common.Booking.Contracts.Filial.Requests;
using Otus.Booking.Common.Booking.Contracts.Filial.Responses;

namespace Booking.Auth.Application.Mappings;

public sealed class FilialMapper : Profile
{
    public FilialMapper()
    {
        CreateMap<CreateFilial, Domain.Entities.Filial>();
        CreateMap<Domain.Entities.Filial, CreateFilialResult>();
        
        CreateMap<UpdateFilial, Domain.Entities.Filial>();
        CreateMap<Domain.Entities.Filial, UpdateFilialResult>();

        CreateMap<FullFilialDto, Domain.Entities.Filial>();
        CreateMap<Domain.Entities.Filial, FullFilialDto>();

        CreateMap<GetFilialId, Domain.Entities.Filial>();
        CreateMap<Domain.Entities.Filial, GetFilialResult>();
    }
}