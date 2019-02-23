using Deville.EntityDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deville.Repositories
{
    public class PhotoAlbumRepository : IPhotoAlbumRepository
    {
        private IRepository<PhotoAlbum> photoAlbumRepository;

        public PhotoAlbumRepository(IRepository<PhotoAlbum> _photoAlbumRepository)
        {
            this.photoAlbumRepository = _photoAlbumRepository;
        }

        public IQueryable<PhotoAlbum> GetAll()
        {
            return photoAlbumRepository.GetAll().Where(model => model.Status == (int)Status.Active);
        }

        public IQueryable<PhotoAlbum> GetGalleryAlbums(int galleryId)
        {
            return photoAlbumRepository.GetAll().Where(a => a.GalleryId == galleryId && a.Status == (int)Status.Active);
        }

        public PhotoAlbum GetById(int id)
        {
            if (id == 0)
                return null;
            PhotoAlbum photoAlbum = photoAlbumRepository.GetById(id);
            return photoAlbum != null && photoAlbum.Status == (int)Status.Active ? photoAlbum : null;
        }

        public int Insert(PhotoAlbum model)
        {
            if (model == null)
                throw new ArgumentNullException("photoAlbum");
            model.Status = (int)Status.Active;
            return photoAlbumRepository.Insert(model);
        }

        public void Update(PhotoAlbum model)
        {
            if (model == null)
                throw new ArgumentNullException("photoAlbum");
            photoAlbumRepository.Update(model);

        }

        public void Delete(PhotoAlbum model)
        {
            if (model == null)
                throw new ArgumentNullException("photoAlbum");
            model.Status = (int)Status.Deleted;
        }

        public void SaveChanges()
        {
            photoAlbumRepository.SaveChanges();
        }
    }
}
