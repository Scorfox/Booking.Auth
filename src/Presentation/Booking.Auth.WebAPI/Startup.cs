using Booking.Auth.Application;
using Booking.Auth.Application.Consumers.Auth;
using Booking.Auth.Application.Consumers.Company;
using Booking.Auth.Application.Consumers.Filial;
using Booking.Auth.Application.Consumers.User;
using Booking.Auth.Persistence;
using Booking.Auth.WebAPI.Extensions;
using MassTransit;
using Microsoft.OpenApi.Models;

namespace Booking.Auth.WebAPI;

public class Startup
{
    public IConfiguration Configuration { get; }
    
    public Startup(IConfiguration configuration) 
    {
        Configuration = configuration;
    }
    
    public void ConfigureServices(IServiceCollection services)
    {
        services.ConfigurePersistence(Configuration);
        services.ConfigureApplication(Configuration);
        services.AddConfigurations(Configuration);

        services.ConfigureApiBehavior();
        services.ConfigureCorsPolicy();

        services.AddControllers();
        services.AddEndpointsApiExplorer();

        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(Configuration["RabbitMQ:Host"], h =>
                {
                    h.Username(Configuration["RabbitMQ:Username"]);
                    h.Password(Configuration["RabbitMQ:Password"]);
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
    }
    
    public void Configure(WebApplication app, IWebHostEnvironment env) 
    {
        app.UseCors();
        app.MapControllers();
        app.UseAuthentication();
        app.UseAuthorization();
    }
}