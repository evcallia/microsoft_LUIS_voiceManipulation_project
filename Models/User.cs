using System.ComponentModel.DataAnnotations;
using System;

namespace theWall.Models
{
    public abstract class BaseEntity {}

    public class User : BaseEntity
    {
        [Required]
        [MinLength(2)]
        public string first_name { get; set; }
        [Required]
        [MinLength(2)]
        public string last_name { get; set; }
        [Required]
        [EmailAddress]
        public string email { get; set; }
        [Required]
        [MinLength(8)]
        public string password { get; set; }
        [Compare("password", ErrorMessage = "password confirmation does not match")]
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