using System;
using System.Collections.Generic;

namespace Core.Domain.Business
{
    /// <summary>
    /// Log SOS ID
    /// </summary>
    public class LogSos: BaseEntity
    {
        /// <summary>
        /// YayYo Id
        /// </summary>
        public int YayYoId { get; set; }        
        /// <summary>
        /// Safety Setting Id
        /// </summary>
        public int? SafetySettingId { get; set; }        
        /// <summary>
        /// Updated UTC Date
        /// </summary>
        public DateTime? UpdatedOnUtc { get; set; }
        /// <summary>
        /// Safety Setting
        /// </summary>
        public SafetySetting SafetySetting { get; set; }
        /// <summary>
        /// List GeoLocation
        /// </summary>
        public ICollection<LogSosGeolocation> SosGeolocations { get; set; }
        /// <summary>
        /// List LogRideInformations
        /// </summary>
        public ICollection<LogRideInformation> LogRideInformations { get; set; }
    }
}
