using Core.Domain.Business;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Mapping.Business
{
    public class GroupContactMapping : EntityTypeConfiguration<GroupContact>
    {
        public GroupContactMapping()
        {
            // Primary Key
            HasKey(t => t.Id);
            // Table & Column Mappings
            ToTable("GroupContact");
        }
    }
}
