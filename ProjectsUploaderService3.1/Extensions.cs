using System;
using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ProjectsUploaderService3._1.Services.Settings;

namespace ProjectsUploaderService3._1
{
	public static class Extensions
	{
		public static IServiceCollection AddAuth(this IServiceCollection services, SecuritySettings settings)
		{
			byte[] key = Encoding.ASCII.GetBytes(settings.JwtKey);
			services.AddAuthentication(authentication =>
			{
				authentication.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				authentication.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(bearer =>
			{
				bearer.RequireHttpsMetadata = false;
				bearer.SaveToken = true;
				bearer.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = false,
					ValidateLifetime = true,
					ValidateAudience = false,
					RoleClaimType = ClaimTypes.Role,
					ClockSkew = TimeSpan.Zero
				};
				bearer.Events = new JwtBearerEvents
				{
					OnChallenge = context =>
					{
						context.HandleResponse();
						if (!context.Response.HasStarted)
						{
							throw new Exception("Authentication Failed.");
						}

						return Task.CompletedTask;
					},
					OnForbidden = _ => throw new Exception("You are not authorized to access this resource.")
				};
			});
			return services;
		}
	}
}