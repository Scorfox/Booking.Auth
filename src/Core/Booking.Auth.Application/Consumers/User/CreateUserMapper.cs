using AutoMapper;
using Otus.Booking.Common.Booking.Contracts.Authentication.Requests;
using Otus.Booking.Common.Booking.Contracts.Authentication.Responses;

namespace Booking.Auth.Application.Consumers.User;

public sealed class CreateUserMapper : Profile
{
    public CreateUserMapper()
    {
        CreateMap<CreateUser, Domain.Entities.User>();
        CreateMap<Domain.Entities.User, CreateUserResult>();
    }
}