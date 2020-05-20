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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using tuto3.DAL;
using tuto3.DTOs.Requests;
using tuto3.DTOs.Responses;
using tuto3.Entities;
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
        private readonly StudentContext _studentContext;

        public StudentsController(IDbService dbService, IConfiguration configuration, StudentContext studentContext)
        {
            _dbService = dbService;
            _configuration = configuration;
            _studentContext = studentContext;
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
        //[Authorize(Roles = "student")]
        [AllowAnonymous]
        public IActionResult GetStudents()
        {
            var students = _studentContext.Student.
                Include(s => s.IdEnrollmentNavigation).ThenInclude(s => s.IdStudyNavigation)
                .Select(s => new GetStudentRes
                {
                    IndexNumber = s.IndexNumber,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    BirthDate = s.BirthDate.ToShortDateString(),
                    Semester = s.IdEnrollmentNavigation.Semester,
                    Studies = s.IdEnrollmentNavigation.IdStudyNavigation.Name
                }).ToList();
            //return Ok(_dbService.GetStudents());
            return Ok(students);
        }

        [HttpGet("enrollment/{indexNumber}")]
        public IActionResult GetStudentEnrollment(string indexNumber)
        {
            Models.Enrollment enrollment = _dbService.GetEnrollment(indexNumber);
            if (enrollment == null)
            {
                return NotFound("Student not found");
            }
            return Ok(enrollment);
        }

        [HttpGet("{indexNumber}")]
        public IActionResult GetStudent(string indexNumber)
        {
            Models.Student student = _dbService.GetStudent(indexNumber);
            if (student == null)
            {
                return NotFound("Student not found");
            }
            return Ok(student);
        }

        [HttpPost]
        [AllowAnonymous]
        //public IActionResult CreateStudent(Models.Student student)
        public IActionResult CreateStudent(AddStudentReq student)
        {
            //int inserted = _dbService.InsertStudent(student);
            //return Ok($"Inserted students: {inserted}");
            var studentEntity = new Entities.Student
            {
                IndexNumber = student.IndexNumber,
                FirstName = student.FirstName,
                LastName = student.LastName,
                BirthDate = student.BirthDate,
                IdEnrollment = student.IdEnrollment,
                IdRole = 1,
                Password = "pass",
                Salt = "salt"
            };
            _studentContext.Add(studentEntity);
            _studentContext.SaveChanges();
            return Ok($"Inserted");
        }

        [HttpDelete("{indexNumber}")]
        [AllowAnonymous]
        public IActionResult DeleteStudent(string indexNumber)
        {   /*
            int deleted = _dbService.DeleteStudnet(indexNumber);
            if (deleted == 0)
            {
                return NotFound("Student not found");
            }
            return Ok($"Students deleted: {deleted}");
            */
            var student = (new Entities.Student { IndexNumber = indexNumber });
            _studentContext.Attach(student);
            _studentContext.Remove(student);
            _studentContext.SaveChanges();
            return Ok($"Student deleted");
        }

        [HttpPost("update")]
        [AllowAnonymous]
        public IActionResult UpdateStudent(UpdateStudentReq req)
        {
            var studentEntity = new Entities.Student { IndexNumber = req.IndexNumber };
            _studentContext.Attach(studentEntity);
            if (req.FirstName != null)
            {
                studentEntity.FirstName = req.FirstName;
                _studentContext.Entry(studentEntity).Property("FirstName").IsModified = true;

            }
            if (req.LastName != null)
            {
                studentEntity.LastName = req.LastName;
                _studentContext.Entry(studentEntity).Property("LastName").IsModified = true;
            }
            if (req.BirthDate != null)
            {
                studentEntity.BirthDate = req.BirthDate;
                _studentContext.Entry(studentEntity).Property("BirthDate").IsModified = true;
            }
            if (req.IdEnrollment != null)
            {
                studentEntity.IdEnrollment = Int32.Parse(req.IdEnrollment);
                _studentContext.Entry(studentEntity).Property("IdEnrollment").IsModified = true;
            }

            _studentContext.SaveChanges();
            return Ok($"Updated");
        }

        [HttpPut]
        public IActionResult PutStudent(Models.Student student)
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