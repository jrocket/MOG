[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(MoG.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(MoG.App_Start.NinjectWebCommon), "Stop")]

namespace MoG.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using MoG.Domain.Service;
    using MoG.Domain.Repository;

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

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        public static IKernel CreatePublicKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

            RegisterServices(kernel);
            kernel.Rebind<IdbContextProvider>().To<dbContextProviderWithoutSingletonPerRequest>();
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IProjectService>().To<ProjectService>();
            kernel.Bind<IFileService>().To<FileService>();
            kernel.Bind<IUserService>().To<UserService>();
            kernel.Bind<ICommentService>().To<CommentService>();
            kernel.Bind<IMessageService>().To<MessageService>();

            kernel.Bind<IProjectRepository>().To<ProjectRepository>();
            kernel.Bind<IActivityRepository>().To<ActivityRepository>();
            kernel.Bind<IFileRepository>().To<FileRepository>();
            kernel.Bind<ICommentRepository>().To<CommentRepository>();
            kernel.Bind<IUserRepository>().To<UserRepository>();
            kernel.Bind<IMessageRepository>().To<MessageRepository>();

            kernel.Bind<IdbContextProvider>().To<dbContextProvider>();


        }        
    }
}
