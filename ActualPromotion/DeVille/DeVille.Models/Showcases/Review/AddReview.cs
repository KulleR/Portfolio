using Deville.Models.MetaData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deville.Models.Showcases
{
    [MetadataType(typeof(ReviewMetaData))]
    public class AddReview
    {
        [Required]
        public System.DateTime CreateDate { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public string AuthorFullName { get; set; }
        [Required]
        public string AuthorPhoto { get; set; }
    }
}
