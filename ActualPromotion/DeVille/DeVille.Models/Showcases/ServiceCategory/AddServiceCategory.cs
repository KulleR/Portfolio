using Deville.Models.MetaData;
using System.ComponentModel.DataAnnotations;

namespace Deville.Models.Showcases
{
    [MetadataType(typeof(ServiceCategoryMetaData))]
    public class AddServiceCategory
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public string ImgCover { get; set; }
    }
}
