using Booking.Auth.Domain.Common;

namespace Booking.Auth.Domain.Entities;

public class Filial : BaseEntity
{
    public Company Company { get; set; }
    public Guid CompanyId { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string? Description { get; set; }
}