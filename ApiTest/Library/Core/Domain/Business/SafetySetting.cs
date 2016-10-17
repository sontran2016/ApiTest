using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Business
{
    public class SafetySetting: BaseEntity
    {
        /// <summary>
        /// YayYo User Id
        /// </summary>
        [Index("IX_Yayo", 1, IsUnique = true)]
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
        /// enable when user presses SOS button
        /// </summary>
        public bool Active { get; set; }
        /// <summary>
        /// Cancellation pin
        /// </summary>
        public string CancellationPin { get; set; }
        /// <summary>
        /// Duress pin
        /// </summary>
        public string DuressPin { get; set; }
        /// <summary>
        /// Timestamp
        /// </summary>
        public DateTime? UpdatedOnUtc { get; set; }
        /// <summary>
        /// List contact
        /// </summary>
        public ICollection<Contact> Contacts { get; set; }

        /// <summary>
        /// Ride Information
        /// </summary>
        public ICollection<LogSos> LogSoses { get; set; }
    }
}
