using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LogItUpApi.Models
{
    public class UserTokenDTO
    {
        [Required(AllowEmptyStrings = false)]
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
