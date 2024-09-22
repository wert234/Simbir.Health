using Account.Application.Commands;
using Account.Application.Handlers;
using Account.Domain.Entitys;
using Account.Infrastructure.Data.DbContexts;
using Account.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
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

builder.Services.AddScoped<IRepository<User, Guid>, AccountRepository>();

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