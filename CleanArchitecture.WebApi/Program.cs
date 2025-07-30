using CleanArchitecture.Application.Behaviors;
using CleanArchitecture.Application.Services;
using CleanArchitecture.Domain.Repositories;
using CleanArchitecture.Persistance.Context;
using CleanArchitecture.Persistance.Repositories;
using CleanArchitecture.Persistance.Services;
using CleanArchitecture.WebApi.Middleware;
using FluentValidation;
using GenericRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using CleanArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using CleanArchitecture.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);


#region Mail Ayarlarý
var mailSettings = builder.Configuration.GetSection("MailSettings");
string? fromEmail = mailSettings["FromEmail"];
string? fromName = mailSettings["FromName"];
string? smtpHost = mailSettings["SmtpHost"];
int smtpPort = int.Parse(mailSettings["SmtpPort"]);
#endregion


builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IMailService, MailService>();


builder.Services.AddTransient<ExceptionMiddleware>();
builder.Services.AddScoped<IUnitOfWork>(srv => srv.GetRequiredService<AppDbContext>());
builder.Services.AddScoped<ICarRepository, CarRepository>();


builder.Services.AddAutoMapper(cfg =>
{	
	cfg.AddMaps(typeof(CleanArchitecture.Persistance.AssemblyReference).Assembly);
});

string? connectionString = builder.Configuration.GetConnectionString("SqlServer");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 1;
    options.Password.RequireUppercase = false;
}).AddEntityFrameworkStores<AppDbContext>();


builder.Services.AddControllers()
        .AddApplicationPart(typeof(CleanArchitecture.Presentation.AssemblyReference).Assembly);

builder.Services.AddFluentEmail(fromEmail, fromName).AddSmtpSender(smtpHost, smtpPort);

builder.Services.AddMediatR(cfr =>
    cfr.RegisterServicesFromAssembly(typeof(CleanArchitecture.Application.AssemblyReference).Assembly)); //Asýl CQRS Pattern uyguladýðýmýz katmanýn referansýný veririz

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>)); //IPipeLine gördüðümde ValidationBehavior ver

builder.Services.AddValidatorsFromAssembly(typeof(CleanArchitecture.Application.AssemblyReference).Assembly);

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseMiddlewareExtensions();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
