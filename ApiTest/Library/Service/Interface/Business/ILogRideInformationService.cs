using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Domain.Business;
using Service.Models.LogRideInformation;


namespace Service.Interface.Business
{

    public interface ILogRideInformationService
    {
        #region async

        /// <summary>
        /// Get contact by id async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<LogRideInformation> GetByIdAsync(int id);

        /// <summary>
        /// Create contact async
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        Task CreateAsync(LogRideInformation contact);

        /// <summary>
        /// Update contact async
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        Task UpdateAsync(LogRideInformation contact);

        /// <summary>
        /// Delete contact async
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        Task DeleteAsync(LogRideInformation contact);

        #endregion

        #region sync
        /// <summary>
        /// Get contact by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        LogRideInformation GetById(int id);

        /// <summary>
        /// Create contact
        /// </summary>
        /// <param name="contact"></param>
        void Create(LogRideInformation contact);

        /// <summary>
        /// Update contact
        /// </summary>
        /// <param name="contact"></param>
        void Update(LogRideInformation contact);

        /// <summary>
        /// Delete contact
        /// </summary>
        /// <param name="contact"></param>

        void Delete(LogRideInformation contact);
        /// <summary>
        /// Get current ride information
        /// </summary>
        /// <returns></returns>
        LogRideInformation GetLastestLogRideInformation(int yayYoId);
        /// <summary>
        /// Get Ridebook id
        /// </summary>
        /// <returns></returns>
        List<UserRideIdModel> GetRideBookId();

        #endregion
    }
}
