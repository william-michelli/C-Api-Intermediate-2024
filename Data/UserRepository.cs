using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Data
{
    public class UserRepository : IUserRepository
    {
        DataContextEF _entityFramework;

        public UserRepository(IConfiguration config)
        {
            _entityFramework = new DataContextEF(config);
        }

        public bool SaveChanges()
        {
            return _entityFramework.SaveChanges() > 0;
        }

        public void AddEntity<T>(T entityToAdd)
        {
            if(entityToAdd != null) 
                _entityFramework.Add(entityToAdd);
        }

        public void RemoveEntity<T>(T entityToRemove)
        {
            if (entityToRemove != null)
                _entityFramework.Remove(entityToRemove);
        }

        public IEnumerable<User> GetUsers()
        {
            IEnumerable<User> users = _entityFramework.Users.ToList<User>();
            return users;
        }

        public User GetSingleUser(int id)
        {
            User? user = _entityFramework.Users
                .Where(u => u.UserId == id)
                .FirstOrDefault<User>();

            if (user != null)
            {
                return user;
            }

            throw new Exception("Failed to Get user");
        }

        public bool GetSingleUserEmail(string email)
        {
            User? user = _entityFramework.Users
                .Where(u => u.Email == email)
                .FirstOrDefault<User>();

            if (user != null)
            {
                return true;
            }

            return false;

        }

        public UserSalary GetSingleUserSalary(int id)
        {
            UserSalary? userSalary = _entityFramework.UserSalary
                .Where(u => u.UserId == id)
                .FirstOrDefault<UserSalary>();

            if (userSalary != null)
            {
                return userSalary;
            }

            throw new Exception("Failed to Get user salary");
        }

        public UserJobInfo GetSingleUserJobInfo(int id)
        {
            UserJobInfo? userJobInfo = _entityFramework.UserJobInfo
                .Where(u => u.UserId == id)
                .FirstOrDefault<UserJobInfo>();

            if (userJobInfo != null)
            {
                return userJobInfo;
            }

            throw new Exception("Failed to Get user job info");
        }

    }
}
