using Booking.Auth.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Auth.Application.EntityTypeConfigurations;

public class FilialEntityTypeConfiguration : IEntityTypeConfiguration<Filial>
{
    public void Configure(EntityTypeBuilder<Filial> builder)
    {
        builder.HasIndex(i => i.Name)
            .IsUnique();
    }
}