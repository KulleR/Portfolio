using Deville.Models.MetaData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deville.Models.Showcases
{
    [MetadataType(typeof(PhotoMetaData))]
    public class AddPhoto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string ImgUrl { get; set; }
        [Required]
        public int AlbumId { get; set; }
    }
}
