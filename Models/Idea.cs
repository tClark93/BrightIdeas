using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrightIdeas.Models
{
    public class Idea
    {
        [Key]
        public int id { get; set; }

        [Required(ErrorMessage = "You can't post nothing")]
        [MinLength(10, ErrorMessage = "Ideas have a minimum length of 10 characters"),MaxLength(250, ErrorMessage = "Ideas have a maximum length of 250 characters")]
        public string Content { get; set; }

        public DateTime Created { get; set; }

        [ForeignKey("id")]
        public int Userid { get; set; }

        public User Author { get; set; }

        public List<Like> LikedBy { get; set; }

        public Idea()
        {
            LikedBy = new List<Like>();
            Created = DateTime.Now;
        }
    }
}