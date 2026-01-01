using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Application.DTOs;
using ProjectManagement.Application.Interfaces;
using ProjectManagement.Application.Wrappers;
using ProjectManagement.Domain.Constants;
using ProjectManagement.Domain.Entities;

namespace ProjectManagement.API.Controllers
{
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IEmailService _emailService;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IJwtTokenGenerator jwtTokenGenerator,
            IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenGenerator = jwtTokenGenerator;
            _emailService = emailService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var userExists = await _userManager.FindByEmailAsync(request.Email);
            if (userExists != null)
                return BadRequest(new ApiResponse<string>("User already exists"));

            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return BadRequest(new ApiResponse<string>("Registration failed") { Errors = errors });
            }
            // Assign default role
            await _userManager.AddToRoleAsync(user, Roles.User);

            // Generate and send email confirmation link
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action(nameof(VerifyEmail), "Auth", new { token, email = user.Email }, Request.Scheme);
            var emailBody = $"Please confirm your account by clicking this link: <a href='{confirmationLink}'>Confirm Email</a>";
            await _emailService.SendEmailAsync(user.Email, "Confirm your email", emailBody);

            return Ok(new ApiResponse<string>(user.Email, "User registered successfully! Please check your email."));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return Unauthorized(new ApiResponse<string>("Invalid email or password."));

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!result.Succeeded)
                return Unauthorized(new ApiResponse<string>("Invalid email or password."));

            if (!await _userManager.IsEmailConfirmedAsync(user))
                return Unauthorized(new ApiResponse<string>("Email is not confirmed."));

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtTokenGenerator.GenerateToken(user, roles);

            return Ok(new ApiResponse<object>(new { User = user.Email, Token = token, Roles = roles }, "Login Successful"));
        }

        [HttpGet("verify-email")]
        public async Task<IActionResult> VerifyEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return BadRequest(new ApiResponse<string>("Invalid email confirmation request."));

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
                return Ok(new ApiResponse<string>(user.Email, "Email confirmed successfully!"));

            return BadRequest(new ApiResponse<string>("Email confirmation failed."));
        }
    }
}
