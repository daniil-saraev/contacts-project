using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Core.Models.Identity;
using IdentityServer.Extensions;
using IdentityServer.Data;
using IdentityServer.Services;
using IdentityServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Core.Constants;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<UserDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("UserDbConnection"));
});

builder.Services.AddIdentityServices();

builder.Services.AddSingleton<TokenService>();

builder.Configuration.Bind("Auth", new AuthConfiguration());

builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(new AuthConfiguration().Secret)),
        ValidIssuer = BaseUrls.IdentityServerUrl,
        ValidAudience = BaseUrls.ContactsDatabaseAPIUrl,
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Policies.RequireAdmin, policy => policy.RequireClaim(ClaimStore.AdminClaim.Type, ClaimStore.AdminClaim.Value));
});

builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var scopedProvider = scope.ServiceProvider;
    try
    {
        var userManager = scopedProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var dbContext = scopedProvider.GetRequiredService<UserDbContext>();
        await DbInitializer.InitializeAsync(userManager, dbContext);
        app.Logger.LogInformation("DATABASE INITIALIZED");
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "An error occurred initializing the DB.");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
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
app.UseAuthentication();
app.UseAuthorization();
app.MapDefaultControllerRoute();

app.Run();
