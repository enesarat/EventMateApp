using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMate.Core.Model.Token
{
    public class TokenRequest
    {
        [Required]
        [MaxLength(50)]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
