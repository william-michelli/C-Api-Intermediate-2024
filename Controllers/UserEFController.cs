using AutoMapper;
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
    public class UserEFController : ControllerBase
    {
        IUserRepository _userRepository;
        IMapper _mapper;

        public UserEFController(IConfiguration config, IUserRepository userRepository)
        {
            _userRepository = userRepository;

            _mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AddUserDTO, User>();
            }));
        }

        #region User

        [HttpGet("GetUsers")]
        public IEnumerable<User> GetUsers()
        {
            IEnumerable<User> users = _userRepository.GetUsers();
            return users;
        }

        [HttpGet("GetSingleUser/{id}")]
        public User GetSingleUser(int id)
        {
            return _userRepository.GetSingleUser(id);
        }

        [HttpPut("EditUser")]
        public IActionResult EditUser(User user)
        {
            User? userDb = _userRepository.GetSingleUser(user.UserId);

            if (userDb != null)
            {
                userDb.FirstName = user.FirstName;
                userDb.LastName = user.LastName;
                userDb.Email = user.Email;
                userDb.Gender = user.Gender;
                userDb.Active = user.Active;

                if (_userRepository.SaveChanges())
                {
                    return Ok();
                }

                throw new Exception("Failed to Update user");
            }

            throw new Exception("Failed to Get user");
        }

        [HttpPost("AddUser")]
        public IActionResult AddUser(AddUserDTO user)
        {
            //░░░░░░░░░░░░░░  SEM MAPPER ░░░░░░░░░░░░░░  
            //User userDb = new User(user);

            //_userRepository.AddEntity<User>(userDb)

            //if (_userRepository.SaveChanges())
            //{
            //    return Ok();
            //}


            //throw new Exception("Failed to Add user");

            //░░░░░░░░░░░░░░  COM MAPPER ░░░░░░░░░░░░░░  

            User userDb = _mapper.Map<User>(user);

            _userRepository.AddEntity<User>(userDb);

            if (_userRepository.SaveChanges())
            {
                return Ok();
            }


            throw new Exception("Failed to Add user");
        }


        [HttpDelete("DeleteUser/{id}")]
        public IActionResult DeleteUser(int id)
        {
            User? user = _userRepository.GetSingleUser(id);

            if (user != null)
            {
                _userRepository.RemoveEntity<User>(user);

                if (_userRepository.SaveChanges())
                {
                    return Ok();
                }

                throw new Exception("Failed to Delete user");
            }

            throw new Exception("Failed to Get user");
        }
        #endregion

        #region Salary

        [HttpGet("GetUserSalary/{id}")]
        public UserSalary GetUserSalary(int id)
        {
            return _userRepository.GetSingleUserSalary(id);
        }

        [HttpPut("EditUserSalary")]
        public IActionResult EditUserSalary(UserSalary user)
        {
            UserSalary? userDb = _userRepository.GetSingleUserSalary(user.UserId);

            if (userDb != null)
            {
                userDb.Salary = user.Salary;

                if (_userRepository.SaveChanges())
                {
                    return Ok();
                }

                throw new Exception("Failed to Update user salary");
            }

            throw new Exception("Failed to Get user salary");
        }

        [HttpPost("AddUserSalary")]
        public IActionResult AddUserSalary(UserSalary user)
        {
            UserSalary userDb = _mapper.Map<UserSalary>(user);

            _userRepository.AddEntity<UserSalary>(userDb);

            if (_userRepository.SaveChanges())
            {
                return Ok();
            }


            throw new Exception("Failed to Add user salary");
        }


        [HttpDelete("DeleteUserSalary/{id}")]
        public IActionResult DeleteUserSalary(int id)
        {
            UserSalary? user = _userRepository.GetSingleUserSalary(id);

            if (user != null)
            {
                _userRepository.RemoveEntity<UserSalary>(user);

                if (_userRepository.SaveChanges())
                {
                    return Ok();
                }

                throw new Exception("Failed to Delete user salary");
            }

            throw new Exception("Failed to Get user salary");
        }
        #endregion

        #region Job Info

        [HttpGet("GetUserJobInfo/{id}")]
        public UserJobInfo GetUserJobInfo(int id)
        {
            return _userRepository.GetSingleUserJobInfo(id);
        }

        [HttpPut("EditUserJobInfo")]
        public IActionResult EditUserJobInfo(UserJobInfo user)
        {
            UserJobInfo? userDb = _userRepository.GetSingleUserJobInfo(user.UserId);

            if (userDb != null)
            {
                userDb.JobTitle = user.JobTitle;
                userDb.Department = user.Department;

                if (_userRepository.SaveChanges())
                {
                    return Ok();
                }

                throw new Exception("Failed to Update user job info");
            }

            throw new Exception("Failed to Get user job info");
        }

        [HttpPost("AddUserJobInfo")]
        public IActionResult AddUserJobInfo(UserJobInfo user)
        {
            UserJobInfo userDb = _mapper.Map<UserJobInfo>(user);

            _userRepository.AddEntity<UserJobInfo>(userDb);

            if (_userRepository.SaveChanges())
            {
                return Ok();
            }

            throw new Exception("Failed to Add user job info");
        }


        [HttpDelete("DeleteUserJobInfo/{id}")]
        public IActionResult DeleteUserJobInfo(int id)
        {
            UserJobInfo? user = _userRepository.GetSingleUserJobInfo(id);

            if (user != null)
            {
                _userRepository.RemoveEntity<UserJobInfo>(user);

                if (_userRepository.SaveChanges())
                {
                    return Ok();
                }

                throw new Exception("Failed to Delete user job info");
            }

            throw new Exception("Failed to Get user job info");
        }
        #endregion

    }
}


