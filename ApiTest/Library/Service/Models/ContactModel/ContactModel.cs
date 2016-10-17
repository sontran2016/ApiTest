using System.Globalization;
using Core.Domain.Business;
using CsvHelper.Configuration;

namespace Service.Models.ContactModel
{
    /// <summary>
    /// Contact search model
    /// </summary>
    public class ContactSearchModel
    {
        /// <summary>
        /// Id
        /// </summary>        
        public int Id { get; set; }
        /// <summary>
        /// First Name
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Last Name
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// Phone
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// RowCounts
        /// </summary>
        public long? RowCounts { get; set; }

    }

    public sealed class ImportContactModelMap : CsvClassMap<ExcelInfo>
    {
        public ImportContactModelMap()
        {
            Map(m => m.PartnerID).Index(0);
            Map(m => m.Style).Index(1).TypeConverterOption(CultureInfo.InvariantCulture);
            Map(m => m.PartnerSKU).Index(2);
            Map(m => m.UPC).Index(3);
            Map(m => m.Description).Index(4);
            Map(m => m.ColorCode).Index(5);
            Map(m => m.ColorDesc).Index(6);
            Map(m => m.SizeCode).Index(7);
            Map(m => m.SizeDescription).Index(8);
            Map(m => m.SizeClassDescription).Index(10);//not get column 9
            Map(m => m.WeightLBS).Index(11);
            Map(m => m.PreviewImageURL).Index(12);
        }
    }
}
