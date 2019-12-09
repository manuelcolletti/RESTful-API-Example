using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogItUpApi.Entities
{
    public class Category: UserEntity
    {
        public long CategoryTypeId { get; set; }
        public virtual CategoryType CategoryType { get; set; }
    }
}