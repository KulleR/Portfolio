using Deville.Models.MetaData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deville.EntityDataModel
{
    public enum Status
    {
        Active = 5,
        Deleted = -99
    }

    [MetadataType(typeof(ProductCategoryMetaData))]
    public partial class ProductCategory : IEntity
    {
        public int ID
        {
            get { return this.Id; }
        }
    }
}
