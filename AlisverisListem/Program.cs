using Microsoft.AspNetCore.Authentication.Cookies;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddMemoryCache();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(a => { a.LoginPath = "/Account/Login"; });
builder.Services.AddSession();
var app = builder.Build();
app.UseAuthentication();
app.UseSession();
app.UseStaticFiles();
app.UseRouting();
app.MapControllerRoute("dflt","{controller=Home}/{action=Listelerim}/{id?}");
app.UseAuthorization();

app.Run();
