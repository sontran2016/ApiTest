using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Domain.Business
{
    /// <summary>
    /// Log Ride Information
    /// </summary>
    public class LogRideInformation : BaseEntity
    {
        /// <summary>
        /// YayYo Id
        /// </summary>
        public int YayYoId { get; set; }
        /// <summary>
        /// Driver Name
        /// </summary>
        public string DriverName { get; set; }
        /// <summary>
        /// Car make
        /// </summary>
        public string CarMake { get; set; }
        /// <summary>
        /// Car Model
        /// </summary>
        public string CarModel { get; set; }
        /// <summary>
        /// Car Color
        /// </summary>
        public string CarColor { get; set; }
        /// <summary>
        /// Car License
        /// </summary>
        public string CarLicense { get; set; }
        /// <summary>
        /// Location Pickup
        /// </summary>
        [MaxLength(100)]
        public string LocationPickup { get; set; }

        /// <summary>
        /// Location Destination
        /// </summary>
        [MaxLength(100)]
        public string LocationDestination { get; set; }
        /// <summary>
        /// Time Pickup
        /// </summary>
        public DateTime? TimePickup { get; set; }
        /// <summary>
        /// Estimate time (minute)
        /// </summary>
        public int TimeEta { get; set; } 
        /// <summary>
        /// Log SOS Id
        /// </summary>
        public int? LogSosId { get; set; }
        /// <summary>
        /// Log SOS
        /// </summary>
        public LogSos LogSos { get; set; }
    }
}
