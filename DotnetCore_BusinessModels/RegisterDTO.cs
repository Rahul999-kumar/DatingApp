using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetCore_BusinessModels
{
    public class RegisterDTO
    {
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        [StringLength(8, MinimumLength =4)]
        public string Password { get; set; }
    }
}
