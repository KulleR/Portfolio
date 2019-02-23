using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deville.Models.MetaData
{
    public class ReviewMetaData
    {
        [Display(Name = "Дата создания")]
        public System.DateTime CreateDate { get; set; }
        [Display(Name = "Заголовок")]
        public string Title { get; set; }
        [Display(Name = "Отзыв")]
        public string Message { get; set; }
        [Display(Name = "Полное имя автора")]
        public string AuthorFullName { get; set; }
        [Display(Name = "Фото автора")]
        public string AuthorPhoto { get; set; }
    }
}
