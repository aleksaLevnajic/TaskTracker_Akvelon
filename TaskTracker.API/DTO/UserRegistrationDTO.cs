using System.ComponentModel.DataAnnotations;

namespace TaskTracker.API.DTO
{
    public class UserRegistrationDTO
    {
        [Required(ErrorMessage = "First name is requierd.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last name is requierd.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Username is requierd.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is requierd.")]
        public string Password { get; set; }
    }
}
