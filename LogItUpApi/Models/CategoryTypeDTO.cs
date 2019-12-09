using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LogItUpApi.Models
{
    public class CategoryTypeDTO
    {

        public long Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Description { get; set; }
    }
}
