using Autofac;
using Autofac.Extensions.DependencyInjection;
using EventMate.API.Filters;
using EventMate.API.Middlewares;
using EventMate.API.Modules;
using EventMate.Core.Repository;
using EventMate.Core.Service;
using EventMate.Core.UnitOfWork;
using EventMate.Repository.Context;
using EventMate.Repository.Repository;
using EventMate.Repository.UnitOfWork;
using EventMate.Service.Mapper;
using EventMate.Service.Service;
using EventMate.Service.Validator.Category;
using EventMate.Service.Validator.City;
using EventMate.Service.Validator.Event;
using EventMate.Service.Validator.Role;
using EventMate.Service.Validator.Ticket;
using EventMate.Service.Validator.User;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

#region Fluent Validation implementation
builder.Services.AddControllers(options => options.Filters.Add(new ValidateFilterAttribute()))
    .AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<CategoryDtoValidator>())
    .AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<CategoryCreateDtoValidator>())
    .AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<CategoryUpdateDtoValidator>())
    .AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<CityDtoValidator>())
    .AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<CityCreateDtoValidator>())
    .AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<CityUpdateDtoValidator>())
    .AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<EventDtoValidator>())
    .AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<EventCreateDtoValidator>())
    .AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<EventUpdateDtoValidator>())
    .AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<RoleDtoValidator>())
    .AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<RoleCreateDtoValidator>())
    .AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<RoleUpdateDtoValidator>())
    .AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<TicketDtoValidator>())
    .AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<TicketCreateDtoValidator>())
    .AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<TicketUpdateDtoValidator>())
    .AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<UserDtoValidator>())
    .AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<UserCreateDtoValidator>())
    .AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<UserUpdateDtoValidator>());
#endregion

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Autofac Definition Sources
//builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
//builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
//builder.Services.AddScoped(typeof(ICategoryRepository), typeof(CategoryRepository));
//builder.Services.AddScoped(typeof(ICityRepository), typeof(CityRepository));
//builder.Services.AddScoped(typeof(IEventRepository), typeof(EventRepository));
//builder.Services.AddScoped(typeof(IRoleRepository), typeof(RoleRepository));
//builder.Services.AddScoped(typeof(ITicketRepository), typeof(TicketRepository));
//builder.Services.AddScoped(typeof(IUserRepository), typeof(UserRepository));

//builder.Services.AddScoped(typeof(IGenericService<>), typeof(GenericServcie<>));
//builder.Services.AddScoped(typeof(ICategoryService), typeof(CategoryService));
#endregion

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddDbContext<ApplicationDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("MsSql_EventMate_DB"), option =>
    {
        option.MigrationsAssembly(Assembly.GetAssembly(typeof(ApplicationDbContext)).GetName().Name);
    });
});

builder.Host.UseServiceProviderFactory
    (new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => containerBuilder.RegisterModule(new RepositoryAndServiceModule()));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCustomException();

app.UseAuthorization();

app.MapControllers();

app.Run();
