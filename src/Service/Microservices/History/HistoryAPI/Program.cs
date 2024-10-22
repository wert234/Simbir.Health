using FluentValidation;
using History.Application.Commands;
using History.Application.Handlers;
using History.Application.Queries;
using History.Application.Validators;
using History.Infastructure.Data.DbContexts;
using History.Infastructure.Repositories;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Sherad.Application.Behaviors;
using Sherad.Application.Common;
using Sherad.Application.Repositories;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.EnableAnnotations();
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "HistoryAPI", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{}
        }
    });
});
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq", h =>
        {
            h.Username("Admin");
            h.Password("Frostwert234Z");
        });
    });
});
builder.Services.AddMediatR(option =>
{
    option.RegisterServicesFromAssemblies
    (
        typeof(GetHistoryQueró).Assembly,
        typeof(GetHistoryHandler).Assembly,

        typeof(GetDetailHistoryQueró).Assembly,
        typeof(GetDetailHistoryHandler).Assembly,

        typeof(AddHistoryCommand).Assembly,
        typeof(AddHistoryHandler).Assembly
    );
});

builder.Services.AddDbContext<HistoryDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Connection"));
});

builder.Services.AddAuthentication("RabbitMQ")
    .AddScheme<AuthenticationSchemeOptions, RabbitMqJwtAuthenticationHandler>("RabbitMQ", null);

builder.Services.AddScoped<IRepository<History.Domain.Entitys.History, int>, HistoryRepository>();
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddScoped<IValidator<GetHistoryQueró>, GetHistoryQueróValidator>();
builder.Services.AddScoped<IValidator<GetDetailHistoryQueró>, GetDetailHistoryQueróValidator>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<HistoryDbContext>();
    dbContext.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();