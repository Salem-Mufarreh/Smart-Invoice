using Google.Apis.Auth.OAuth2;
using Google.Cloud.DocumentAI.V1;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Host.Mef;
using Microsoft.EntityFrameworkCore;
using Smart_Invoice.Data;
using Smart_Invoice.Utility;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();
builder.Services.Configure<ExchangeRateAPi>(builder.Configuration.GetSection("ExchangeRatesAPI"));

builder.Services.AddHttpClient();
builder.Services.AddSession();
builder.Services.AddDistributedMemoryCache();
builder.Services.Configure<OpenAiSettings>(builder.Configuration.GetSection("OpenAi"));
builder.Services.Configure<ExchangeRateAPi>(builder.Configuration.GetSection("ExchangeRatesAPI"));



var app = builder.Build();
/* Document AI */


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();



app.UseRouting();
app.UseSession();
app.MapRazorPages();
app.UseAuthorization();


app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
    endpoints.MapControllerRoute(
      name: "default",
      pattern: "{controller=Home}/{action=Index}/{id?}"
    );
});

using (var scope = app.Services.CreateScope()) 
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var roles = new[] { SD.Role_Admin, SD.Role_Accountant };

    foreach (var item in roles)
    {
        if(! await roleManager.RoleExistsAsync(item))
        {
            await roleManager.CreateAsync(new IdentityRole(item));
        }
    }
}

app.Run();