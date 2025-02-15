using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OFinance.API.Services;
using static OFinance.API.Models.AuthModel;

namespace OFinance.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly JwtService _jwtService;

        public AuthController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            JwtService jwtService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
        {
            var user = new IdentityUser { UserName = request.Username, Email = request.Email };
            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return BadRequest(new ErrorResponse(
                    "Registration failed",
                    string.Join(", ", result.Errors.Select(e => e.Description))
                ));
            }

            var token = _jwtService.GenerateToken(user.UserName!);
            return new AuthResponse(token, user.UserName!);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null)
            {
                return Unauthorized(new ErrorResponse("Invalid username or password"));
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!result.Succeeded)
            {
                return Unauthorized(new ErrorResponse("Invalid username or password"));
            }

            // Generate JWT token
            var token = _jwtService.GenerateToken(user.UserName!);

            // Set cookie for additional auth method
            Response.Cookies.Append("X-Access-Token", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)
            });

            return new AuthResponse(token, user.UserName!);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            Response.Cookies.Delete("X-Access-Token");
            return Ok();
        }
    }
}
