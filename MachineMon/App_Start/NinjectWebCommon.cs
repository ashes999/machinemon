[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(MachineMon.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(MachineMon.App_Start.NinjectWebCommon), "Stop")]

namespace MachineMon.App_Start
{
    using System;
    using System.Web;
    using System.Linq;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using Core.Repositories;
    using Repository.Dapper.Repositories;
    using System.Configuration;
    using Core.Services;
    using Core.Domain;
    using System.Text;

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

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"];
            var repository = new GenericRepository(connectionString);

            kernel.Bind<IGenericRepository>().To<GenericRepository>().WithConstructorArgument<ConnectionStringSettings>(connectionString);

            var secureKey = repository.GetAll<ConfigurationSetting>().SingleOrDefault(c => c.Setting == "SecureKey");
            if (secureKey == null)
            {
                AesEncryptionService.GenerateSecureKeyIfMissing(repository);
                secureKey = repository.GetAll<ConfigurationSetting>().Single(c => c.Setting == "SecureKey");
            }
            
            kernel.Bind<IEncryptionService>().To<AesEncryptionService>().WithConstructorArgument(secureKey);
        }        
    }
}
