using AutoMapper;
using Otus.Booking.Common.Booking.Contracts.User.Models;
using Otus.Booking.Common.Booking.Contracts.User.Requests;
using Otus.Booking.Common.Booking.Contracts.User.Responses;

namespace Booking.Auth.Application.Mappings;

public sealed class UserMapper : Profile
{
    public UserMapper()
    {
        CreateMap<CreateUser, Domain.Entities.User>()
            .ForMember(d => d.Email, s => s.MapFrom(e => e.Email.ToLower()));
        CreateMap<Domain.Entities.User, CreateUserResult>()
            .ForMember(d => d.Email, s => s.MapFrom(e => e.Email.ToLower()));

        CreateMap<FullUserDto, Domain.Entities.User>()
            .ForMember(d => d.Email, s => s.MapFrom(e => e.Email.ToLower()));
        CreateMap<Domain.Entities.User, FullUserDto>()
            .ForMember(d => d.Email, s => s.MapFrom(e => e.Email.ToLower()));


        CreateMap<UpdateUser, Domain.Entities.User>()
            .ForMember(d => d.Email, s => s.MapFrom(e => e.Email.ToLower()));
        CreateMap<Domain.Entities.User, UpdateUserResult>()
            .ForMember(d => d.Email, s => s.MapFrom(e => e.Email.ToLower()));
    }
}