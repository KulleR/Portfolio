using Deville.Models.MetaData;
using System.ComponentModel.DataAnnotations;

namespace Deville.EntityDataModel
{
    [MetadataType(typeof(PhotoMetaData))]
    public partial class Photo : IEntity
    {
        public int ID
        {
            get { return this.Id; }
        }
    }
}
