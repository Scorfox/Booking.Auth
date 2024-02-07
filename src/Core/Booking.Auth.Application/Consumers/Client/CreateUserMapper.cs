using AutoMapper;
using Booking.Auth.Domain.Entities;
using Otus.Booking.Common.Booking.Contracts.Authentication.Requests;
using Otus.Booking.Common.Booking.Contracts.Authentication.Responses;

namespace Booking.Auth.Application.Consumers.Client;

public sealed class CreateUserMapper : Profile
{
    public CreateUserMapper()
    {
        CreateMap<CreateClient, User>();
        CreateMap<User, CreateClientResult>();
    }
}