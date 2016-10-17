using System.Data.Entity;
using API.Providers;
using Autofac;
using Autofac.Integration.WebApi;
using Common.Logs;
using Data;
using Microsoft.Owin.Security.DataProtection;
using Owin;
using Service.CachingLayer;
using Service.Implement.AzureBlob;
using Service.Implement.BackgroundTask;
using Service.Implement.Business;
using Service.Implement.SendgridEmail;
using Service.Implement.Twilio;
using Service.Interface.BackgroundTask;
using Service.Interface.Business;
using Service.Interface.Media;
using Service.Interface.SendEmail;
using Service.Interface.Twilio;
using Service.Implement.YayYo;
using Service.Interface.YayYo;
using Service.Implement.Export;
using Service.Interface.Export;

namespace API
{
    /// <summary>
    /// 
    /// </summary>
    public static class AutofacConfig
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static ContainerBuilder Configuration(IAppBuilder app)
        {
            var builder = new ContainerBuilder();

            // Register your Web API controllers.

           // builder.RegisterAssemblyTypes(typeof(ApiController).Assembly);
          
            //builder.RegisterControllers(typeof(ApiController).Assembly);

            builder.RegisterApiControllers(typeof(WebApiApplication).Assembly);

            builder.Register(c => new NoisContext())
                .As<DbContext>().InstancePerDependency();
            #region cache

            builder.RegisterType<MemoryCacheManager>()
                .As<ICacheManager>().InstancePerDependency();

            #endregion
            #region logging
            
            builder.RegisterType<Log4NetService>()
             .As<INoisLoggingService>().InstancePerDependency();
            #endregion
            
            builder.RegisterType<SendgridEmailService>()
               .As<IEmailService>().InstancePerDependency();
           
            var dataProtectionProvider = new MachineKeyProtectionProvider();
            builder.Register<IDataProtectionProvider>(cc => dataProtectionProvider).InstancePerDependency();

            //business
            builder.RegisterType<YayYoApplicationService>()
              .As<IYayYoApplicationService>().InstancePerDependency();
            builder.RegisterType<SafetySettingService>()
              .As<ISafetySettingService>().InstancePerDependency();
            builder.RegisterType<ContactService>()
              .As<IContactService>().InstancePerDependency();
            builder.RegisterType<LogSosService>()
              .As<ILogSosService>().InstancePerDependency();
            builder.RegisterType<LogRideInformationService>()
              .As<ILogRideInformationService>().InstancePerDependency();
            builder.RegisterType<SosGeolocationService>()
              .As<ISosGeolocationService>().InstancePerDependency();
            builder.RegisterType<YayYoService>()
              .As<IYayYoService>().InstancePerDependency();
            builder.RegisterType<ScheduleTaskService>()
              .As<IScheduleTaskService>().InstancePerDependency();
            builder.RegisterType<TaskService>()
              .As<ITaskService>().InstancePerDependency();

            builder.RegisterType<ExportExelService>()
              .As<IExportExel>().InstancePerDependency();
            builder.RegisterType<ExportPdfService>()
              .As<IExportPdf>().InstancePerDependency();
            

            //Twilio Service
            builder.RegisterType<TwilioService>()
              .As<ITwilioService>().InstancePerDependency();

            //register AzureBlobSavingService
            builder.RegisterType<AzureBlobSavingService>()
               .As<IMediaService>().InstancePerDependency();
            return builder; 
        }
    }
}