using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LogItUpApi.Models
{
    public class ResetPasswordDTO
    {
        [Required(AllowEmptyStrings = false)]
        public string Email { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string ResetPasswordToken { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string NewPassword { get; set; }
    }
}
