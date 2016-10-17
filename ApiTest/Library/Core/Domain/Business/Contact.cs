namespace Core.Domain.Business
{
    public class Contact: BaseEntity
    {
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
        /// Safety setting Id
        /// </summary>
        public int? SafetySettingId { get; set; }

        public SafetySetting SafetySetting { get; set; }
    }

    public class ExcelInfo: BaseEntity
    {
        public string PartnerID { get; set; }
        public string Style { get; set; }
        public string PartnerSKU { get; set; }
        public string UPC { get; set; }
        public string Description { get; set; }
        public string ColorCode { get; set; }
        public string ColorDesc { get; set; }
        public string SizeCode { get; set; }
        public string SizeDescription { get; set; }
        public string SizeClassDescription { get; set; }
        public string WeightLBS { get; set; }
        public string PreviewImageURL { get; set; }
    }
}
