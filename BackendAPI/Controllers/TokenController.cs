using BackendAPI.Interface;
using BackendAPI.Models;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BackendAPI.Controllers
{
    [Route("api/token")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly DatabaseContext _context;

        private readonly IUserInfoRepository _userInfoRepository;

        public TokenController(IConfiguration config, DatabaseContext context, IUserInfoRepository userInfoRepository)
        {
            _configuration = config;
            _context = context;
            _userInfoRepository = userInfoRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Post(UserInfo _userData)
        {
            if (_userData != null && _userData.Email != null && _userData.Password != null)
            {
                var user = await GetUser(_userData.Email, _userData.Password);

                if (user != null)
                {
                    // Buat JWT berdasarkan user info
                    var token = GenerateJwtToken(user);
                    return Ok(token);
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("google")]
        public async Task<IActionResult> AddGoogleUser([FromBody] UserInfo userInfo)
        {
            try
            {
                // Cek apakah user sudah ada berdasarkan email
                var existingUser = await _userInfoRepository.GetUserByEmailAsync(userInfo.Email);
                if (existingUser != null)
                {
                    // Jika user sudah ada, buatkan JWT token dan kembalikan ke FE
                    var existingUserToken = GenerateJwtToken(existingUser);
                    return Ok(new
                    {
                        message = "User already exists",
                        token = existingUserToken,
                        user = existingUser
                    });
                }

                // Tambahkan user baru
                var newUser = await _userInfoRepository.AddUserAsync(userInfo);

                // Generate JWT token untuk user baru
                var newUserToken = GenerateJwtToken(newUser);

                // Kembalikan respons dengan token dan data user
                return Ok(new
                {
                    message = "User added successfully",
                    token = newUserToken,
                    user = newUser
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding user: {ex.Message}");
                return BadRequest(new { error = ex.Message });
            }
        }


        /*[HttpPost("google")]
        public async Task<IActionResult> AddGoogleUser([FromBody] UserInfo userInfo)
        {
            try
            {
                var existingUser = await _userInfoRepository.GetUserByEmailAsync(userInfo.Email);
                if (existingUser != null)
                {
                    return Ok(new { message = "User already exists", user = existingUser });
                }

                var newUser = await _userInfoRepository.AddUserAsync(userInfo);
                return Ok(new { message = "User added successfully", user = newUser });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding user: {ex.Message}");
                return BadRequest(new { error = ex.Message });
            }
        }*/


        /*[HttpPost("google")]
        public async Task<IActionResult> VerifyGoogleToken([FromBody] GoogleTokenRequest request)
        {
            try
            {
                string idToken = request.idToken;

                // Verifikasi token Google
                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[]
                    {
                        "707250141496-m9guu7jbsj34qq5tqrpk8h97ep98pgil.apps.googleusercontent.com", // Web
                        "707250141496-dg9l728g8ds4kmo9uqeo3idiks59ovvv.apps.googleusercontent.com" // Android            
                    }
                });
                Console.WriteLine($"Payload valid: {JsonConvert.SerializeObject(payload)}");

                // Cek apakah email diverifikasi
                if (!payload.EmailVerified)
                {
                    return BadRequest(new { error = "Email not verified" });
                }

                // Cari atau buat user berdasarkan informasi Google
                var user = await _context.UserInfos.FirstOrDefaultAsync(u => u.Email == payload.Email);
                if (user == null)
                {
                    user = new UserInfo
                    {
                        DisplayName = payload.Name,
                        Email = payload.Email,
                        UserName = payload.Email.Split('@')[0],
                        CreatedDate = DateTime.UtcNow
                    };
                    _context.UserInfos.Add(user);
                    await _context.SaveChangesAsync();
                }

                // Buat JWT internal
                var token = GenerateJwtToken(user);
                return Ok(new { jwt = token });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Google token validation failed: {ex.Message}");
                return BadRequest(new { error = ex.Message });
            }
        }*/


        /*[HttpPost("google")]
        public async Task<IActionResult> VerifyGoogleToken([FromBody] string idToken)
        {
            try
            {
                // Verifikasi token Google
                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { "350984358500-680cbo02sgqp5t4516gv4r8u7plgrd02.apps.googleusercontent.com" }
                });
                Console.WriteLine($"Payload valid: {JsonConvert.SerializeObject(payload)}");

                // Cari atau buat user berdasarkan informasi dari Google
                var user = await _context.UserInfos.FirstOrDefaultAsync(u => u.Email == payload.Email);
                if (user == null)
                {
                    user = new UserInfo
                    {
                        DisplayName = payload.Name,
                        Email = payload.Email,
                        UserName = payload.Email.Split('@')[0],
                        CreatedDate = DateTime.UtcNow
                    };
                    _context.UserInfos.Add(user);
                    await _context.SaveChangesAsync();
                }

                // Buat JWT internal untuk aplikasi
                var token = GenerateJwtToken(user);
                return Ok(new { jwt = token });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Google token validation failed: {ex.Message}");
                return BadRequest(new { error = ex.Message });
            }
        }*/

        private string GenerateJwtToken(UserInfo user)
        {
            // Buat detail claims berdasarkan user info
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("UserId", user.UserId.ToString()),
                new Claim("DisplayName", user.DisplayName ?? ""),
                new Claim("UserName", user.UserName ?? ""),
                new Claim("Email", user.Email ?? "")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(60), // Set JWT kedaluwarsa 60 menit
                signingCredentials: signIn);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<UserInfo> GetUser(string email, string password)
        {
            return await _context.UserInfos.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
        }

        // Endpoint untuk mendapatkan semua pengguna
        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userInfoRepository.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = $"Failed to retrieve users: {ex.Message}" });
            }
        }

        // Endpoint untuk mendapatkan pengguna berdasarkan email
        [HttpGet("{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            try
            {
                var user = await _userInfoRepository.GetUserByEmailAsync(email);
                if (user == null)
                {
                    return NotFound(new { message = "User not found" });
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = $"Failed to retrieve user: {ex.Message}" });
            }
        }
    }
}
