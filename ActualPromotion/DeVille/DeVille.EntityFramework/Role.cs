//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Deville.EntityDataModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class Role
    {
        public Role()
        {
            this.Users = new HashSet<User>();
        }
    
        public int Id { get; set; }
        public string RoleName { get; set; }
        public string CyrillicName { get; set; }
    
        public virtual ICollection<User> Users { get; set; }
    }
}