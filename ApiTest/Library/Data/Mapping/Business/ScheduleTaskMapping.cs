using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain.Business;

namespace Data.Mapping.Business
{
    public class ScheduleTaskMapping : EntityTypeConfiguration<ScheduleTask>
    {

        public ScheduleTaskMapping()
        {
            // Primary Key
            HasKey(t => t.Id);
            // Table & Column Mappings
            ToTable("ScheduleTask");
        }
    }
}
