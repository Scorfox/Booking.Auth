using Booking.Auth.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Auth.Application.EntityTypeConfigurations;

public class RoleTypeConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasIndex(i => i.Name)
            .IsUnique();

        builder.HasMany<User>()
            .WithOne(user => user.Role)
            .HasForeignKey(e => e.RoleId);
    }
}