[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(DeVille.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(DeVille.NinjectWebCommon), "Stop")]

namespace DeVille
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using Deville.Repositories;
    using Deville.EntityDataModel;
    using Deville.EntityDataModel.DataContext;
    using Deville.Models;
    using Deville.Core.Mapper;
    using Deville.Core.Context;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
                kernel.Bind<IDataContext>().To<DeVilleEntities>().InRequestScope();
                kernel.Bind(typeof(IRepository<>)).To(typeof(Repository<>));
                kernel.Bind<IServiceCategoryRepository>().To<ServiceCategoryRepository>();
                kernel.Bind<IServiceSubcategoryRepository>().To<ServiceSubcategoryRepository>();
                kernel.Bind<IServiceRepository>().To<ServiceRepository>();
                kernel.Bind<IPhotoAlbumRepository>().To<PhotoAlbumRepository>();
                kernel.Bind<IPhotoGalleryRepository>().To<PhotoGalleryRepository>();
                kernel.Bind<IDevilleContext>().To<DevilleContext>();
                kernel.Bind<IAuthentication>().To<CustomAuthentication>().InRequestScope();
                kernel.Bind<IUserRepository>().To<UserRepository>();
                kernel.Bind<IPhotoRepository>().To<PhotoRepository>();
                kernel.Bind<IProductRepository>().To<ProductRepository>();
                kernel.Bind<IProductCategoryRepository>().To<ProductCategoryRepository>();
                kernel.Bind<IReviewRepository>().To<ReviewRepository>();
                kernel.Bind<IVacancyRepository>().To<VacancyRepository>();
                kernel.Bind<IOnlineAppointmentRepository>().To<OnlineAppointmentRepository>();
                kernel.Bind<INewsRepository>().To<NewsRepository>();
                kernel.Bind<IMapper>().To<CommonMapper>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!1
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
        }        
    }
}
