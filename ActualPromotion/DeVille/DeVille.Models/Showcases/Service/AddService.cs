using Deville.Models.MetaData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deville.Models.Showcases
{
    [MetadataType(typeof(ServiceMetaData))]
    public class AddService
    {
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? VirtuosoPrice { get; set; }
        public decimal? ExpertPrice { get; set; }
        public decimal? HandymanPrice { get; set; }
        public decimal? Price { get; set; }
        public string ImgCover { get; set; }
        public int? SubcategoryId { get; set; }
    }
}
