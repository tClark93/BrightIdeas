using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrightIdeas.Models
{
    public class Like
    {
        [Key]
        public int id { get; set; }

        [ForeignKey("id")]
        public int Userid { get; set; }

        [ForeignKey("id")]
        public int Ideaid { get; set; }

        public User LikedByUser { get; set; }

        public Idea LikedIdea {get; set; }
    }
}