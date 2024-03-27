using Booking.Auth.Domain.Entities;
using MassTransit.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.ComponentModel.Design;
using System.Net;
using System.Reflection.Emit;
using System.Xml.Linq;

namespace Booking.Auth.Persistence.Context;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<Company> Companies { get; set; } = null!;
    public DbSet<Filial> Filials { get; set; } = null!;

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

            LastName = "Фамилия",
            FirstName = "Имя",
            MiddleName = "Отчество",

            EmailConfirmed = true,
            PhoneNumber = "5553555",
            RoleId = superAdminRole.Id
        });

        var companyId1 = Guid.NewGuid();
        var companyId2 = Guid.NewGuid();

        builder.Entity<Company>().HasData([
        new Company
        {
            Id = companyId1,
            Name = "Чайхана",
            Description = "Ну а как без неё?!",
            Inn = "987654321091",
            MainAddress = "company1@mail.ru",
        },
        new Company
        {
            Id = companyId2,
            Name = "Грустно и не вкусно",
            Description = "Тиньков: Блинб, я заплакал",
            Inn = "987654321092",
            MainAddress = "company2@mail.ru",
        }]);

        builder.Entity<Filial>().HasData([
            new Filial
            {
                Id = Guid.NewGuid(),
                CompanyId = companyId1,
                Name = "Чайхана #1",
                Address = "Ул. Пушкина дом Колотушкина",
                Description = "Спойлер: не #1"
            },
            new Filial
            {
                Id = Guid.NewGuid(),
                CompanyId = companyId2,
                Name = "Грустно и не вкусно",
                Address = "Везде",
                Description = "Description not found ;)"
            }
        ]);
    }
}