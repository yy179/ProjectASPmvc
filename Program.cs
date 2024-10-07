using ProjectLibrary.Services.Interfaces;
using ProjectLibrary.Services;
using ProjectLibrary.Repositories.Interfaces;
using ProjectLibrary.Repositories;
using ProjectLibrary;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ProjectDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IRequestRepository, RequestRepository>();
builder.Services.AddScoped<IVolunteersRepository, VolunteersRepository>();
builder.Services.AddScoped<IOrganizationsRepository, OrganizationsRepository>();
builder.Services.AddScoped<IMilitaryUnitRepository, MilitaryUnitRepository>();
builder.Services.AddScoped<IContactPersonRepository, ContactPersonRepository>();

builder.Services.AddScoped<IRequestService, RequestService>();
builder.Services.AddScoped<IVolunteerService, VolunteerService>();
builder.Services.AddScoped<IOrganizationService, OrganizationService>();
builder.Services.AddScoped<IMilitaryUnitService, MilitaryUnitService>();
builder.Services.AddScoped<IContactPersonService, ContactPersonService>();

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
