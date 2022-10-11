using System.Reflection;
using HealthyTasty.Domain;
using HealthyTasty.Domain.Tables;
using HealthyTasty.Dto.AutoMapper;
using HealthyTasty.Infrastructure.Aws;
using HealthyTasty.Infrastructure.EntityFramework;
using HealthyTasty.Infrastructure.Exceptions;
using HealthyTasty.Infrastructure.Jwt;
using HealthyTasty.Repositories;
using HealthyTasty.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOptions();

builder.Services.AddJwt(builder.Configuration
    .GetSection(JwtExtension.SectionName).Get<JwtOptions>());

builder.Services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddPostgres<HealthyTastyContext>(builder.Configuration
    .GetSection(PostgresExtension.SectionName).Get<PostgresConfig>());

builder.Services.AddAws(builder.Configuration
    .GetSection(AwsExtension.SectionName).Get<AwsOptions>());

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
builder.Services.AddRepositories();
builder.Services.AddServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseStatusCodePages();

app.UseEndpoints(x => x.MapControllers());

app.MapControllers();

app.Run();
