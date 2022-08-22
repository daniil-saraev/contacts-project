using Web.Extentions;
using Core.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Net;
using Web.Configuration;
using DatabaseApi;
using IdentityApi;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddConsole();
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddApiServices();
builder.Services.AddSession();
builder.Services.AddDistributedMemoryCache();

AuthConfiguration configuration = new AuthConfiguration();
builder.Configuration.Bind("JWT", configuration);
builder.Services.AddSingleton(configuration);

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
}).AddCookie();

builder.Services.AddAuthorization();

var app = builder.Build();

app.Logger.LogInformation("App created...");

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

app.UseToken(app.Services.GetServices<IApiService>());
app.UseAuthentication();
app.UseAuthorization();
app.UseRefreshToken(app.Services.GetRequiredService<IdentityApiService>(), configuration);

app.MapDefaultControllerRoute();

app.Logger.LogInformation("LAUNCHING");
app.Run();