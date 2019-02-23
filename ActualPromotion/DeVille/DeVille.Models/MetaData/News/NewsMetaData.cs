using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deville.Models.MetaData
{
    public class NewsMetaData
    {
        [Display(Name = "Заголовок")]
        public string Title { get; set; }
        [Display(Name = "Текст")]
        public string Text { get; set; }
        [Display(Name = "Дата сздания")]
        public System.DateTime CreateDate { get; set; }
        [Display(Name = "Картинка")]
        public string ImgCover { get; set; }
    }
}
