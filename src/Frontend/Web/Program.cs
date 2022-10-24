using Web.Extentions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Net;
using ApiServices.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Web.Configuration;
using Web.Middleware;
using ApiServices.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddConsole();
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddSession();
builder.Services.AddDistributedMemoryCache();

AuthConfiguration configuration = new AuthConfiguration();
builder.Configuration.Bind("JWT", configuration);
builder.Services.AddSingleton(configuration);
builder.Services.AddSingleton<TokenValidator>();

builder.Services.AddApiServices();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.Secret)),
        ValidIssuer = configuration.Issuer,
        ValidAudience = configuration.Audience,
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseStatusCodePages(async context => 
{
    var request = context.HttpContext.Request;
    var response = context.HttpContext.Response;

    if (response.StatusCode == (int)HttpStatusCode.Unauthorized)
    {
        response.Redirect("/account/login");
    }
});

app.UseSession();

app.UseMiddleware<TokenMiddleware>(app.Services.GetRequiredService<ITokenValidator>(), app.Services.GetRequiredService<IIdentityApi>());
app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultControllerRoute();

app.Run();