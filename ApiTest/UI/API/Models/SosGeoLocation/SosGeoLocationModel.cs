using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Models.SosGeoLocation
{
    /// <summary>
    /// Create SOS Geolocation Model
    /// </summary>
    public class CreateSosGeoLocationModel
    {
        /// <summary>
        /// YayYoId
        /// </summary>
        [Required]
        public int YayYoId { get; set; }

        /// <summary>
        /// GeoLocation
        /// </summary>
        [Required]
        public string Location { get; set; }
    }

    /// <summary>
    /// CreateListSosGeoLocationModel
    /// </summary>
    public class CreateListSosGeoLocationModel
    {
        /// <summary>
        /// YayYoId
        /// </summary>
        [Required]
        public int YayYoId { get; set; }

        /// <summary>
        /// GeoLocation
        /// </summary>
        [Required]
        public List<SosGeoLocationModel> Locations { get; set; }
    }

    /// <summary>
    /// SosGeoLocationModel
    /// </summary>
    public class SosGeoLocationModel
    {
        /// <summary>
        /// Location
        /// </summary>
        [Required]
        public string Location { get; set; }

        /// <summary>
        /// CreatedOnUtc
        /// </summary>
        [Required]
        public long Timestamp { get; set; }
    }
}