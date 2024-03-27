using AutoMapper;
using Booking.Auth.Domain.Entities;
using Otus.Booking.Common.Booking.Contracts.User.Models;
using Otus.Booking.Common.Booking.Contracts.User.Requests;
using Otus.Booking.Common.Booking.Contracts.User.Responses;

namespace Booking.Auth.Application.Mappings;

public sealed class UserMapper : Profile
{
    public UserMapper()
    {
        // Create
        CreateMap<CreateUser, User>()
            .ForMember(d => d.Email, s => s.MapFrom(e => e.Email.ToLower()));
        CreateMap<User, CreateUserResult>()
            .ForMember(d => d.Email, s => s.MapFrom(e => e.Email.ToLower()));
	
	    // Read
        CreateMap<User, GetUserResult>();
        CreateMap<User, UserGettingDto>()
            .ForMember(d => d.Email, s => s.MapFrom(e => e.Email.ToLower()));

        // Update
        CreateMap<UpdateUser, User>()
            .ForMember(d => d.Email, s => s.MapFrom(e => e.Email.ToLower()));
        CreateMap<User, UpdateUserResult>()
            .ForMember(d => d.Email, s => s.MapFrom(e => e.Email.ToLower()));
    }
}