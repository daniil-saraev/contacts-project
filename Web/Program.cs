using Web.Extentions;
using Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddApiServices();
builder.Services.AddScoped<ITokenService, AuthorizationCodeTokenService>();
builder.Services.AddHttpClient();

builder.Services.AddAuthenticationConfiguration();
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
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultControllerRoute();
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Account}/{action=Login}");

app.Logger.LogInformation("LAUNCHING");
app.Run();