using Deville.EntityDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deville.Repositories
{
    public class PhotoRepository : IPhotoRepository
    {
        private IRepository<Photo> photoRepository;

        public PhotoRepository(IRepository<Photo> _photoRepository)
        {
            this.photoRepository = _photoRepository;
        }

        public IQueryable<Photo> GetAll()
        {
            return photoRepository.GetAll();
        }

        public IQueryable<Photo> GetAlbumPhotos(int albumId)
        {
            return photoRepository.GetAll().Where(ph => ph.AlbumId == albumId);
        }

        public Photo GetById(int id)
        {
            if (id == 0)
                return null;
            return photoRepository.GetById(id);
        }

        public int Insert(Photo model)
        {
            if (model == null)
                throw new ArgumentNullException("photo");
            return photoRepository.Insert(model);
        }

        public void Update(Photo model)
        {
            if (model == null)
                throw new ArgumentNullException("photo");
            photoRepository.Update(model);

        }

        public void Delete(Photo model)
        {
            if (model == null)
                throw new ArgumentNullException("photo");
            photoRepository.Delete(model);
        }

        public void SaveChanges()
        {
            photoRepository.SaveChanges();
        }
    }
}
