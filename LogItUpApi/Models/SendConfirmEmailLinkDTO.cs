﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LogItUpApi.Models
{
    public class SendConfirmEmailLinkDTO
    {
        [Required(AllowEmptyStrings = false)]
        public string Email { get; set; } 
    }
}
