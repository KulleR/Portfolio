using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deville.Models.MetaData
{
    public class ServiceSubcategoryMetaData
    {
        [Display(Name = "Название подкатегории")]
        public string Title { get; set; }
        [Display(Name = "Описание")]
        public string Description { get; set; }
        [Display(Name = "Категория")]
        public int CategoryId { get; set; }
    }
}
