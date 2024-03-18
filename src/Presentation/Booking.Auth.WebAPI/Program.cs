//#define dds_tests
using Booking.Auth.Application;
using Booking.Auth.Application.Consumers.Auth;
using Booking.Auth.Application.Consumers.Company;
using Booking.Auth.Application.Consumers.Filial;
using Booking.Auth.Application.Consumers.User;
using Booking.Auth.Persistence;
using Booking.Auth.Persistence.Context;
using Booking.Auth.WebAPI.Extensions;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigurePersistence(builder.Configuration);
builder.Services.ConfigureApplication(builder.Configuration);
builder.Services.AddConfigurations(builder.Configuration);

builder.Services.ConfigureApiBehavior();
builder.Services.ConfigureCorsPolicy();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMQ:Host"], h =>
        {
            h.Username(builder.Configuration["RabbitMQ:Username"]);
            h.Password(builder.Configuration["RabbitMQ:Password"]);
        });
        cfg.ConfigureEndpoints(context);
    });
    
    // User
    x.AddConsumer<CreateUserConsumer>();
    x.AddConsumer<GetUserConsumer>();
    x.AddConsumer<GetUsersListConsumers>();
    x.AddConsumer<UpdateUserConsumer>();
    x.AddConsumer<DeleteUserConsumer>();

    // Company
    x.AddConsumer<CreateCompanyConsumer>();
    x.AddConsumer<GetCompanyConsumer>();
    x.AddConsumer<GetCompaniesListConsumer>();
    x.AddConsumer<UpdateCompanyConsumer>();
    x.AddConsumer<DeleteCompanyConsumer>();
    
    // Filial
    x.AddConsumer<CreateFilialConsumer>();
    x.AddConsumer<GetFilialConsumer>();
    x.AddConsumer<GetFilialsListConsumer>();
    x.AddConsumer<UpdateFilialConsumer>();
    x.AddConsumer<DeleteFilialConsumer>();
    
    // Auth
    x.AddConsumer<AuthenticateConsumer>();
});

var app = builder.Build();

var serviceScope = app.Services.CreateScope();
var dataContext = serviceScope.ServiceProvider.GetService<DataContext>();
dataContext?.Database.EnsureCreated();

app.UseCors();
app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();
app.Run();