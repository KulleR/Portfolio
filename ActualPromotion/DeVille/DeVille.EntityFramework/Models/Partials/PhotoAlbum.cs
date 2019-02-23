using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deville.EntityDataModel
{
    public partial class PhotoAlbum : IEntity
    {
        public int ID
        {
            get { return this.Id; }
        }
    }
}
