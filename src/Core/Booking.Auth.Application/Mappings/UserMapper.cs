using AutoMapper;
using Otus.Booking.Common.Booking.Contracts.User.Requests;
using Otus.Booking.Common.Booking.Contracts.User.Responses;

namespace Booking.Auth.Application.Mappings
{
    public sealed class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<GetUserId, Domain.Entities.User>();
            CreateMap<Domain.Entities.User, GetUsersListResult>();
        }
    }
}
Т