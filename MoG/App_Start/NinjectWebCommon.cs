[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(MoG.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(MoG.App_Start.NinjectWebCommon), "Stop")]

namespace MoG.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using MoG.Domain.Repository;
    using MoG.Domain.Service;
    using System.Web.Mvc;

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
            kernel.Bind<IActivityService>().To<ActivityService>();
            kernel.Bind<ITempFileService>().To<TempFileService>();
            kernel.Bind<IWaveformService>().To<WaveformService>();
            kernel.Bind<IDownloadCartService>().To<DownloadCartService>();
#if NOFILEUPLOAD
            kernel.Bind<ILocalStorageService>().To<FakeAzureStorageService>();
            kernel.Bind<IDropBoxService>().To<FakeDropboxService>();
#else

            kernel.Bind<ILocalStorageService>().To<AzureStorageService>();
            kernel.Bind<IDropBoxService>().To<DropBoxService>();
#endif
            
            kernel.Bind<IThumbnailService>().To<ThumbnailService>();
            kernel.Bind<ILikeService>().To<LikeService>();
            kernel.Bind<ILogService>().To<LogService>();
            kernel.Bind<ISocialService>().To<SocialService>();
            kernel.Bind<IInvitService>().To<InvitService>();
            kernel.Bind<INoteService>().To<NoteService>();
            kernel.Bind<IFollowService>().To<FollowService>();
            kernel.Bind<IScheduledTaskService>().To<ScheduledTaskService>().WithConstructorArgument("_cache", HttpRuntime.Cache);
            kernel.Bind<ISecurityService>().To<SecurityService>();
            kernel.Bind<IRegistrationCodeService>().To<RegistrationCodeService>();
            kernel.Bind<ISkydriveService>().To<SkydriveService>();
            kernel.Bind<IInviteMeService>().To<InviteMeService>();
            kernel.Bind<IParameterService>().To<ParameterService>();
            kernel.Bind<IMailService>().To<MailService>();
            kernel.Bind<IUserStatisticsService>().To<UserStatisticsService>();
            kernel.Bind<ICacheService>().To<InMemoryCache>();


            kernel.Bind<IProjectRepository>().To<ProjectRepository>();
            kernel.Bind<IActivityRepository>().To<ActivityRepository>();
            kernel.Bind<IFileRepository>().To<FileRepository>();
            kernel.Bind<ICommentRepository>().To<CommentRepository>();
            kernel.Bind<IUserRepository>().To<UserRepository>();
            kernel.Bind<IMessageRepository>().To<MessageRepository>();
            kernel.Bind<ITempFileRepository>().To<TempFileRepository>();
            kernel.Bind<IThumbnailRepository>().To<ThumbnailRepository>();
            kernel.Bind<IAuthCredentialRepository>().To<AuthCredentialRepository>();
            kernel.Bind<IDownloadCartRepository>().To<DownloadCartRepository>();
            kernel.Bind<ILikeRepository>().To<LikeRepository>();
            kernel.Bind<ILogRepository>().To<LogRepository>();
            kernel.Bind<IInvitRepository>().To<InvitRepository>();
            kernel.Bind<INoteRepository>().To<NoteRepository>();
            kernel.Bind<IFollowRepository>().To<FollowRepository>();
            kernel.Bind<IRegistrationCodeRepository>().To<RegistrationCodeRepository>();
            kernel.Bind<IInviteMeRepository>().To<InviteMeRepository>();
            kernel.Bind<IParameterRepository>().To<ParameterRepository>();
            kernel.Bind<INotificationRepository>().To<NotificationRepository>();

            kernel.Bind<IdbContextProvider>().To<dbContextProvider>();
        }        
    }
}
