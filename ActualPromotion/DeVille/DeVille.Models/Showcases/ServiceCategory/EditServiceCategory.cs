using Deville.Models.MetaData;
using System.ComponentModel.DataAnnotations;

namespace Deville.Models.Showcases
{
    [MetadataType(typeof(ServiceCategoryMetaData))]
    public class EditServiceCategory
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImgCover { get; set; }
    }
}
