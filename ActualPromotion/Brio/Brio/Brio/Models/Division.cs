//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Brio.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Division
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Head { get; set; }
        public int CompanyId { get; set; }
        public int State { get; set; }
    
        public virtual Company Company { get; set; }
    }
}
