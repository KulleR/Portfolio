using Deville.Models.MetaData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deville.Models.Showcases
{
    [MetadataType(typeof(ProductMetaData))]
    public class EditProduct
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImgCover { get; set; }
        public string Article { get; set; }
        public Nullable<decimal> Price { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public bool IsNovelty { get; set; }
    }
}
