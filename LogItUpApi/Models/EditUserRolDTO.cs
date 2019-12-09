using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LogItUpApi.Models
{
    public class EditUserRolDTO
    {
        [Required(AllowEmptyStrings = false)]
        public string UserId { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string RoleName { get; set; }
    }
}
