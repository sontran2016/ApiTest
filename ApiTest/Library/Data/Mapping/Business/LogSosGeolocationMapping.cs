using System.Data.Entity.ModelConfiguration;
using Core.Domain.Business;

namespace Data.Mapping.Business
{
    public class LogSosGeolocationMapping :  EntityTypeConfiguration<LogSosGeolocation>
    {
        public LogSosGeolocationMapping()
        {
            // Primary Key
            HasKey(t => t.Id);
            HasOptional(t => t.LogSos).WithMany(t => t.SosGeolocations).HasForeignKey(t => t.LogSosId);            
            // Table & Column Mappings
            ToTable("LogSosGeolocation");
        }
    }
}
