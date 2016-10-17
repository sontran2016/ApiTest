using System;
using System.Collections.Generic;
using System.Data.Entity;
using Common.Helpers;
using Core.Domain.Business;
using Data.Mapping.Business;

namespace Data
{
    /// <summary>
    /// EF context for YayYo
    /// </summary>
    public class NoisContext : DbContext
    {
        public NoisContext()
            : base("NoisContext")
        {
            Database.SetInitializer(new NoisContextInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new YayYoApplicationMapping());
            modelBuilder.Configurations.Add(new SafetySettingMapping());
            modelBuilder.Configurations.Add(new ContactMapping());
            modelBuilder.Configurations.Add(new LogRideInformationMapping());
            modelBuilder.Configurations.Add(new LogSosGeolocationMapping());
            modelBuilder.Configurations.Add(new ScheduleTaskMapping());
            modelBuilder.Configurations.Add(new GroupContactMapping());
            modelBuilder.Configurations.Add(new ContactListMapping());
            modelBuilder.Configurations.Add(new ExcelInfoMapping());
        }
    }

    /// <summary>
    /// Context initializer
    /// </summary>
    public class NoisContextInitializer : CreateDatabaseIfNotExists<NoisContext>
    {
        protected override void Seed(NoisContext context)
        {
            var secret = Guid.NewGuid().ToString();
            var secret1 = Guid.NewGuid().ToString();
            var applications = new List<YayYoApplication>
            {

              new YayYoApplication
              {
                  AppSecret =CommonSecurityHelper.GetHash(secret),
                  EncryptSecret = CommonSecurityHelper.Encrypt(secret, CommonSecurityHelper.KeyEncrypt),
                  Active = true,
                  AllowOrigin = "*",
                  Name = "Web",
                  Description = "Web app",
                  RefreshTokenLifeTime = 365*24*60,
                  Type = ApplicationType.Javascript
              },
              new YayYoApplication
              {
                  AppSecret =CommonSecurityHelper.GetHash(secret1),
                  EncryptSecret = CommonSecurityHelper.Encrypt(secret1, CommonSecurityHelper.KeyEncrypt),
                  Active = true,
                  AllowOrigin = "*",
                  Name = "Mobile",
                  Description = "Mobile app",
                  RefreshTokenLifeTime = 365*24*60,
                  Type = ApplicationType.Native
              }
            };

            var appDbSets = context.Set<YayYoApplication>();
            appDbSets.AddRange(applications);
            context.SaveChanges();
        }
    }
}
