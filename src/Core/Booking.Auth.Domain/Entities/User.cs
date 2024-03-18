using Booking.Auth.Domain.Common;

namespace Booking.Auth.Domain.Entities;

public class User : BaseEntity
{
    public string Email { get; set; }
    public string Login { get; set; }
    public string PasswordHash { get; set; }
    
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    
    public string PhoneNumber { get; set; }
    public bool EmailConfirmed { get; set; }
    
    public virtual Role Role { get; set; }
    public Guid RoleId { get; set; }
    public Guid? CompanyId { get; set; }
}