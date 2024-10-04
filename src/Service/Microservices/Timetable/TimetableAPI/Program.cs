using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Sherad.Application.Behaviors;
using Sherad.Application.Common;
using Sherad.Application.Repositories;
using System.Text.Json.Serialization;
using Timetable.Application.Commands;
using Timetable.Application.Handlers;
using Timetable.Application.Validators;
using Timetable.Domain.Entitys;
using Timetable.Infastructure.Data.DbContexts;
using Timetable.Infastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.EnableAnnotations();
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "TimetableAPI", Version = "v1" });
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

builder.Services.AddMediatR(options =>
{
    options.RegisterServicesFromAssemblies(

        typeof(AddTimetableHandler).Assembly,
        typeof(AddTimetableCommand).Assembly,

        typeof(UpdateTimetableHandler).Assembly,
        typeof(UpdateTimetableCommand).Assembly,

        typeof(DeleteTimetableHandler).Assembly,
        typeof(DeleteTimetableCommand).Assembly

        );
});

builder.Services.AddAuthentication("RabbitMQ")
    .AddScheme<AuthenticationSchemeOptions, RabbitMqJwtAuthenticationHandler>("RabbitMQ", null);


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

builder.Services.AddDbContext<TimetableDbContext>(options => {
    options.UseNpgsql(builder.Configuration.GetConnectionString("Connection"));
});

builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddScoped<IRepository<Timetable.Domain.Entitys.Timetable, int>, TimetableRepository>();
builder.Services.AddScoped<IValidator<AddTimetableCommand>, AddTimetableCommandValidator>();
builder.Services.AddScoped<IValidator<UpdateTimetableCommand>, UpdateTimetableCommandValidator>();
builder.Services.AddScoped<IValidator<DeleteTimetableCommand>, DeleteTimetableCommandValidator>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<TimetableDbContext>();
    dbContext.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
