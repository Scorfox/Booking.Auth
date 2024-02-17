using Booking.Auth.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Auth.Application.EntityTypeConfigurations;

public class CompanyEntityTypeConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.HasIndex(i => i.Inn)
            .IsUnique();

        builder.HasMany<Filial>()
            .WithOne(user => user.Company)
            .HasForeignKey(e => e.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}