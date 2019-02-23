using Deville.Models.MetaData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deville.EntityDataModel
{
    [MetadataType(typeof(NewsMetaData))]
    public partial class News : IEntity
    {
        public int ID
        {
            get { return this.Id; }
        }
    }
}
