using Deville.Models.MetaData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deville.Models.Showcases
{
    [MetadataType(typeof(OnlineAppointmentMetaData))]
    public class SendOnlineAppointment
    {
        [Required]
        public int ServiceId { get; set; }
        [Required]
        public string AuthorName { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
