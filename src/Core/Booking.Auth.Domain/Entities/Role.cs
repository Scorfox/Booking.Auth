using Microsoft.AspNetCore.Identity;

namespace Booking.Auth.Domain.Entities;

public class Role : IdentityRole<Guid>
{
    public bool IsActive { get; set; }
    public string? Description { get; set; }
}