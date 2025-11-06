using System.Configuration;
using Microsoft.EntityFrameworkCore;
using rsH60Store.Models;
using rsH60Store.Models.Interfaces;
using rsH60Store.Models.Repositories;
using rsH60Store.Models.Repositories;
using Microsoft.AspNetCore.Identity;
using rsH60Store.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var storeConnectionString = builder.Configuration.GetConnectionString("rsH60StoreContextConnection");
builder.Services.AddDbContext<rsH60StoreContext>(options => options.UseSqlServer(storeConnectionString));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<rsH60StoreContext>();

// Register repositories
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
builder.Services.AddScoped<ProductValidationService>();
builder.Services.AddScoped<ICustomersRepository, CustomersRepository>();
builder.Services.AddScoped<ISoapServiceRepository, SoapServiceRepository>();

builder.Services.AddHttpClient<IProductRepository, ProductRepository>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5219/swagger/index.html");
    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
});

builder.Services.AddHttpClient<IProductCategoryRepository, ProductCategoryRepository>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5219/swagger/index.html");
    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
});

builder.Services.AddHttpClient<ICustomersRepository, CustomersRepository>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5219/swagger/index.html");
    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
});

builder.Services.AddHttpClient<ISoapServiceRepository, SoapServiceRepository>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5219/swagger/index.html");
    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();