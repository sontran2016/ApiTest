using System.ComponentModel.DataAnnotations;

namespace Core.Domain.Business
{
    public class ContactList : BaseEntity
    {
        /// <summary>
        /// Group Name
        /// </summary>
        [Required]
        public string Email { get; set; }

        public int? GroupContactId { get; set; }

        public GroupContact Group { get; set; }
    }
}
