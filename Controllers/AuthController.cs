using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Helpers;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DotnetAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly DataContextDapper _dapper;

        private readonly AuthHelper _authHelper;

        IUserRepository _userRepository;


        public AuthController(IConfiguration config, IUserRepository userRepository) 
        { 
            _dapper = new DataContextDapper(config);
            _authHelper = new AuthHelper(config);

            _userRepository = userRepository;
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult Register(UserForRegistrationDTO user)
        {
            if(user.Password == user.PasswordConfirm)
            {
                bool userExists = _userRepository.GetSingleUserEmail(user.Email);

                if (userExists)
                {
                    throw new Exception("Email already registered");
                }
                else
                {
                    byte[] passwordSalt = new byte[128 / 8];
                    using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                    {
                        rng.GetNonZeroBytes(passwordSalt);
                    }

                    byte[] passwordHash = _authHelper.GetPasswordHash(user.Password, passwordSalt);


                    string sqlAddAuth = $"INSERT INTO TutorialAppSchema.Auth ([Email], [PasswordHash], [PasswordSalt]) VALUES ('{user.Email}', @PasswordHash, @PasswordSalt)";

                    List<SqlParameter> sqlParams = new List<SqlParameter>();

                    SqlParameter passwordSaltParameter = new SqlParameter("@PasswordSalt", SqlDbType.VarBinary);
                    passwordSaltParameter.Value = passwordSalt;

                    SqlParameter passwordHashParameter = new SqlParameter("@PasswordHash", SqlDbType.VarBinary);
                    passwordHashParameter.Value = passwordHash;

                    sqlParams.Add(passwordSaltParameter);
                    sqlParams.Add(passwordHashParameter);

                    if(_dapper.ExecuteSqlWithParameters(sqlAddAuth, sqlParams))
                    {
                        string sqlAddUser = @"INSERT INTO TutorialAppSchema.Users (
                                [FirstName],
                                [LastName],
                                [Email],
                                [Gender],
                                [Active] 
                            ) VALUES (" +
                                "'" + user.FirstName + "'," +
                                "'" + user.LastName + "'," +
                                "'" + user.Email + "'," +
                                "'" + user.Gender + "'," +
                                "+1)";

                  
                        if (_dapper.ExecuteSql(sqlAddUser))
                        {
                            return Ok();
                        }

                        throw new Exception("Failed to add user");
                    }
                    else
                    {
                        throw new Exception("Failed to add user");
                    }
                }
            }

            throw new Exception("Passwords do not match!");

        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login(UserForLoginDTO user)
        {
            string sqlForHashAndSalt = $"SELECT [PasswordHash],[PasswordSalt] FROM TutorialAppSchema.Auth WHERE Email='{user.Email}'";

            UserForLoginConfirmationDTO userForConfirmation = _dapper.LoadDataSingle<UserForLoginConfirmationDTO>(sqlForHashAndSalt);

            byte[] passwordHash = _authHelper.GetPasswordHash(user.Password, userForConfirmation.PasswordSalt);

            for (int i = 0; i < passwordHash.Length; i++) {
                if(passwordHash[i] != userForConfirmation.PasswordHash[i])
                {
                    return StatusCode(401, "Incorrect Password");
                }
            }

            string userIdSql = $"SELECT UserId FROM TutorialAppSchema.Users WHERE Email = '{user.Email}'";
            int userId = _dapper.LoadDataSingle<int>(userIdSql);

            return Ok(new Dictionary<string, string> {
                {"token", _authHelper.CreateToken(userId)}
            });
        }


        [HttpGet("RefreshToken")]
        public IActionResult RefreshToken()
        {
            string userId = User.FindFirst("userId")?.Value + "";

            string userIdSql = $"SELECT userId FROM TutorialAppSchema.Users WHERE UserId = {userId}";

            int userIdFromDB = _dapper.LoadDataSingle<int>(userIdSql);

            return Ok(new Dictionary<string, string> {
                {"token", _authHelper.CreateToken(userIdFromDB)}
            });
        }
    }
}
