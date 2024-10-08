using Sherad.Application.Common;
using Hospital.Application.Handlers;
using Hospital.Application.Queries;
using Hospital.Infastructure.Data.DbContexts;
using Hospital.Infastructure.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Sherad.Application.Repositories;
using Microsoft.Extensions.DependencyInjection;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using MassTransit;
using FluentValidation;
using Hospital.Application.Validators;
using MediatR;
using Sherad.Application.Behaviors;
using Hospital.Application.Commands;
using HospitalAPI.Consumers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.EnableAnnotations();
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "HospetalsAPI", Version = "v1" });
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

builder.Services.AddDbContext<HospitalDbContext>(options => {
    options.UseNpgsql(builder.Configuration.GetConnectionString("Connection"));
});

builder.Services.AddAuthentication("RabbitMQ")
    .AddScheme<AuthenticationSchemeOptions, RabbitMqJwtAuthenticationHandler>("RabbitMQ", null);

builder.Services.AddMediatR(options =>
{
    options.RegisterServicesFromAssemblies(

        typeof(GetHospitalsHandler).Assembly,
        typeof(GetHospitalsQuery).Assembly,

        typeof(GetRoomsHandler).Assembly,
        typeof(GetRoomsQuery).Assembly,

        typeof(AddHospitalHandler).Assembly,
        typeof(AddHospitalCommand).Assembly,

        typeof(UpdateHospitalHandler).Assembly,
        typeof(UpdateHospitalCommand).Assembly,

        typeof(DeleteHospitalHandler).Assembly,
        typeof(DeleteHospitalCommand).Assembly
        );
});

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<HospitalConsumer>();
    x.AddConsumer<RoomConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq", h =>
        {
            h.Username("Admin");
            h.Password("Frostwert234Z");
        });

        cfg.ReceiveEndpoint("hospital_queue", options =>
        {
            options.ConfigureConsumer<HospitalConsumer>(context);
        });

        cfg.ReceiveEndpoint("room_queue", options =>
        {
            options.ConfigureConsumer<RoomConsumer>(context);
        });
    });
});

builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddScoped<IRepository<Hospital.Domain.Entitys.Hospital, int>, HospitalRepository>();
builder.Services.AddScoped<IValidator<GetHospitalQuery>, GetHospitalQueryValidator>();
builder.Services.AddScoped<IValidator<AddHospitalCommand>, AddHospitalCommandValidator>();
builder.Services.AddScoped<IValidator<UpdateHospitalCommand>, UpdateHospitalCommandValidator>();
builder.Services.AddScoped<IValidator<DeleteHospitalCommand>, DeleteHospitalCommandValidator>();
builder.Services.AddScoped<IValidator<GetRoomsQuery>, GetRoomsQueryValidator>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<HospitalDbContext>();
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
