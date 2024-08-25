using DotnetAPI.Dtos;

namespace DotnetAPI.Models
{
    public partial class User
    {
        public int UserId { get; set; }

        public string FirstName { get; set; } = "";

        public string LastName { get; set; } = "";

        public string Email { get; set; } = "";

        public string Gender { get; set; } = "";

        public bool Active { get; set; }

        public User()
        {

        }

        public User(AddUserDTO user)
        {
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            Gender = user.Gender;
            Active = user.Active;
        }
    }
}
