using Deville.EntityDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deville.Repositories
{
    public interface IPhotoAlbumRepository
    {
        IQueryable<PhotoAlbum> GetAll();
        IQueryable<PhotoAlbum> GetGalleryAlbums(int galleryId);
        PhotoAlbum GetById(int id);
        int Insert(PhotoAlbum model);
        void Update(PhotoAlbum model);
        void Delete(PhotoAlbum model);
        void SaveChanges();
    }
}
