using System;
using Deville.EntityDataModel;
using Deville.Models.Showcases;

namespace Deville.Core.Mapper
{
    using AutoMapper;
    public class CommonMapper : IMapper
    {
        /// <summary>
        /// Инициализирует новый экземпляр CommonMapper.</summary>
        static CommonMapper()
        {
            // ServiceCategory
            Mapper.CreateMap<ServiceCategory, EditServiceCategory>();
            Mapper.CreateMap<ServiceCategory, AddServiceCategory>();
            Mapper.CreateMap<EditServiceCategory, ServiceCategory>();
            Mapper.CreateMap<AddServiceCategory, ServiceCategory>();

            Mapper.CreateMap<ServiceCategory, DtoServiceCategory>();
            Mapper.CreateMap<DtoServiceCategory, ServiceCategory>();

            // ServiceSubcategory
            Mapper.CreateMap<ServiceSubcategory, EditServiceSubcategory>();
            Mapper.CreateMap<ServiceSubcategory, AddServiceSubcategory>();
            Mapper.CreateMap<EditServiceSubcategory, ServiceSubcategory>();
            Mapper.CreateMap<AddServiceSubcategory, ServiceSubcategory>();

            Mapper.CreateMap<ServiceSubcategory, DtoServiceSubcategory>();
            Mapper.CreateMap<DtoServiceSubcategory, ServiceSubcategory>();

            // Service
            Mapper.CreateMap<Service, EditService>();
            Mapper.CreateMap<Service, AddService>();
            Mapper.CreateMap<EditService, Service>();
            Mapper.CreateMap<AddService, Service>();

            // PhotoAlbum
            Mapper.CreateMap<PhotoAlbum, EditPhotoAlbum>();
            Mapper.CreateMap<PhotoAlbum, AddPhotoAlbum>();
            Mapper.CreateMap<EditPhotoAlbum, PhotoAlbum>();
            Mapper.CreateMap<AddPhotoAlbum, PhotoAlbum>();

            // PhotoGallery
            Mapper.CreateMap<PhotoGallery, EditPhotoGallery>();
            Mapper.CreateMap<PhotoGallery, AddPhotoGallery>();
            Mapper.CreateMap<EditPhotoGallery, PhotoGallery>();
            Mapper.CreateMap<AddPhotoGallery, PhotoGallery>();

            // Photo
            Mapper.CreateMap<Photo, EditPhoto>();
            Mapper.CreateMap<Photo, AddPhoto>();
            Mapper.CreateMap<EditPhoto, Photo>();
            Mapper.CreateMap<AddPhoto, Photo>();

            // Product
            Mapper.CreateMap<Product, EditProduct>();
            Mapper.CreateMap<Product, AddProduct>();
            Mapper.CreateMap<EditProduct, Product>();
            Mapper.CreateMap<AddProduct, Product>();

            // ProductCategory
            Mapper.CreateMap<ProductCategory, EditProductCategory>();
            Mapper.CreateMap<ProductCategory, AddProductCategory>();
            Mapper.CreateMap<EditProductCategory, ProductCategory>();
            Mapper.CreateMap<AddProductCategory, ProductCategory>();

            // Review
            Mapper.CreateMap<Review, EditReview>();
            Mapper.CreateMap<Review, AddReview>();
            Mapper.CreateMap<EditReview, Review>();
            Mapper.CreateMap<AddReview, Review>();

            // Vacancy
            Mapper.CreateMap<Vacancy, EditVacancy>();
            Mapper.CreateMap<Vacancy, AddVacancy>();
            Mapper.CreateMap<EditVacancy, Vacancy>();
            Mapper.CreateMap<AddVacancy, Vacancy>();

            // News
            Mapper.CreateMap<News, EditNews>();
            Mapper.CreateMap<News, AddNews>();
            Mapper.CreateMap<EditNews, News>();
            Mapper.CreateMap<AddNews, News>();
        }

        /// <summary>
        /// Выполняет отображение исходного объекта в новый объект назначения.</summary>
        /// <param name="source">Объект-источник данных для отображения</param>
        /// <param name="sourceType">Тип объекта-источника(использование)</param>
        /// <param name="destinationType">Тип объекта-назначения(создание)</param>
        /// <returns>Отображенный объект назначения</returns>
        public object Map(object source, Type sourceType, Type destinationType)
        {
            return Mapper.Map(source, sourceType, destinationType);
        }
    }
}