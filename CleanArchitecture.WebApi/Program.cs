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

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddTransient<ExceptionMiddleware>();
builder.Services.AddScoped<IUnitOfWork>(srv => srv.GetRequiredService<AppDbContext>());
builder.Services.AddAutoMapper(typeof(CleanArchitecture.Persistance.AssemblyReference).Assembly); //AutoMApper kurduktan sonra belirttik

string? connectionString = builder.Configuration.GetConnectionString("SqlServer");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddScoped<ICarRepository, CarRepository>();

builder.Services.AddControllers()
        .AddApplicationPart(typeof(CleanArchitecture.Presentation.AssemblyReference).Assembly);


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
