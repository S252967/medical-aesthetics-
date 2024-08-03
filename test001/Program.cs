using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using test001.Data;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext and Identity services to the container.
builder.Services.AddDbContext<test001Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("test001Context") ?? throw new InvalidOperationException("Connection string 'test001Context' not found.")));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<test001Context>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
