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
    
    public partial class Review
    {
        public int Id { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string AuthorFullName { get; set; }
        public string AuthorPhoto { get; set; }
    }
}
