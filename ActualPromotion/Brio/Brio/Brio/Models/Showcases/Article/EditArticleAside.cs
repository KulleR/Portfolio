using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brio.Models
{
    public class EditArticleAside
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Заглавие")]
        public string Title { get; set; }
        [Required]
        [Display(Name = "Текст")]
        public string Text { get; set; }
        public int ArticleId { get; set; }
    }
}
