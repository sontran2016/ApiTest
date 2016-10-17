using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Domain.Business;
using Service.Models.ContactModel;
using System.IO;

namespace Service.Interface.Business
{

    public interface IContactService
    {
        #region async

        /// <summary>
        /// Get contact by id async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Contact> GetByIdAsync(int id);

        /// <summary>
        /// Get By SafetySettingId Async
        /// </summary>
        /// <param name="safetySettingId"></param>
        /// <returns></returns>
        Task<List<Contact>> GetBySafetySettingIdAsync(int safetySettingId);

        /// <summary>
        /// Create contact async
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        Task CreateAsync(Contact contact);

        /// <summary>
        /// Update contact async
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        Task UpdateAsync(Contact contact);

        /// <summary>
        /// Delete contact async
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        Task DeleteAsync(Contact contact);

        /// <summary>
        /// Search contact by keyword
        /// </summary>
        /// <param name="yayYoId"></param>
        /// <param name="countSkip"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <param name="keySort"></param>
        /// <param name="orderDescending"></param>
        /// <returns></returns>
        Task<List<ContactSearchModel>> GetListAsync(int yayYoId, int? countSkip, int? pageSize, string keyword, string keySort, bool orderDescending);

        Task<bool> ImportContactAsyn(Stream data, string fileName);
        #endregion

        #region sync
        /// <summary>
        /// Get contact by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Contact GetById(int id);

        /// <summary>
        /// Create contact
        /// </summary>
        /// <param name="contact"></param>
        void Create(Contact contact);

        /// <summary>
        /// Update contact
        /// </summary>
        /// <param name="contact"></param>
        void Update(Contact contact);

        /// <summary>
        /// Delete contact
        /// </summary>
        /// <param name="contact"></param>

        void Delete(Contact contact);
        
        #endregion        
    }
}
