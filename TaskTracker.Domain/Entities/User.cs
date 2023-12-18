using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracker.DataAccess.Entities;

namespace TaskTracker.Domain.Entities
{
    public class User : BaseEntity
    {
        [Required(ErrorMessage = "First name is requierd.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last name is requierd.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Username is requierd.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is requierd.")]
        public string PasswordHash { get; set; }
    }
}
