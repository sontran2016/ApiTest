using System.ComponentModel.DataAnnotations;

namespace API.Models.Contact
{
    /// <summary>
    /// Create Contact Model
    /// </summary>
    public class CreateContactModel
    {
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
        /// Phone
        /// </summary>
        [Required]
        public string Phone { get; set; }

        /// <summary>
        /// Safety YayYoId
        /// </summary>
        [Required]
        public int YayYoId { get; set; }
    }

    /// <summary>
    /// Edit Contact Model
    /// </summary>
    public class EditContactModel
    {
        /// <summary>
        /// Id
        /// </summary>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// First Name
        /// </summary>
        [Required]
        public string FirstName { get; set; }
        
        /// <summary>
        /// Last Name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Phone
        /// </summary>
        [Required]
        public string Phone { get; set; }

    }
}