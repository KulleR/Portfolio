using Deville.Models.MetaData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deville.Models.Showcases
{
    [MetadataType(typeof(ProductCategoryMetaData))]
    public class AddProductCategory
    {
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public string ImgCover { get; set; }
    }
}
