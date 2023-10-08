using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SPATIUM_backend.Core;
using SPATIUM_backend.Interfaces;
using SPATIUM_backend.Models;
using SPATIUM_backend.Security;
using SPATIUM_backend.Services.User;
using System.Security.Claims;
using System.Text;

namespace SPATIUM_backend.Extensions;

public static class ServiceExtension
{
	public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration cfg)
	{
		if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
		{
			services.AddDbContext<AppDbContext>(options =>
			{
				options.UseSqlServer(cfg.GetConnectionString("DefaultConnectionProd"));
			});
		}
		else
		{
			services.AddDbContext<AppDbContext>(options =>
			{
				options.UseSqlServer(cfg.GetConnectionString("DefaultConnection"));
			});
		}

		services.BuildServiceProvider().GetService<AppDbContext>()?.Database.Migrate();

		services.AddIdentityCore<User>().AddEntityFrameworkStores<AppDbContext>();

		services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			.AddJwtBearer("Bearer", options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = cfg["Jwt:Issuer"],
					ValidAudience = cfg["Jwt:Issuer"],
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(cfg["Jwt:Key"]!))
				};
			});
		services.AddAuthorization(options =>
		{
			options.FallbackPolicy = new AuthorizationPolicyBuilder()
				.RequireAuthenticatedUser()
				.Build();
		});

		services.AddCors(options =>
		{
			options.AddDefaultPolicy(policy =>
			{
				policy
					.AllowAnyHeader()
					.AllowAnyMethod()
					.AllowAnyOrigin();
			});
		});

		services.AddScoped<ITokenService, TokenService>();
		services.AddScoped<IUserAccessor, UserAccessor>();
		services.AddHttpContextAccessor();
		services.AddMediatR(msc => msc.RegisterServicesFromAssemblyContaining(typeof(LoginCommandHandler)));
		services.AddScoped<MapperlyMapper>();
		services.AddScoped<ClaimsPrincipal>();

		return services;
	}
}