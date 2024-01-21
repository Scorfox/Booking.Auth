using MediatR;

namespace Booking.Auth.Application.Features.ClientFeatures;

public sealed record CreateClientRequest : IRequest<CreateClientResponse>
{
    public string Email { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    
    public string PhoneNumber { get; set; }
}