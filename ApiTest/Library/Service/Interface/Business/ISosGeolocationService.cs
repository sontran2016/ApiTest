using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Domain.Business;


namespace Service.Interface.Business
{

    public interface ISosGeolocationService
    {
        #region async

        /// <summary>
        /// Get sosGeolocation by id async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<LogSosGeolocation> GetByIdAsync(int id);

        /// <summary>
        /// Get By Log SosId Async
        /// </summary>
        /// <param name="logSosId"></param>
        /// <returns></returns>
        Task<List<LogSosGeolocation>> GetByLogSosIdAsync(int logSosId);

        /// <summary>
        /// Create sosGeolocation async
        /// </summary>
        /// <param name="logSosGeolocation"></param>
        /// <returns></returns>
        Task CreateAsync(LogSosGeolocation logSosGeolocation);

        /// <summary>
        /// Create list sosGeolocation Async
        /// </summary>
        /// <param name="logSosGeolocation"></param>
        /// <returns></returns>
        Task InsertRangeAsync(List<LogSosGeolocation> logSosGeolocation);

        /// <summary>
        /// Update sosGeolocation async
        /// </summary>
        /// <param name="logSosGeolocation"></param>
        /// <returns></returns>
        Task UpdateAsync(LogSosGeolocation logSosGeolocation);

        /// <summary>
        /// Delete sosGeolocation async
        /// </summary>
        /// <param name="logSosGeolocation"></param>
        /// <returns></returns>
        Task DeleteAsync(LogSosGeolocation logSosGeolocation);

        #endregion

        #region sync
        /// <summary>
        /// Get sosGeolocation by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        LogSosGeolocation GetById(int id);

        /// <summary>
        /// Create sosGeolocation
        /// </summary>
        /// <param name="logSosGeolocation"></param>
        void Create(LogSosGeolocation logSosGeolocation);

        /// <summary>
        /// Update sosGeolocation
        /// </summary>
        /// <param name="logSosGeolocation"></param>
        void Update(LogSosGeolocation logSosGeolocation);

        /// <summary>
        /// Delete sosGeolocation
        /// </summary>
        /// <param name="logSosGeolocation"></param>

        void Delete(LogSosGeolocation logSosGeolocation);

        #endregion
    }
}
