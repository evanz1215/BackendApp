using Application;
using Infrastructure;
using WebAPI.Authentication;
using WebAPI.Controllers;
using WebAPI.CORS;
using WebAPI.Middlewares;
using WebAPI.Swagger;
using WebAPI.Versioning;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
IServiceCollection services = builder.Services;
IWebHostEnvironment environment = builder.Environment;
IConfiguration configuration = builder.Configuration;

services.AddMyApi();
services.AddMyApiAuthDeps();
//services.AddMyErrorHandling();
services.AddMySwagger(configuration);
services.AddMyVersioning();
services.AddMyCorsConfiguration(configuration);
services.AddMyInfrastructureDependencies(configuration, environment);
services.AddMyApplicationDependencies();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

//app.UseMyRequestLogging();
app.UseHttpsRedirection();
app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseRouting();
app.UseMyCorsConfiguration();
app.UseMySwagger(configuration);
app.UseMyInfrastructure(configuration, environment);
app.UseMyApi();

app.Run();