using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace theWall.Models
{
    public class Message : BaseEntity
    {
        public Message(){
            comments = new List<Comment>();
        }
        [Required]
        [MinLength(5)]
        public string message {get; set;}

        [Required]
        public int user_id {get; set;}     
        public User user {get; set;} 

        public int id {get; set;}  

        public DateTime created_at {get; set;}

        public DateTime updated_at {get; set;}

        public List<Comment> comments {get; set;}
    }
}