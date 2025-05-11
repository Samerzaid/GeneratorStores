using GeneratorStore.DataAccess.Interfaces;
using GeneratorStores.DataAccess;
using GeneratorStores.DataAccess.Interfaces;
using GeneratorStores.DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using GeneratorStores.DataAccess.Entities;
using GeneratorStores.DataAccess.Services;
using GeneratorStores.API.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Use ReferenceHandler.IgnoreCycles to avoid $id and $values
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true; // Optional: Pretty-print JSON output
    });

// Configure Entity Framework Core with SQL Server
builder.Services.AddDbContext<GeneratorDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Identity services for ApplicationUser and IdentityRole
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<GeneratorDbContext>()
    .AddDefaultTokenProviders();

// Register UnitOfWork and Repositories
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IProductOrderRepository, ProductOrderRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryProductRepository, CategoryProductRepository>(); // Add CategoryProductRepository
builder.Services.AddScoped<IEmailSender<ApplicationUser>, EmailSender>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ChatGptService>();


builder.Services.Configure<PayPalSettings>(
    builder.Configuration.GetSection("PayPal"));

    builder.Services.Configure<IdentityOptions>(options =>
{
    options.SignIn.RequireConfirmedEmail = true;
});

  

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();



