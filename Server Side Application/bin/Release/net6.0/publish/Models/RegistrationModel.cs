using System.ComponentModel.DataAnnotations;

namespace Server_Side_Application.Models
{
    public class RegistrationModel
    {
        [MinLength(3, ErrorMessage = "Usernames can be minimum 3 characters!")]
        [MaxLength(50, ErrorMessage = "Usernames can be maximum 50 characters!")]
        public string UserName { get; set; }
        [Required]
        [StringLength(64, ErrorMessage = "Something went wrong. Please try again!")]
        public string Password { get; set; }    
    }
}
