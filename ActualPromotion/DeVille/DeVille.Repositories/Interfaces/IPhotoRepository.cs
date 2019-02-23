using Deville.EntityDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deville.Repositories
{
    public interface IPhotoRepository
    {
        IQueryable<Photo> GetAll();
        IQueryable<Photo> GetAlbumPhotos(int albumId);
        Photo GetById(int id);
        int Insert(Photo model);
        void Update(Photo model);
        void Delete(Photo model);
        void SaveChanges();
    }
}
