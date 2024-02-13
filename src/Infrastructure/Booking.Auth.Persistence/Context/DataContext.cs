using Booking.Auth.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Booking.Auth.Persistence.Context;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
    
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        FillDefaultData(modelBuilder);
    }
    
    private static void FillDefaultData(ModelBuilder builder)
    {
        var rolesWithIds = Application.Common.Roles.GetAllRolesWithIds();
        var superAdminRole = new Role
        {
            Id = rolesWithIds[Application.Common.Roles.SuperAdmin], 
            Name = Application.Common.Roles.SuperAdmin, 
            IsActive = true
        };
        
        builder.Entity<Role>().HasData([
            new Role {Id = rolesWithIds[Application.Common.Roles.Client], Name = Application.Common.Roles.Client, IsActive = true},
            new Role {Id = rolesWithIds[Application.Common.Roles.Admin], Name = Application.Common.Roles.Admin, IsActive = true},
            superAdminRole,
        ]);
        
        builder.Entity<User>().HasData(new User
        {
            Id = Guid.NewGuid(),
            Login = "admin",
            Email = "dadkova-anna@mail.ru",
            PasswordHash = "AQAAAAIAAYagAAAAEHpX9YRUx2LHWG4N5dxWszz3Cgn1mdFl6f5l3slTKrMmqFodCjz7abc564LoKqS98w==", //root
            RefreshToken = "4uvqVaGMGf7VF9Dk6+51kM0LFjEh60ghpgmkBz1qNyU=",
            LastName = "Фамилия",
            FirstName = "Имя",
            MiddleName = "Отчество",
            EmailConfirmed = true,
            PhoneNumber = "5553555",
            RoleId = superAdminRole.Id
        });
    }
}