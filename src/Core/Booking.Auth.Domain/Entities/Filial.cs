using Booking.Auth.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Booking.Auth.Domain.Entities;

public class Filial : BaseEntity
{
    public Guid CompanyId { get; set; }
    [ForeignKey("CompanyId")]
    public Company Company { get; set; }

    public string Name { get; set; }
    public string Address { get; set; }
    public string? Description { get; set; }
}