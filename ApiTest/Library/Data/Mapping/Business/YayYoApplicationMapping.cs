using System.Data.Entity.ModelConfiguration;
using Core.Domain.Business;

namespace Data.Mapping.Business
{
    public class YayYoApplicationMapping : EntityTypeConfiguration<YayYoApplication>
    {
        public YayYoApplicationMapping()
        {
            // Primary Key
            HasKey(t => t.Id);

            // Table & Column Mappings
            ToTable("YayYoApplication");
        }
    }
}
