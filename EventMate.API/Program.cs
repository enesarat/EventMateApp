using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using EventMate.API.Filters;
using EventMate.API.Middlewares;
using EventMate.API.Modules;
using EventMate.Core.DTO.Concrete.Category;
using EventMate.Core.DTO.Concrete.City;
using EventMate.Core.DTO.Concrete.Event;
using EventMate.Core.DTO.Concrete.Role;
using EventMate.Core.DTO.Concrete.Ticket;
using EventMate.Core.DTO.Concrete.User;
using EventMate.Core.Model.Concrete;
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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

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
    .AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<UserUpdateDtoValidator>())
    .AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<UserUpdateAsAdminDtoValidator>());
#endregion

builder.Services.AddHttpContextAccessor();


builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "1.0.0",
        Title = "EventMate RESTful API",
        Contact = new OpenApiContact
        {
            Name = "Enes Arat",
            Url = new Uri("https://github.com/enesarat"),
            Email = "enes_arat@outlook.com"
        },
    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

#region CreatedDate Filter Definitions
builder.Services.AddScoped<CreateDateSafetyFilter<Category, CategoryUpdateDto>>();
builder.Services.AddScoped<CreateDateSafetyFilter<City, CityUpdateDto>>();
builder.Services.AddScoped<CreateDateSafetyFilter<Role, RoleUpdateDto>>();
builder.Services.AddScoped<CreateDateSafetyFilter<User, UserUpdateDto>>();
builder.Services.AddScoped<CreateDateSafetyFilter<User, UserUpdateAsAdminDto>>();
builder.Services.AddScoped<CreateDateSafetyFilter<Event, EventUpdateDto>>();
builder.Services.AddScoped<CreateDateSafetyFilter<Ticket, TicketUpdateDto>>();
#endregion

#region Update TicketNum Safety Filter Definitions
builder.Services.AddScoped<UpdateTicketNumSafetyFilter<Ticket, TicketUpdateDto>>();
#endregion

#region StartDate Filter Definitions
builder.Services.AddScoped<StartDateSafetyFilter<Event, EventUpdateDto>>();
#endregion

#region EndDate Filter Definitions
builder.Services.AddScoped<EndDateSafetyFilter<Event, EventUpdateDto>>();
#endregion

#region Update Name Safety Filter Definitions
builder.Services.AddScoped<UpdateEventNameSafetyFilter<Event, EventUpdateDto>>();
#endregion

#region Update Desc Safety Filter Definitions
builder.Services.AddScoped<UpdateEventDescSafetyFilter<Event, EventUpdateDto>>();
#endregion

#region Update Desc Safety Filter Definitions
builder.Services.AddScoped<UpdateCityIdSafetyFilter<Event, EventUpdateDto>>();
#endregion

#region Update Desc Safety Filter Definitions
builder.Services.AddScoped<UpdateCategoryIdSafetyFilter<Event, EventUpdateDto>>();
#endregion

#region Update Approved Safety Filter Definitions
builder.Services.AddScoped<UpdateIsApprSafetyFiltercs<Event, EventUpdateDto>>();
#endregion

#region Event Start Date (before delete) Safety Filter Definitions
builder.Services.AddScoped<EventStartDateFilter>();
#endregion

#region CreatedBy Filter Definitions
builder.Services.AddScoped<CreatedBySafetyFilter<Category, CategoryUpdateDto>>();
builder.Services.AddScoped<CreatedBySafetyFilter<City, CityUpdateDto>>();
builder.Services.AddScoped<CreatedBySafetyFilter<Role, RoleUpdateDto>>();
builder.Services.AddScoped<CreatedBySafetyFilter<User, UserUpdateDto>>();
builder.Services.AddScoped<CreatedBySafetyFilter<User, UserUpdateAsAdminDto>>();
builder.Services.AddScoped<CreatedBySafetyFilter<Event, EventUpdateDto>>();
builder.Services.AddScoped<CreatedBySafetyFilter<Ticket, TicketUpdateDto>>();
#endregion

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

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseCustomException();

app.MapControllers();

app.Run();
