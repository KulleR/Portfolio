using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deville.Models.MetaData
{
    public class PhotoAlbumMetaData
    {
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Display(Name = "Обложка")]
        public string ImgCover { get; set; }
    }
}
