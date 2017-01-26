using System.ComponentModel.DataAnnotations;
using System;

namespace theWall.Models
{
    public abstract class BaseEntity {}

    public class User : BaseEntity
    {
        [Required(ErrorMessage = "First name is required")]
        [MinLength(2, ErrorMessage = "First name must be at least 2 characters")]
        public string first_name { get; set; }
        [Required(ErrorMessage = "Last name is required")]
        [MinLength(2, ErrorMessage = "Last name must be at least 2 characters")]
        public string last_name { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        public string password { get; set; }
        [Compare("password", ErrorMessage = "Password confirmation does not match")]
        public string password_confirmation { get; set; } 
        public int id { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string encPass {get; set;}

        // public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        // {
        //     if (password != passConf)
        //     {
        //         yield return new ValidationResult("password confirmation does not match", new [] { "passConf" });
        //     }
        // }
    }
}