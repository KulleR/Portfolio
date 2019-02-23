using Deville.EntityDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deville.Repositories
{
    public class PhotoGalleryRepository : IPhotoGalleryRepository
    {
        private IRepository<PhotoGallery> photoGalleryRepository;

        /// <summary>
        /// Предоставляет доступ к хранилищу данных о категориях услуг
        /// </summary>
        private readonly IPhotoAlbumRepository photoAlbumRepository;

        public PhotoGalleryRepository(IRepository<PhotoGallery> _photoGalleryRepository, IPhotoAlbumRepository _photoAlbumRepository)
        {
            this.photoGalleryRepository = _photoGalleryRepository;
            this.photoAlbumRepository = _photoAlbumRepository;
        }

        public IQueryable<PhotoGallery> GetAll()
        {
            return photoGalleryRepository.GetAll().Where(model => model.Status == (int)Status.Active);
        }

        public PhotoGallery GetById(int id)
        {
            if (id == 0)
                return null;
            PhotoGallery photoGallery = photoGalleryRepository.GetById(id);
            return photoGallery != null && photoGallery.Status == (int)Status.Active ? photoGallery : null;
        }

        public int Insert(PhotoGallery model)
        {
            if (model == null)
                throw new ArgumentNullException("photoGallery");
            model.Status = (int)Status.Active;
            return photoGalleryRepository.Insert(model);
        }

        public void Update(PhotoGallery model)
        {
            if (model == null)
                throw new ArgumentNullException("photoGallery");
            photoGalleryRepository.Update(model);

        }

        public void Delete(PhotoGallery model)
        {
            if (model == null)
                throw new ArgumentNullException("photoGallery");
            model.Status = (int)Status.Deleted;
            foreach (PhotoAlbum pa in model.PhotoAlbums)
            {
                photoAlbumRepository.Delete(pa);
                photoAlbumRepository.SaveChanges();
            }
        }

        public void SaveChanges()
        {
            photoGalleryRepository.SaveChanges();
        }
    }
}
