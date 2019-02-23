using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deville.Models.MetaData
{
    /// <summary>
    /// Класс для установки мета-данных соответствующей модели
    /// </summary>
    public class ServiceMetaData
    {
        [Display(Name = "Категория")]
        public int CategoryId { get; set; }

        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Топ-стилисты")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        public Nullable<decimal> VirtuosoPrice { get; set; }

        [Display(Name = "Стилисты")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        public Nullable<decimal> ExpertPrice { get; set; }

        [Display(Name = "Начинающие мастера")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        public Nullable<decimal> HandymanPrice { get; set; }

        [Display(Name = "Цена")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        public Nullable<decimal> Price { get; set; }

        [Display(Name = "Картинка")]
        public string ImgCover { get; set; }

        [Display(Name = "Подкатегория")]
        public int SubcategoryId { get; set; }
    }
}
