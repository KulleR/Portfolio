using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deville.Models.MetaData
{
    public class OnlineAppointmentMetaData
    {
        [Display(Name = "Имя")]
        public string AuthorName { get; set; }
        [Display(Name = "Категория")]
        public int ServiceId { get; set; }
        [Display(Name = "Телефон")]
        public string Phone { get; set; }
        [Display(Name = "Почтовый адрес")]
        public string Email { get; set; }
    }
}
