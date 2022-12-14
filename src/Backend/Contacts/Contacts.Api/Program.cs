using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Diagnostics;
using Contacts.Common.Configuration;
using Core.Contacts.Exceptions;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddConsole();

builder.Services.RegisterContactsServices(builder.Configuration);

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"])),
        ValidateIssuerSigningKey = true,
        ValidateIssuer = false,
        ValidateAudience = false,        
    };
});
builder.Services.AddAuthorization();

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
    options.OperationFilter<SecurityRequirementsOperationFilter>();
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var scopedProvider = scope.ServiceProvider;
    await ContactsServicesConfiguration.InitializeDbAsync(scopedProvider);
}

app.UseSwagger();
app.UseSwaggerUI(c => c.DisplayOperationId());

app.UseExceptionHandler(app => app.Run(async context =>
{
    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
    var exception = exceptionHandlerPathFeature?.Error;
    if(exception is ContactNotFoundException)
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
    if(exception is UnauthorizedException)
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
    else
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
    await context.Response.WriteAsync(exception?.Message);
}));

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
