using System.ComponentModel.DataAnnotations;

namespace API.Models.SafetySetting
{
    /// <summary>
    /// Create Safety Setting Model
    /// </summary>
    public class CreateSafetySettingModel
    {
        /// <summary>
        /// YayYo User Id
        /// </summary>
        [Required]
        public int YayYoId { get; set; }

        /// <summary>
        /// First Name
        /// </summary>
        [Required]
        public string FirstName { get; set; }
        
        /// <summary>
        /// Last Name
        /// </summary>
        [Required]
        public string LastName { get; set; }
        
        /// <summary>
        /// Phone Number
        /// </summary>
        [Required]
        public string PhoneNumber { get; set; }
        
        /// <summary>
        /// Old Cancellation Pin
        /// </summary>
        public string OldCancellationPin { get; set; }
        /// <summary>
        /// Cancellation pin
        /// </summary>
        [Required]
        public string CancellationPin { get; set; }

        /// <summary>
        /// Old Duress pin
        /// </summary>        
        public string OldDuressPin { get; set; }
        /// <summary>
        /// Duress pin
        /// </summary>
        [Required]                
        public string DuressPin { get; set; }
    }
    /// <summary>
    /// Active SOS model
    /// </summary>
    public class ActiveSosModel
    {
        /// <summary>
        /// YayYo User Id
        /// </summary>
        [Required]
        public int YayYoId { get; set; }

        /// <summary>
        /// GeoLocation
        /// </summary>
        [Required]
        public string GeoLocation { get; set; }
    }
    /// <summary>
    /// Deactive Sos model
    /// </summary>
    public class DeactiveSosModel
    {
        /// <summary>
        /// YayYo User Id
        /// </summary>
        [Required]
        public int YayYoId { get; set; }
        /// <summary>
        /// Cancellation Pin
        /// </summary>
        public string CancellationPin { get; set; }
    }
    /// <summary>
    /// Deactive cancel SOS model
    /// </summary>
    public class DeactiveCancelSosResponseModel
    {
        /// <summary>
        /// Is Duress Mode 
        /// true:  Duress Pin is entered correctly
        /// false: Duress Pin is entered incorrectly
        /// false: Cancel Pin is entered correctly
        /// </summary>
        public bool IsDuressMode { get; set; }
    }
}