using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AMS_API.Models
{
    public class User
    {
        public int UserId { get; set; }

        public string EmailId { get; set; }

        public string Password { get; set; }
        public Role Role { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }  
        public string LastName { get; set; }  
        public string ProfilePhoto { get; set; }
        public string ContactNo { get; set; }
        public bool IsActive { get; set; }
        public int RMId { get; set; }
        public User()
        {
            Role = new Role();
        }
    }
}
