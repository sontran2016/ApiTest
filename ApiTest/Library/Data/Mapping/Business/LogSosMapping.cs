using System.Data.Entity.ModelConfiguration;
using Core.Domain.Business;

namespace Data.Mapping.Business
{
    public class LogSosMapping :  EntityTypeConfiguration<LogSos>
    {
        public LogSosMapping()
        {
            // Primary Key
            HasKey(t => t.Id);
            HasOptional(t => t.SafetySetting).WithMany(t => t.LogSoses).HasForeignKey(t => t.SafetySettingId);            
            // Table & Column Mappings
            ToTable("LogSosGeolocation");
        }
    }
}
