using Deville.EntityDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deville.Repositories
{
    public interface IPhotoGalleryRepository
    {
        IQueryable<PhotoGallery> GetAll();
        PhotoGallery GetById(int id);
        int Insert(PhotoGallery model);
        void Update(PhotoGallery model);
        void Delete(PhotoGallery model);
        void SaveChanges();
    }
}
