using Microsoft.EntityFrameworkCore;
using IdentityAPI.Data;
using IdentityAPI.Configuration;
using IdentityAPI.Extensions;
using IdentityAPI.Services;
using Microsoft.AspNetCore.Diagnostics;
using System.Reflection;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<UserDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("UserDbConnection"));
});

builder.Services.AddIdentityServices();

builder.Services.AddScoped<ITokenService, TokenService>();

AuthConfiguration configuration = new AuthConfiguration();
builder.Configuration.Bind("JWT", configuration);
builder.Services.AddSingleton(configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var path = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(path);

    options.CustomOperationIds(info =>
    {
        return info.TryGetMethodInfo(out MethodInfo methodInfo) ? methodInfo.Name : null;
    });
});

var app = builder.Build();

await app.UseIdentityInitializationAsync();

app.UseSwagger();
app.UseSwaggerUI();

app.UseExceptionHandler(app => app.Run(async context =>
{
    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
    var exception = exceptionHandlerPathFeature?.Error;
    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
    await context.Response.WriteAsync(exception?.Message);
}));

app.UseHttpsRedirection();
app.UseRouting();

app.MapDefaultControllerRoute();

app.Run();
