using DotnetAPI.Models;

namespace DotnetAPI.Data
{
    public interface IUserRepository
    {
        public bool SaveChanges();

        public void AddEntity<T>(T entityToAdd);

        public void RemoveEntity<T>(T entityToRemove);

        public IEnumerable<User> GetUsers();

        public User GetSingleUser(int id);

        public bool GetSingleUserEmail(string email);

        public UserSalary GetSingleUserSalary(int id);

        public UserJobInfo GetSingleUserJobInfo(int id);
    }
}
