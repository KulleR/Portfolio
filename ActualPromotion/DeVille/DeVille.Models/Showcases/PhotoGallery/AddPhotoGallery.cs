using Deville.Models.MetaData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deville.Models.Showcases
{
    [MetadataType(typeof(PhotoGalleryMetaData))]
    public class AddPhotoGallery
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string ImgCover { get; set; }
    }
}
