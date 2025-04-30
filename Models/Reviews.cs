using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheLab.Models
{
    public class Reviews
    {
        public int Id { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        public double Rating { get; set; }

        [Required]
        public int UserId { get; set; }  

        
        public AppUser? User { get; set; }  
    }
}
