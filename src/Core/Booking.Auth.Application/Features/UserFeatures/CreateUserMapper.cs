using AutoMapper;
using Booking.Auth.Domain.Entities;

namespace Booking.Auth.Application.Features.UserFeatures;

public sealed class CreateUserMapper : Profile
{
    public CreateUserMapper()
    {
        CreateMap<CreateUserRequest, User>();
        CreateMap<User, CreateUserResponse>();
    }
}