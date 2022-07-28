using IdentityAPI.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using IdentityAPI.Extensions;
using Core.Models.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<UserDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("UserDbConnection"));
});

builder.Services.AddIdentityServices();

//builder.Services.ConfigureApplicationCookie(config =>
//{
//    config.LoginPath = "/Account/Login";
//    config.LogoutPath = "/Account/Logout";
//});

builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Policies.RequireAdmin, policy => policy.RequireClaim(ClaimStore.AdminClaim.Type, ClaimStore.AdminClaim.Value));
});

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
app.UseIdentityServer();
app.MapDefaultControllerRoute();

app.Run();
