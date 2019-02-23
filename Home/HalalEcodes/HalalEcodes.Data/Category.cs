using System;
using System.Collections.Generic;
using HalalEcodes.Data.Models;

namespace HalalEcodes.Data
{
    public partial class Category : IEntity
    {
        public Category()
        {
            Ecode = new HashSet<Ecode>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Desc { get; set; }

        public ICollection<Ecode> Ecode { get; set; }
    }
}
