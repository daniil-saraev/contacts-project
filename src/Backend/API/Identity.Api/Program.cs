using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Diagnostics;
using System.Reflection;
using Swashbuckle.AspNetCore.SwaggerGen;
using Identity.Api.Services;
using Identity.Api.Extensions;
using Identity.Api.Configuration;
using Identity.Data.Configuration;
using Identity.Data.Context;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<IdentityDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("UserDbConnection"));
});

builder.Services.AddIdentityServices(builder.Configuration);

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

using (var scope = app.Services.CreateScope())
{
    var scopedProvider = scope.ServiceProvider;
    await IdentityDbInitialization.InitializeDbAsync(scopedProvider);
}

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
