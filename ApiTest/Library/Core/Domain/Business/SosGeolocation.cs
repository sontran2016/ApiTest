using System.ComponentModel.DataAnnotations;

namespace Core.Domain.Business
{
    /// <summary>
    /// Log SOS Geolocation
    /// </summary>
    public class LogSosGeolocation: BaseEntity
    {
        /// <summary>
        /// Destination Location
        /// </summary>
        [MaxLength(100)]
        public string Location { get; set; }
        /// <summary>
        /// Log SOS Id
        /// </summary>
        public int? LogSosId { get; set; }
        /// <summary>
        /// Log SOS entity
        /// </summary>
        public LogSos LogSos { get; set; }
    }
}
