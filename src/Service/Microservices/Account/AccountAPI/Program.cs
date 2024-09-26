using Account.Application.Commands;
using Account.Application.Handlers;
using Account.Application.Queries;
using Account.Application.Validators;
using Account.Domain.Entitys.Account;
using Account.Domain.Entitys.Tokens.Common;
using Account.Domain.Entitys.Tokens.JWT;
using Account.Infrastructure.Data.DbContexts;
using Account.Infrastructure.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Sherad.Application.Behaviors;
using Sherad.Application.Repositories;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.EnableAnnotations();
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "AccountAPI", Version = "v1" });
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

builder.Services.AddDbContext<AccountDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Connection"));
});

builder.Services.AddMediatR(options =>
{
    options.RegisterServicesFromAssemblies(

        typeof(SignUpHandler).Assembly,
        typeof(SignUpCommand).Assembly,

        typeof(SignInHandler).Assembly,
        typeof(SignInCommand).Assembly,

        typeof(SignOutHandler).Assembly,
        typeof(SignOutCommand).Assembly,

        typeof(ValidateHandler).Assembly,
        typeof(ValidateQuery).Assembly,

        typeof(RefreshHandler).Assembly,
        typeof(RefreshCommand).Assembly,

        typeof(MeHandler).Assembly,
        typeof(MeQuery).Assembly,

        typeof(UpdateHandler).Assembly,
        typeof(UpdateCommand).Assembly,

        typeof(CreateAccountHandler).Assembly,
        typeof(CreateAccountCommand).Assembly,

        typeof(UpdateAccountHandler).Assembly,
        typeof(UpdateAccountCommand).Assembly
        );
});

builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddScoped<IRepository<User, Guid>, AccountRepository>();
builder.Services.AddScoped<IValidator<SignUpCommand>, SignUpCommandValidator>();
builder.Services.AddScoped<IValidator<SignInCommand>, SignInCommandValidator>();
builder.Services.AddScoped<IValidator<SignOutCommand>, SignOutCommandValidator>();
builder.Services.AddScoped<IValidator<RefreshCommand>, RefreshCommandValidator>();
builder.Services.AddScoped<IValidator<UpdateCommand>, UpdateCommandValidator>();
builder.Services.AddScoped<IValidator<CreateAccountCommand>, CreateAccountCommandValidator>();
builder.Services.AddScoped<IValidator<UpdateAccountCommand>, UpdateAccountCommandValidator>();
builder.Services.AddScoped<IValidator<MeQuery>, MeQueryValidator>();
builder.Services.AddSingleton<ITokenService, JwtTokenService>();


builder.Services.AddAuthentication(opt => {
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"]!,
            ValidAudience = builder.Configuration["JWT:Audience"]!,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"]!))
        };
    });
builder.Services.AddAuthorization(options => options.DefaultPolicy =
    new AuthorizationPolicyBuilder
            (JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser()
        .Build());

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();