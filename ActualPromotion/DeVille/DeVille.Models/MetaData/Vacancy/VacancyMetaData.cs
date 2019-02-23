using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deville.Models.MetaData
{
    public class VacancyMetaData
    {
        [Display(Name = "Заголовок")]
        public string Title { get; set; }
        [Display(Name = "Описание")]
        public string Description { get; set; }
        [Display(Name = "Обложка")]
        public string ImgCover { get; set; }
        [Display(Name = "Требования")]
        public string Demands { get; set; }
        [Display(Name = "Обязанности")]
        public string Duties { get; set; }
        [Display(Name = "Условия")]
        public string Сondition { get; set; }
    }
}
