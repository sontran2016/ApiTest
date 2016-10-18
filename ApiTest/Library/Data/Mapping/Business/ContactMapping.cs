using System.Data.Entity.ModelConfiguration;
using Core.Domain.Business;

namespace Data.Mapping.Business
{
    public class ContactMapping :  EntityTypeConfiguration<Contact>
    {
        public ContactMapping()
        {
            // Primary Key
            HasKey(t => t.Id);
            HasOptional(t => t.SafetySetting).WithMany(t => t.Contacts).HasForeignKey(t => t.SafetySettingId);            
            // Table & Column Mappings
            ToTable("Contact");
        }
    }

    
}
