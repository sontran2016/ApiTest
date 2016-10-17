using System;

namespace Core.Domain.Business
{
    public class ScheduleTask : BaseEntity
    {
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the run period (in seconds)
        /// </summary>
        public int Seconds { get; set; }

        /// <summary>
        /// Gets or sets the value indicating whether a task is enabled
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// LastStartOnUtc
        /// </summary>
        public DateTime? LastStartOnUtc { get; set; }

        /// <summary>
        /// LastEndOnUtc
        /// </summary>
        public DateTime? LastEndOnUtc { get; set; }

        /// <summary>
        /// LastSuccessOnUtc
        /// </summary>
        public DateTime? LastSuccessOnUtc { get; set; }

        /// <summary>
        /// IsRunning
        /// </summary>
        public bool IsRunning { get; set; }
    }
}
