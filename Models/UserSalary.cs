using DotnetAPI.Dtos;

namespace DotnetAPI.Models
{
    public partial class UserSalary
    {
        public int UserId { get; set; }

        public decimal Salary { get; set; }

        public UserSalary()
        {
         
        }

        public UserSalary(UserSalary user) {
            Salary = user.Salary;
        }
    }
}
