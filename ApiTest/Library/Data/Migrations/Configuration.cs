using System;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using Common.Helpers;
using Core.Domain.Business;

namespace Data.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<NoisContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(NoisContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            #region Schedule Tasks

            context.Set<ScheduleTask>().AddOrUpdate(
                p => p.Name,
                new ScheduleTask
                {
                    Name = TaskType.SmsSafetyService,
                    Seconds = 60,
                    IsEnabled = true
                }
                );

            #endregion

            //update Function
            UpdateFunction(context);
            //update StoreProcedure
            UpdateStoreProcedure(context);
        }

        /// <summary>
        /// UpdateStoreProcedure
        /// </summary>
        /// <param name="context"></param>
        public void UpdateStoreProcedure(NoisContext context)
        {
            //
            var path = "/App_Data/StoreProcedures";

            if (HostingEnvironment.IsHosted)
            {
                //hosted
                path = HostingEnvironment.MapPath(path);
            }
            else
            {
                //not hosted. For example, run in unit tests
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                path = path.Replace("~/", "").TrimStart('/').Replace('/', '\\');
                path = Path.Combine(baseDirectory, path);
            }
            if (path != null)
            {
                var sortedFiles =
                    new DirectoryInfo(path.Replace(@"\Library\Data\bin\Debug", @"\UI\API")).GetFiles().ToList();
                if (sortedFiles.Any())
                {
                    //clear all sp
                    var query = @"declare @procName varchar(500)
                                    declare cur cursor 

                                    for select [name] from sys.objects where type = 'p'
                                    open cur
                                    fetch next from cur into @procName
                                    while @@fetch_status = 0
                                    begin
                                        exec('drop procedure [' + @procName + ']')
                                        fetch next from cur into @procName
                                    end
                                    close cur
                                    deallocate cur";

                    context.Database.ExecuteSqlCommand(query);
                    foreach (var file in sortedFiles)
                    {
                        var sqlCommand = File.ReadAllText(file.FullName);
                        context.Database.ExecuteSqlCommand(sqlCommand);
                    }

                }
            }
        }

        /// <summary>
        /// UpdateFunction
        /// </summary>
        /// <param name="context"></param>
        public void UpdateFunction(NoisContext context)
        {
            //
            var path = "/App_Data/Functions";

            if (HostingEnvironment.IsHosted)
            {
                //hosted
                path = HostingEnvironment.MapPath(path);
            }
            else
            {
                //not hosted. For example, run in unit tests
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                path = path.Replace("~/", "").TrimStart('/').Replace('/', '\\');
                path = Path.Combine(baseDirectory, path);
            }
            if (path != null)
            {
                var sortedFiles =
                    new DirectoryInfo(path.Replace(@"\Library\Data\bin\Debug", @"\UI\API")).GetFiles().ToList();
                if (sortedFiles.Any())
                {
                    //clear all functions
                    var query = @"declare @procName varchar(500)
                                    declare cur cursor 

                                    for select [name] from sys.objects where type in ('FN','TF') 
                                    open cur
                                    fetch next from cur into @procName
                                    while @@fetch_status = 0
                                    begin
                                        exec('drop function [' + @procName + ']')
                                        fetch next from cur into @procName
                                    end
                                    close cur
                                    deallocate cur";

                    context.Database.ExecuteSqlCommand(query);
                    foreach (var file in sortedFiles)
                    {
                        var sqlCommand = File.ReadAllText(file.FullName);
                        context.Database.ExecuteSqlCommand(sqlCommand);
                    }

                }
            }
        }
    }
}
