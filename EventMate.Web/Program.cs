using Autofac.Extensions.DependencyInjection;
using Autofac;
using EventMate.Web.Modules;
using EventMate.Repository.Context;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using EventMate.Service.Mapper;
using Autofac.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddDbContext<ApplicationDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("MsSql_EventMate_DB"), option =>
    {
        option.MigrationsAssembly(Assembly.GetAssembly(typeof(ApplicationDbContext)).GetName().Name);
    });
});
builder.Services.AddHttpContextAccessor();
builder.Host.UseServiceProviderFactory
    (new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => containerBuilder.RegisterModule(new RepositoryAndServiceModule()));

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
