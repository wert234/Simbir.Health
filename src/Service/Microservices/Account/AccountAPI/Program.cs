using Account.Application.Commands;
using Account.Application.Handlers;
using Account.Application.Validators;
using Account.Domain.Entitys;
using Account.Domain.Tokens.JWT;
using Account.Infrastructure.Data.DbContexts;
using Account.Infrastructure.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sherad.Application.Behaviors;
using Sherad.Application.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AccountDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Connection"));
});

builder.Services.AddMediatR(options =>
{
    options.RegisterServicesFromAssemblies(

        typeof(SignUpHandler).Assembly,
        typeof(SignUpCommand).Assembly
        );
});

builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Services.AddScoped<IRepository<User, Guid>, AccountRepository>();
builder.Services.AddScoped<IValidator<SignUpCommand>, SignUpCommandValidator>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AccountDbContext>();
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