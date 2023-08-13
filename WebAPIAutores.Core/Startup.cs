using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WebAPIAutores.Core;
using WebAPIAutores.Core.Filters;
using WebAPIAutores.Core.Services;
using WebAPIAutores.Core.Services.Shared;
using WebAPIAutores.Core.Utils;

namespace WebApiAutores.Core
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Conventions.Add(new SwaggerGroupByVersion());
            })
            .AddJsonOptions(option => option.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles)
            .AddNewtonsoftJson();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("defaultConnection")));
            services.AddEndpointsApiExplorer();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["keyJwt"])),
                    ClockSkew = TimeSpan.Zero
                });
            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPIAutores", Version = "v1" });
                swagger.SwaggerDoc("v2", new OpenApiInfo { Title = "WebAPIAutores", Version = "v2" });
                swagger.OperationFilter<AddParameterHATEOAS>();
                swagger.OperationFilter<AddParameterXVersion>();
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                });
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                        },
                        new string[]{}
                    }
                });
            });
            services.AddAutoMapper(typeof(Startup));
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("IsAdmin", policy => policy.RequireClaim("isAdmin"));
            });
            services.AddDataProtection();
            services.AddScoped<IAutorServices, AutorServicesImpl>();
            services.AddScoped<ILibrosServices, LibrosServicesImpl>();
            services.AddScoped<IComentariosServices, ComentariosServicesImpl>();
            services.AddScoped<IAuthServices, AuthServicesImpl>();
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("").AllowAnyMethod().AllowAnyHeader()
                        .WithExposedHeaders(new string[] { "cantidadTotalRegistros" });
                });
            });
            services.AddTransient<GenerateUrlsService>();
            services.AddTransient<HATEOASAutorFilterAttribute>();
            services.AddTransient<IActionContextAccessor, ActionContextAccessor>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPIAutores V1");
                    c.SwaggerEndpoint("/swagger/v2/swagger.json", "WebAPIAutores V2");
                });
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}