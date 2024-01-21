using MediatR;

namespace Booking.Auth.Application.Features.AuthFeatures;

public sealed record AuthenticateRequest : IRequest<(bool success, string? roleName)>
{
    public string Email { get; set; }
    public string Password { get; set; }
}