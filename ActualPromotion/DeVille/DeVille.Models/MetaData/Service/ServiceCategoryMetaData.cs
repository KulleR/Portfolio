using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deville.Models.MetaData
{
    public class ServiceCategoryMetaData
    {
        [Display(Name = "Название категории")]
        public string Name { get; set; }
        [Display(Name = "Описание")]
        public string Description { get; set; }
        [Display(Name = "Картинка")]
        public string ImgCover { get; set; }
    }
}
