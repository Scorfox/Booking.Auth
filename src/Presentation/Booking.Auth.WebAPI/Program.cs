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
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigurePersistence(builder.Configuration);
builder.Services.ConfigureApplication(builder.Configuration);
builder.Services.AddConfigurations(builder.Configuration);

builder.Services.ConfigureApiBehavior();
builder.Services.ConfigureCorsPolicy();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddMassTransit(x =>
{
    // Добавляем шину сообщений

#if dds_tests
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMQdds:Host"], h =>
        {
            h.Username(builder.Configuration["RabbitMQdds:Username"]);
            h.Password(builder.Configuration["RabbitMQdds:Password"]);
        });
        cfg.ConfigureEndpoints(context);
    });
#else
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMQ:Host"], h =>
        {
            h.Username(builder.Configuration["RabbitMQ:Username"]);
            h.Password(builder.Configuration["RabbitMQ:Password"]);
        });
        cfg.ConfigureEndpoints(context);
    });
#endif
    // User
    x.AddConsumer<CreateUserConsumer>();
    x.AddConsumer<UpdateUserConsumer>();
    x.AddConsumer<GetUsersListConsumers>();
    x.AddConsumer<DeleteUserConsumer>();

    // Company
    x.AddConsumer<CreateCompanyConsumer>();
    x.AddConsumer<UpdateCompanyConsumer>();
    x.AddConsumer<GetCompaniesListConsumer>();
    x.AddConsumer<DeleteCompanyConsumer>();
    
    // Filial
    x.AddConsumer<CreateFilialConsumer>();
    x.AddConsumer<UpdateFilialConsumer>();
    x.AddConsumer<DeleteFilialConsumer>();
    x.AddConsumer<GetFilialsListConsumer>();
    
    // Auth
    x.AddConsumer<AuthenticateConsumer>();
});

var app = builder.Build();

var serviceScope = app.Services.CreateScope();
var dataContext = serviceScope.ServiceProvider.GetService<DataContext>();
dataContext?.Database.EnsureCreated();

app.UseSwagger();
app.UseSwaggerUI();
app.UseErrorHandler();
app.UseCors();
app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();
app.Run();