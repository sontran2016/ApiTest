using Core.Domain.Business;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Mapping.Business
{
    public class ContactListMapping : EntityTypeConfiguration<ContactList>
    {
        public ContactListMapping()
        {
            // Primary Key
            HasKey(t => t.Id);
            HasOptional(t => t.Group).WithMany(t => t.Contacts).HasForeignKey(t => t.GroupContactId);
            // Table & Column Mappings
            ToTable("ContactList");
        }
    }
}
