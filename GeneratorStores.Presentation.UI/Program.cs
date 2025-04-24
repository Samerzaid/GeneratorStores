using GeneratorStores.DataAccess.Entities;
using GeneratorStores.Presentation.UI.Components.Account;
using GeneratorStores.Presentation.UI.Components;
using GeneratorStores.Presentation.UI.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RentConnect.Presentation.UI;
using GeneratorStores.DataAccess.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

// Configure the database connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Configure Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Configure password settings
    var identityOptions = builder.Configuration.GetSection("IdentityOptions:Password");
    options.Password.RequireDigit = identityOptions.GetValue<bool>("RequireDigit");
    options.Password.RequireLowercase = identityOptions.GetValue<bool>("RequireLowercase");
    options.Password.RequireUppercase = identityOptions.GetValue<bool>("RequireUppercase");
    options.Password.RequiredLength = identityOptions.GetValue<int>("RequiredLength");
    options.Password.RequireNonAlphanumeric = identityOptions.GetValue<bool>("RequireNonAlphanumeric");
    options.Password.RequiredUniqueChars = identityOptions.GetValue<int>("RequiredUniqueChars");

    // Sign-in options
    options.SignIn.RequireConfirmedAccount = true;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Configure redirect paths for unauthorized access
builder.Services.ConfigureApplicationCookie(cookieOptions =>
{
    var identityCookieOptions = builder.Configuration.GetSection("IdentityOptions");
    cookieOptions.LoginPath = identityCookieOptions.GetValue<string>("LoginPath");
    cookieOptions.AccessDeniedPath = identityCookieOptions.GetValue<string>("AccessDeniedPath");
});

// Register RoleInitializer as a scoped service
builder.Services.AddScoped<RoleInitializer>();
builder.Services.AddSingleton<ICartService, CartService>();
builder.Services.AddHttpClient< UserService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5013"); // Backend API base URL
});

// Add email sender service
builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

// Register HttpClient for the backend API and ProductService
builder.Services.AddHttpClient<IProductService, ProductService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5013"); // Backend API base URL
});

builder.Services.AddHttpClient<IOrderService, OrderService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5013");
});

builder.Services.AddHttpClient<ICategoryService, CategoryService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5013"); // Backend API base URL
});

builder.Services.AddHttpClient<IReviewService, ReviewService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5013"); // Your backend API URL
});

builder.Services.AddHttpClient<ICheckoutOrderService, CheckoutOrderService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5013"); // Your backend API URL
});

builder.Services.AddHttpClient<IEmailService, EmailService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5013"); // Your backend API URL
});

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5013") });

builder.Services.AddScoped<UserService>();


var app = builder.Build();

// Initialize roles (if needed)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleInitializer = services.GetRequiredService<RoleInitializer>();
    await roleInitializer.SeedRolesAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapAdditionalIdentityEndpoints();

app.Run();

