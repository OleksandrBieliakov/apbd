using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using tuto3.DAL;
using tuto3.DTOs.Requests;
using tuto3.Models;

namespace tuto3.Controllers
{
    [ApiController]
    [Route("api/students")]
    //apply the basic auth scheme to all endpoints (or apply another explisitly)
    [Authorize]
    public class StudentsController : ControllerBase
    {
        private readonly IDbService _dbService;
        private readonly IConfiguration _configuration;

        public StudentsController(IDbService dbService, IConfiguration configuration)
        {
            _dbService = dbService;
            _configuration = configuration;
        }

        [HttpPost("upgrade-password/{indexNumber}")]
        [AllowAnonymous]
        public IActionResult UpgradePasswords(string indexNumber)
        {
            var credentials = _dbService.GetCredentials(indexNumber);
            if (credentials == null)
            {
                return NotFound("Studnet not found");
            }
            var salt = CreateSalt();
            var saltedPassword = HashPasswordWithSalt(credentials.Password, salt);
            var upgraded = _dbService.UpgradeStudentPassword(new UpgradeStudentPasswordReq
            {
                IndexNumber = indexNumber,
                Password = saltedPassword,
                Salt = salt
            });
            if (upgraded != 1)
            {
                return StatusCode(500, "Something went wrong");
            }
            return Ok("Upgraded");
        }


        [HttpPost("login")]
        //Can exempt an endpoint (or controller) from auth check
        [AllowAnonymous]
        public IActionResult Login(LoginReq loginReq)
        {
            var studentCredentials = _dbService.GetCredentials(loginReq.Login);
            if (studentCredentials == null)
            {
                return NotFound("Student not found");
            }

            if (studentCredentials.Salt == "")
            {
                if (studentCredentials.Password != loginReq.Password)
                {
                    return StatusCode(401, "Invalid password");
                }
            }
            else
            {
                if (!ValidatePassword(loginReq.Password, studentCredentials.Salt, studentCredentials.Password))
                {
                    return StatusCode(401, "Invalid password (salted)");
                }
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, loginReq.Login),
                //new Claim(ClaimTypes.Name, "bob1"),
                new Claim(ClaimTypes.Role, studentCredentials.Role),
                //new Claim(ClaimTypes.Role, "student")
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken
            (
                issuer: "Gakko",
                audience: "Students",
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: credentials
            );

            var newRefreshToken = Guid.NewGuid();
            // put refresh tocken in db
            _dbService.SetRefreshTocken(new SetRefreshTokenReq
            {
                IndexNumber = loginReq.Login,
                RefreshToken = newRefreshToken.ToString()
            });

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                refreshToken = newRefreshToken
            });
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public IActionResult RefreshTocken(RefreshTokenReq req)
        {
            var studentInfo = _dbService.StudentByRefreshToken(req.RefreshToken);
            if (studentInfo == null)
            {
                return StatusCode(401, "Invalid refresh token");
            }


            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, studentInfo.IndexNumber),
                //new Claim(ClaimTypes.Name, "bob1"),
                new Claim(ClaimTypes.Role, studentInfo.Role),
                //new Claim(ClaimTypes.Role, "student")
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken
            (
                issuer: "Gakko",
                audience: "Students",
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: credentials
            );

            var newRefreshToken = Guid.NewGuid();
            // put refresh tocken in db
            var updated = _dbService.SetRefreshTocken(new SetRefreshTokenReq
            {
                IndexNumber = studentInfo.IndexNumber,
                RefreshToken = newRefreshToken.ToString()
            });

            if (updated != 1)
            {
                return StatusCode(500, "Somethong went wrong");
            }

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                refreshToken = newRefreshToken
            });
        }


        [HttpGet]
        //Can specify other authentication scheme for an endpoint (or controller)
        //[Authorize(AuthenticationSchemes = "AuthenticationOther")]

        //Can specify what claims should an authorized user have (here: what role)
        [Authorize(Roles = "student")]
        public IActionResult GetStudents()
        {
            return Ok(_dbService.GetStudents());
        }

        [HttpGet("enrollment/{indexNumber}")]
        public IActionResult GetStudentEnrollment(string indexNumber)
        {
            Enrollment enrollment = _dbService.GetEnrollment(indexNumber);
            if (enrollment == null)
            {
                return NotFound("Student not found");
            }
            return Ok(enrollment);
        }

        [HttpGet("{indexNumber}")]
        public IActionResult GetStudent(string indexNumber)
        {
            Student student = _dbService.GetStudent(indexNumber);
            if (student == null)
            {
                return NotFound("Student not found");
            }
            return Ok(student);
        }

        [HttpPost]
        public IActionResult CreateStudent(Student student)
        {
            int inserted = _dbService.InsertStudent(student);
            return Ok($"Inserted students: {inserted}");
        }

        [HttpDelete("{indexNumber}")]
        public IActionResult DeleteStudent(string indexNumber)
        {
            int deleted = _dbService.DeleteStudnet(indexNumber);
            if (deleted == 0)
            {
                return NotFound("Student not found");
            }
            return Ok($"Students deleted: {deleted}");
        }

        [HttpPut]
        public IActionResult PutStudent(Student student)
        {
            int put = _dbService.InsertOrUpdate(student);
            return Ok($"Inserted or updated students:  {put}");
        }

        private string HashPasswordWithSalt(string password, string salt)
        {
            var valueBytes = KeyDerivation.Pbkdf2(
                    password: password,
                    salt: Encoding.UTF8.GetBytes(salt),
                    prf: KeyDerivationPrf.HMACSHA512,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8
                );
            return Convert.ToBase64String(valueBytes);
        }

        private bool ValidatePassword(string password, string salt, string hash)
            => HashPasswordWithSalt(password, salt) == hash;

        private string CreateSalt()
        {
            byte[] randomBytes = new byte[128 / 8];
            using var generator = RandomNumberGenerator.Create();
            generator.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }

    }
}