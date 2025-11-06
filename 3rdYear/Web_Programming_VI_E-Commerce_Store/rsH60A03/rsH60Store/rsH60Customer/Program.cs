using AspNetCoreHero.ToastNotification;
using Microsoft.EntityFrameworkCore;
using rsH60Customer.Models;
using rsH60Customer.Models.Interfaces;
using rsH60Customer.Models.Repositories;
using Microsoft.AspNetCore.Identity;
using rsH60Customer.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<rsH60CustomerContext>();

var identity = builder.Configuration.GetConnectionString("rsH60StoreContextConnection");
builder.Services.AddDbContext<rsH60CustomerContext>(options => options.UseSqlServer(identity));

var storeConnectionString = builder.Configuration.GetConnectionString("rsH60StoreContextConnection");
builder.Services.AddDbContext<H60CustomerDBContext>(options => options.UseSqlServer(storeConnectionString));


// Register repositories
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
builder.Services.AddScoped<ProductValidationService>();
builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
builder.Services.AddScoped<ICartItemRepository, CartItemRepositoryRepository>();
builder.Services.AddScoped<ICustomersRepository, CustomersRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
builder.Services.AddScoped<ISoapServiceRepository, SoapServiceRepository>();
builder.Services.AddNotyf(config=>
    {
        config.DurationInSeconds = 10;
        config.IsDismissable = true;
        config.Position = NotyfPosition.BottomRight; }
);

builder.Services.AddHttpClient<IProductRepository, ProductRepository>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5219/api/");
    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
});

builder.Services.AddHttpClient<IProductCategoryRepository, ProductCategoryRepository>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5219/api/");
    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
});

builder.Services.AddHttpClient<IShoppingCartRepository, ShoppingCartRepository>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5219/api/");
});

builder.Services.AddHttpClient<ICartItemRepository, CartItemRepositoryRepository>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5219/api/");
});

builder.Services.AddHttpClient<ICustomersRepository, CustomersRepository>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5219/api/");
});

builder.Services.AddHttpClient<IOrderRepository, OrderRepository>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5219/api/");
});

builder.Services.AddHttpClient<IOrderItemRepository, OrderItemRepository>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5219/api/");
});

builder.Services.AddHttpClient<ISoapServiceRepository, SoapServiceRepository>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5219/api/");
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