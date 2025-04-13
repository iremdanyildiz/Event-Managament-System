
using Microsoft.AspNetCore.Mvc;
using EventManagementSystem.Services;
using EventManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;
    private readonly IConfiguration _configuration;

    public UserController(UserService userService, IConfiguration configuration)
    {
        _userService = userService;
        _configuration = configuration;
    }

    // Register metodu, kimlik doğrulaması gerektirmez
    [HttpPost("register")]
    public IActionResult Register([FromBody] User user)
    {
        _userService.RegisterUser(user);
        return Ok(new { message = "User registered successfully" });
    }

    // Login metodu, JWT Token oluşturur
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest loginRequest)
    {
        var user = _userService.LoginUser(loginRequest.Email, loginRequest.Password);
        if (user == null)
        {
            return Unauthorized(new { message = "Invalid credentials" });
        }

        // JWT Token oluştur
        var token = GenerateJwtToken(user);
        return Ok(new { token, message = "Login successful" });
    }

    // Tüm kullanıcıları listeleme (yetkili kullanıcılar)
    [Authorize]
    [HttpGet("all")]
    public IActionResult GetAllUsers()
    {
        var users = _userService.GetAllUsers();
        return Ok(users);
    }

    // ID'ye göre kullanıcı getirme (yetkili kullanıcılar)
    [Authorize]
    [HttpGet("{id}")]
    public IActionResult GetUserById(int id)
    {
        var user = _userService.GetUserById(id);
        if (user == null)
        {
            return NotFound(new { message = "User not found" });
        }
        return Ok(user);
    }

    // Kullanıcı güncelleme (yetkili kullanıcılar)
    [Authorize]
    [HttpPut("update")]
    public IActionResult UpdateUser([FromBody] User user)
    {
        _userService.UpdateUser(user);
        return Ok(new { message = "User updated successfully" });
    }

    // Kullanıcı silme (yetkili kullanıcılar)
    [Authorize]
    [HttpDelete("{id}")]
    public IActionResult DeleteUser(int id)
    {
        _userService.DeleteUser(id);
        return Ok(new { message = "User deleted successfully" });
    }

    // JWT Token oluşturma metodu
    private string GenerateJwtToken(User user)
    {

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
            new Claim(ClaimTypes.Name, user.Name)
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
