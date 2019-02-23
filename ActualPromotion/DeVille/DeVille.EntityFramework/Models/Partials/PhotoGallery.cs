using Deville.Models.MetaData;
using System.ComponentModel.DataAnnotations;

namespace Deville.EntityDataModel
{
    [MetadataType(typeof(PhotoGalleryMetaData))]
    public partial class PhotoGallery : IEntity
    {
        public int ID
        {
            get { return this.Id; }
        }
    }
}
