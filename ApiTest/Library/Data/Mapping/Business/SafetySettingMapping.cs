using System.Data.Entity.ModelConfiguration;
using Core.Domain.Business;

namespace Data.Mapping.Business
{
    public class SafetySettingMapping : EntityTypeConfiguration<SafetySetting>
    {
        public SafetySettingMapping()
        {
            // Primary Key
            HasKey(t => t.Id);           
            // Table & Column Mappings
            ToTable("SafetySetting");
        }
    }
}
