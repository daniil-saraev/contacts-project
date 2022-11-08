using Contacts.Data.Configuration;
using Identity.Data.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddConsole();
builder.Services.AddMvc();

builder.Services.AddIdentityServices(builder.Configuration);

builder.Services.AddCoreServices(builder.Configuration);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// using (var scope = app.Services.CreateScope())
// {
//     var scopedProvider = scope.ServiceProvider;
//     await IdentityDbInitialization.InitializeDbAsync(scopedProvider);
//     await ContactsDbConfiguration.InitializeDbAsync(scopedProvider);
// }

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

app.MapDefaultControllerRoute();
app.MapRazorPages();

app.Run();