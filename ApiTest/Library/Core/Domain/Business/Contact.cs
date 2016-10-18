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

    
}
