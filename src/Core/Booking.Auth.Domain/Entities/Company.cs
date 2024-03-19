using Booking.Auth.Domain.Common;

namespace Booking.Auth.Domain.Entities;

public class Company : BaseEntity
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public string Inn { get; set; }
    public string MainAddress { get; set; }
    
    public virtual List<Filial> Filials { get; set; }
}