using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Domain.Business
{
    public class GroupContact : BaseEntity
    {
        /// <summary>
        /// Group Name
        /// </summary>
        [Required]
        public string Name { get; set; }

        public ICollection<ContactList> Contacts { get; set; }
    }
}
