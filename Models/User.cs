using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrightIdeas.Models
{
    [Table("User")]
    public class User
    {
        public int id { get; set; }

        [Required(ErrorMessage = "Please provide a name")]
        [MinLengthAttribute(3, ErrorMessage="Minimum length is three letters"),MaxLength(45, ErrorMessage="Maximum length is forty-five letters")]
        [RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-_]*$", ErrorMessage = "Use letters only")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please provide an Alias")]
        [MinLengthAttribute(3, ErrorMessage="Minimum length is three characters"),MaxLength(45, ErrorMessage="Maximum length is forty-five characters")]
        public string Alias { get; set; }
 
        [Required(ErrorMessage = "Please enter valid email")]
        [EmailAddress]
        [MinLengthAttribute(3, ErrorMessage="Minimum length is three characters"),MaxLength(150, ErrorMessage="Maximum length is 150 characters")]
        [RegularExpression(@"^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$")]
        public string Email{ get; set; }

        [Required]
        [MinLengthAttribute(8, ErrorMessage="Minimum length is eight characters"),MaxLength(40, ErrorMessage="Maximum length is forty characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [NotMapped]
        public string Confirm { get; set; }

        public DateTime Created { get; set; }
        public List<Idea> UsersIdeas { get; set; }

        public List<Like> LikedIdeas { get; set; }

        public User()
        {
            UsersIdeas = new List<Idea>();
            LikedIdeas = new List<Like>();
            Created = DateTime.Now;
        }
    }
}