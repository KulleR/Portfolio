using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deville.Models.MetaData
{
    public class ProductMetaData
    {
        [Display(Name = "Название")]
        public string Title { get; set; }
        [Display(Name = "Описание")]
        public string Description { get; set; }
        [Display(Name = "Изображение")]
        public string ImgCover { get; set; }
        [Display(Name = "Артикул")]
        public string Article { get; set; }
        [Display(Name = "Цена")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        public Nullable<decimal> Price { get; set; }
        [Display(Name = "Категория продукта")]
        public int CategoryId { get; set; }
        [Display(Name = "Новинка")]
        public bool IsNovelty { get; set; }
    }
}
