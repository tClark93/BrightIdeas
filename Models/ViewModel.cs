using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrightIdeas.Models
{
    public class ViewModel
    {
        public User User { get; set; }
        public List<Idea> UsersIdeas { get; set; }
        public List<Idea> AllIdeas { get; set; }
        public List<Like> LikedIdeas { get; set; }
        public Idea Idea { get; set; }
        public List<Idea> LikedBy { get; set; }
        public Like Like { get; set; }
    }
}