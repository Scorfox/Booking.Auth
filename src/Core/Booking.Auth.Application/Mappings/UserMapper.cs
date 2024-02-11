using AutoMapper;
using Otus.Booking.Common.Booking.Contracts.Authentication.Requests;
using Otus.Booking.Common.Booking.Contracts.Authentication.Responses;

namespace Booking.Auth.Application.Mappings;

public sealed class UserMapper : Profile
{
    public UserMapper()
    {
        CreateMap<CreateUser, Domain.Entities.User>();
        CreateMap<Domain.Entities.User, CreateUserResult>();
        
        CreateMap<UpdateUser, Domain.Entities.User>();
        CreateMap<Domain.Entities.User, UpdateUserResult>();
    }
}