using Hospital.Application.Handlers;
using Hospital.Application.Queries;
using Hospital.Infastructure.Data.DbContexts;
using Hospital.Infastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Sherad.Application.Repositories;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<HospitalDbContext>(options => {
    options.UseNpgsql(builder.Configuration.GetConnectionString("Connection"));
});

builder.Services.AddMediatR(options =>
{
    options.RegisterServicesFromAssemblies(

        typeof(GetHospitalsHandler).Assembly,
        typeof(GetHospitalsQuery).Assembly
        );
});

builder.Services.AddScoped<IRepository<Hospital.Domain.Entitys.Hospital, Guid>, HospitalRepository>();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
