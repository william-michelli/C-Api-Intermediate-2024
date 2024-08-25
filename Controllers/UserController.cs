using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;

namespace DotnetAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        DataContextDapper _dapper;

        public UserController(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
        }

        #region User

        [HttpGet("TestConnection")]
        public DateTime TestConnection()
        {
            return _dapper.LoadDataSingle<DateTime>("SELECT GETDATE()");
        }


        [HttpGet("GetUsers")]
        public IEnumerable<User> GetUsers()
        {
            string sql = @"SELECT * FROM TutorialAppSchema.Users";
            IEnumerable<User> users = _dapper.LoadData<User>(sql); ;
            return users;
        }

        [HttpGet("GetSingleUser/{id}")]
        public User GetSingleUser(int id)
        {
            string sql = $"SELECT * FROM TutorialAppSchema.Users WHERE UserId={id}";
            User user = _dapper.LoadDataSingle<User>(sql); ;
            return user;
        }

        [HttpPut("EditUser")]
        public IActionResult EditUser(User user)
        {
            string sql = @"UPDATE TutorialAppSchema.Users 
                            SET [FirstName]='" + user.FirstName + "'" +
                                ",[LastName]='" + user.LastName + "'" +
                                ",[Email]='" + user.Email + "'" +
                                ",[Gender]='" + user.Gender + "'" +
                                ",[Active]='" + user.Active + "'" +
                                " WHERE UserId='" + user.UserId + "'";

            if (_dapper.ExecuteSql(sql))
            {
                return Ok();
            }

            throw new Exception("Failed to Update user");
        }

        [HttpPost("AddUser")]
        public IActionResult AddUser(AddUserDTO user)
        {
            string sql = @"INSERT INTO TutorialAppSchema.Users (
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
                                      "'" + user.Active + "')";

            if (_dapper.ExecuteSql(sql))
            {
                return Ok();
            }

            throw new Exception("Failed to Add user");
        }


        [HttpDelete("DeleteUser/{id}")]
        public IActionResult DeleteUser(int id)
        {
            string sql = $"DELETE FROM TutorialAppSchema.Users WHERE UserId={id}";

            if (_dapper.ExecuteSql(sql))
            {
                return Ok();
            }

            throw new Exception("Failed to Delete user");
        }
        #endregion

        #region User Salary

        [HttpGet("GetUserSalary/{id}")]
        public UserSalary GetUserSalary(int id)
        {
            string sql = $"SELECT * FROM TutorialAppSchema.UserSalary WHERE UserId={id}";
            UserSalary user = _dapper.LoadDataSingle<UserSalary>(sql); ;
            return user;
        }

        [HttpPut("EditUserSalary")]
        public IActionResult EditUserSalary(UserSalary user)
        {
            string sql = @"UPDATE TutorialAppSchema.UserSalary 
                            SET [Salary]='" + user.Salary + "'" +
                                " WHERE UserId='" + user.UserId + "'";

            if (_dapper.ExecuteSql(sql))
            {
                return Ok();
            }

            throw new Exception("Failed to Update user salary");
        }

        [HttpPost("AddUserSalary")]
        public IActionResult AddUserSalary(UserSalary user)
        {
            string sql = @"INSERT INTO TutorialAppSchema.UserSalary (
                                    [UserId],
                                    [Salary],
                                ) VALUES (" +
                                  "'" + user.UserId  +
                                  "'" + user.Salary + "')";

            if (_dapper.ExecuteSql(sql))
            {
                return Ok();
            }

            throw new Exception("Failed to Add user salary");
        }


        [HttpDelete("DeleteUserSalary/{id}")]
        public IActionResult DeleteUserSalary(int id)
        {
            string sql = $"DELETE FROM TutorialAppSchema.UserSalary WHERE UserId={id}";

            if (_dapper.ExecuteSql(sql))
            {
                return Ok();
            }

            throw new Exception("Failed to Delete user salary");
        }
        #endregion

        #region Job Info

        [HttpGet("GetUserJobInfo/{id}")]
        public UserJobInfo GetUserJobInfo(int id)
        {
            string sql = $"SELECT * FROM TutorialAppSchema.UserJobInfo WHERE UserId={id}";
            UserJobInfo user = _dapper.LoadDataSingle<UserJobInfo>(sql); ;
            return user;
        }

        [HttpPut("EditUserJobInfo")]
        public IActionResult EditUserSalary(UserJobInfo user)
        {
            string sql = @"UPDATE TutorialAppSchema.UserJobInfo 
                            SET [JobTitle]='" + user.JobTitle + "'" +
                                 ",[Department]='" + user.Department + "'" +
                                " WHERE UserId='" + user.UserId + "'";

            if (_dapper.ExecuteSql(sql))
            {
                return Ok();
            }

            throw new Exception("Failed to Update user job info");
        }

        [HttpPost("AddUserJobInfo")]
        public IActionResult AddUserJobInfo(UserJobInfo user)
        {
            string sql = @"INSERT INTO TutorialAppSchema.UserJobInfo (
                                    [UserId],
                                    [JobInfo],
                                    [Department],
                                ) VALUES (" +
                                  "'" + user.UserId  +
                                  "'" + user.JobTitle +
                                  "'" + user.Department + "')";

            if (_dapper.ExecuteSql(sql))
            {
                return Ok();
            }

            throw new Exception("Failed to Add user job info");
        }


        [HttpDelete("DeleteUserJobInfo/{id}")]
        public IActionResult DeleteUserJobInfo(int id)
        {
            string sql = $"DELETE FROM TutorialAppSchema.UserJobInfo WHERE UserId={id}";

            if (_dapper.ExecuteSql(sql))
            {
                return Ok();
            }

            throw new Exception("Failed to Delete user job info");
        }
        #endregion

    }
}


