using Deville.Models.MetaData;
using System.ComponentModel.DataAnnotations;

namespace Deville.EntityDataModel
{
    [MetadataType(typeof(VacancyMetaData))]
    public partial class Vacancy : IEntity
    {
        public int ID
        {
            get { return this.Id; }
        }
    }
}
