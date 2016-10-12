using System.ComponentModel.DataAnnotations;
using System;

namespace theWall.Models
{
    public class Message : BaseEntity
    {
        [Required]
        [MinLength(5)]
        public string message {get; set;}

        [Required]
        public int user_id {get; set;}      

        public int id {get; set;}  

        public DateTime created_at {get; set;}

        public DateTime updated_at {get; set;}
    }
}