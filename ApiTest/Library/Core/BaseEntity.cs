using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core
{
    /// <summary>
    /// Base Entity for project
    /// </summary>
    public abstract class BaseEntity
    {       
        /// <summary>
        /// Id of entity with format as GUID
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

    
        /// <summary>
        /// Created Date in UTC of entity
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        protected BaseEntity()
        {
            CreatedOnUtc = DateTime.UtcNow;
        }     
    }
}
