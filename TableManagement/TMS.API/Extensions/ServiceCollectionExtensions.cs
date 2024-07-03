using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;
using TMS.APP.Commands.User.Handlers;
using TMS.APP.Commands.User;
using TMS.APP.Factory;
using TMS.APP.Models;
using TMS.Infrastructure.Configuration;
using TMS.Infrastructure.Entities;
using TMS.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Swashbuckle.AspNetCore.Filters;
using FluentValidation;
using TMS.Infrastructure.Repository;
using TMS.APP.Commands.Table.Create.Handler;
using TMS.APP.Commands.Table.Create;
using TMS.APP.Commands.Table.Delete.Handler;
using TMS.APP.Commands.Table.Delete;
using TMS.APP.Commands.Table.Update.Handler;
using TMS.APP.Commands.Table.Update;
using TMS.APP.Queries.Handlers;
using TMS.APP.Queries;

namespace TMS.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TMSDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("Connection"));
            });

            return services;
        }

        public static IServiceCollection AddCustomIdentity(this IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<TMSDbContext>();

            return services;
        }

        public static IServiceCollection AddCustomJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var signingKey = new SymmetricSecurityKey(
                Encoding.Default.GetBytes(configuration["JwtConfiguration:Secret"]));

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = configuration["JwtConfiguration:Issuer"],

                ValidateAudience = true,
                ValidAudience = configuration["JwtConfiguration:Audience"],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.Configure<JwtConfiguration>(options =>
            {
                options.Issuer = configuration["JwtConfiguration:Issuer"];
                options.Audience = configuration["JwtConfiguration:Audience"];
                options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(configureOptions =>
            {
                configureOptions.ClaimsIssuer = configuration["JwtConfiguration:Issuer"];
                configureOptions.TokenValidationParameters = tokenValidationParameters;
                configureOptions.SaveToken = true;
            });

            return services;
        }

        public static IServiceCollection AddCustomAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            });

            return services;
        }

        public static IServiceCollection AddCustomCors(this IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            return services;
        }

        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<ITableRepository, TableRepository>();
            services.AddTransient<IJwtFactory, JwtFactory>();
            services.AddTransient<IEmailSender<IdentityUser>, NullEmailSender>();

            return services;
        }

        public static IServiceCollection AddCustomValidators(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<RegisterUserCommandValidator>();
            services.AddValidatorsFromAssemblyContaining<LoginUserCommandValidator>();

            return services;
        }

        public static IServiceCollection AddCustomMediatR(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddTransient<IRequestHandler<RegisterUserCommand, ApplicationResult>, RegisterUserCommandHandler>();
            services.AddTransient<IRequestHandler<LoginUserCommand, AuthResultDto>, LoginUserCommandHandler>();

            services.AddTransient<IRequestHandler<CreateTableCommand, bool>, CreateTableCommandHandler>();
            services.AddTransient<IRequestHandler<UpdateTableCommand, bool>, UpdateTableCommandHandler>();
            services.AddTransient<IRequestHandler<DeleteTableCommand, bool>, DeleteTableCommandHandler>();
            services.AddTransient<IRequestHandler<GetAllTablesQuery, List<DynamicTableDto>>, GetAllTablesQueryHandler>();
            services.AddTransient<IRequestHandler<GetTableByIdQuery, DynamicTableDto>, GetTableByIdQueryHandler>();

            return services;
        }

        public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("oauth2", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
                });
                options.OperationFilter<SecurityRequirementsOperationFilter>();
            });

            return services;
        }
    }
}
