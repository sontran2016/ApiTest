using System;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using API.Helpers;
using Autofac;
using Autofac.Integration.WebApi;
using Hangfire;
using Hangfire.SqlServer;
using Common.Helpers;
using Service.Interface.BackgroundTask;

[assembly: OwinStartup(typeof(API.Startup))]

namespace API
{
    public partial class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public static IContainer container { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        public void Configuration(IAppBuilder app)
        {
            var httpConfig = new HttpConfiguration();
            //register DI here
            var builder = AutofacConfig.Configuration(app);
            container = builder.Build();

            var resolver = new AutofacWebApiDependencyResolver(container);
            httpConfig.DependencyResolver = resolver;

            ConfigureAuth(app, resolver);
            app.UseAutofacMiddleware(container);
            app.UseAutofacWebApi(httpConfig);
            app.UseWebApi(httpConfig);
            WebApiConfig.Register(httpConfig);
            //app.UseCookieAuthentication(httpConfig);
            Hangfire.GlobalConfiguration.Configuration
             .UseSqlServerStorage("HangfireContext", new SqlServerStorageOptions { QueuePollInterval = TimeSpan.FromSeconds(1) })
             .UseAutofacActivator(container)
             ;

            app.UseHangfireServer();
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
#pragma warning disable 618
                AuthorizationFilters = new[] { new MyRestrictiveAuthorizationFilter() }
#pragma warning restore 618
            });

            RecurringJob.RemoveIfExists(TaskType.ScheduleTask);
            RecurringJob.AddOrUpdate<ITaskService>(TaskType.ScheduleTask, p => p.ScheduleTask(), Cron.Minutely);
        }

    }
}
