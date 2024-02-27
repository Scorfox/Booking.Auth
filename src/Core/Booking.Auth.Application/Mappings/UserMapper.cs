using AutoMapper;

namespace Booking.Auth.Application.Mappings
{
    public sealed class UserMapper : Profile
    {
        CreateMap<GetUser, Domain.Entities.User>();
        CreateMap<Domain.Entities.User, GetUser>();
    }
}
