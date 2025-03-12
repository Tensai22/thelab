using System.ComponentModel.DataAnnotations;

namespace TheLab.Models
{
    public class Message
    {
        [Required(ErrorMessage = "Field name required to be filled")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Field email required to be filled")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Message field cannot be empty")]
        public string Content { get; set; }
    }


}
