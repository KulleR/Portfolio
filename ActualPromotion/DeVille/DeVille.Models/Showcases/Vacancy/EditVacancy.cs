using Deville.Models.MetaData;
using System.ComponentModel.DataAnnotations;

namespace Deville.Models.Showcases
{
    [MetadataType(typeof(VacancyMetaData))]
    public class EditVacancy
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImgCover { get; set; }
        public string Demands { get; set; }
        public string Duties { get; set; }
        public string Сondition { get; set; }
    }
}
