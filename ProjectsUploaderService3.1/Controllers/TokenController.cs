using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectsUploaderService3._1.Contracts;

namespace ProjectsUploaderService3._1.Controllers
{
	[Route("token")]
	[ApiController]
	public class TokenController : ControllerBase
	{
		private readonly ITokenService _tokenService;

		public TokenController(ITokenService tokenService)
		{
			_tokenService = tokenService;
		}

		[HttpPost]
		public async Task<IActionResult> GetTokenAsync(TokenRequest request)
		{
			var result = await _tokenService.GetTokenAsync(request.Password);
			if (!string.IsNullOrEmpty(result))
			{
				return Ok(result);
			}
			return BadRequest();
		}
	}

	public class TokenRequest
	{
		public string Password { get; set; }
	}
}
