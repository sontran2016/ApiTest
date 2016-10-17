using System.Data.Entity.ModelConfiguration;
using Core.Domain.Business;

namespace Data.Mapping.Business
{
    public class LogRideInformationMapping :  EntityTypeConfiguration<LogRideInformation>
    {
        public LogRideInformationMapping()
        {
            // Primary Key
            HasKey(t => t.Id);
            HasOptional(t => t.LogSos).WithMany(t => t.LogRideInformations).HasForeignKey(t => t.LogSosId);            
            // Table & Column Mappings
            ToTable("LogRideInformation");
        }
    }
}
