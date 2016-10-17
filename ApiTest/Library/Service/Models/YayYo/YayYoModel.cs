using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models.YayYo
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class GetLastestRideBookRequestModel
    {
        /// <summary>
        /// YayYo Id
        /// </summary>
        public int yayYo_id { get ; set; }
    }
    /// <summary>
    /// Ridebook Information
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class GetLastestRideBookResponseModel
    {
        /// <summary>
        /// Driver name
        /// </summary>
        public string driver_name { get; set; }
        /// <summary>
        /// Car make
        /// </summary>
        public string car_make { get; set; }
        /// <summary>
        /// Car model
        /// </summary>
        public string car_model { get; set; }
        /// <summary>
        /// Car color
        /// </summary>
        public string car_color { get; set; }
        /// <summary>
        /// Car license
        /// </summary>
        public string car_license { get; set; }
        /// <summary>
        /// Pickup Location
        /// </summary>
        public string pickup_location { get; set; }
        /// <summary>
        /// Time Pickup
        /// </summary>
        public DateTime time_pickup { get; set; }
        public string destination_location { get; set; }
        /// <summary>
        /// Estimate time (minute)
        /// </summary>
        public int time_eta { get; set; }
    }

    public class SmsRequestModel
    {
        /// <summary>
        /// FirstName
        /// </summary>
        [Required]
        public string FirstName { get; set; }

        /// <summary>
        /// PhoneNumber
        /// </summary>
        [Required]
        public string ContactPhoneNumber { get; set; }

        /// <summary>
        /// RideScheduledTime
        /// </summary>
        [Required]
        public DateTime RideScheduledTime { get; set; }

        /// <summary>
        /// Rider Pickup Location
        /// </summary>
        [Required]
        public string RiderPickupLocation { get; set; }

        /// <summary>
        /// Destination Address
        /// </summary>
        [Required]
        public string DestinationAddress { get; set; }

        /// <summary>
        /// Geolocation Address
        /// </summary>
        public string GeolocationAddress { get; set; }
    }
}
