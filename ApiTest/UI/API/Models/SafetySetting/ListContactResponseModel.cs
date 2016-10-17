using System.Collections.Generic;

namespace API.Models.SafetySetting
{
    /// <summary>
    /// List YayYo User Response
    /// </summary>
    public class ListContactResponse
    {
        /// <summary>
        /// List YayYo User
        /// </summary>
        public List<ListContactResponseModel> Data { get; set; }
        /// <summary>
        /// TotalRecord
        /// </summary>
        public long TotalRecord { get; set; }
    }
    /// <summary>
    /// List contact response model
    /// </summary>
    public class ListContactResponseModel
    {
        /// <summary>
        /// Id
        /// </summary>        
        public int Id { get; set; }
        /// <summary>
        /// First Name
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Last Name
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// Phone
        /// </summary>
        public string Phone { get; set; }
    }
}