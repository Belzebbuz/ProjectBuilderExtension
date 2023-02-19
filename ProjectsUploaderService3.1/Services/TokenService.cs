using System.Security.Cryptography;
using System;
using System.Threading.Tasks;
using ProjectsUploaderService3._1.Contracts;
using ProjectsUploaderService3._1.Services.Settings;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace ProjectsUploaderService3._1.Services
{
	internal class TokenService : ITokenService
	{
		private readonly SecuritySettings _settings;

		public TokenService(SecuritySettings settings)
		{
			_settings = settings;
		}
		public async Task<string> GetTokenAsync(string password)
		{
			var isValid = await VerifyPasswordHashAsync(_settings.PasswordHash, password);
			return !isValid ? string.Empty : GenerateEncryptedToken(GetSigningCredentials());
		}

		private string GenerateEncryptedToken(SigningCredentials credentials)
		{
			var token = new JwtSecurityToken(
				expires: DateTime.UtcNow.AddMinutes(10),
				signingCredentials: credentials);
			var tokenHandler = new JwtSecurityTokenHandler();
			return tokenHandler.WriteToken(token);
		}
		private SigningCredentials GetSigningCredentials()
		{
			if (string.IsNullOrEmpty(_settings.JwtKey))
				throw new InvalidOperationException("No Key defined in JwtSettings config.");

			byte[] secret = Encoding.UTF8.GetBytes(_settings.JwtKey);
			return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
		}
		private async Task<bool> VerifyPasswordHashAsync(string hash, string password)
		{
			byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
			using (var sha = SHA256.Create())
			{
				var passHashBytes = sha.ComputeHash(passwordBytes);
				var passHash = Convert.ToBase64String(passHashBytes);
				if (hash != passHash)
					return false;
			}
			return true;
		}

	}
}
