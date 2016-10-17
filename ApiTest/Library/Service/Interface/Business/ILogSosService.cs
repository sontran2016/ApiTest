using System.Threading.Tasks;
using Core.Domain.Business;

namespace Service.Interface.Business
{

    public interface ILogSosService
    {
        #region async

        /// <summary>
        /// Get logSos by id async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<LogSos> GetByIdAsync(int id);

        /// <summary>
        /// Get lastest logSos by YayYo Id
        /// </summary>
        /// <param name="yayYoId"></param>
        /// <returns></returns>
        Task<LogSos> GetLastestLogSosAsync(int yayYoId);

        /// <summary>
        /// Create logSos async
        /// </summary>
        /// <param name="logSos"></param>
        /// <returns></returns>
        Task CreateAsync(LogSos logSos);

        /// <summary>
        /// Update logSos async
        /// </summary>
        /// <param name="logSos"></param>
        /// <returns></returns>
        Task UpdateAsync(LogSos logSos);

        /// <summary>
        /// Delete logSos async
        /// </summary>
        /// <param name="logSos"></param>
        /// <returns></returns>
        Task DeleteAsync(LogSos logSos);        
        #endregion

        #region sync
        /// <summary>
        /// Get logSos by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        LogSos GetById(int id);

        /// <summary>
        /// Create logSos
        /// </summary>
        /// <param name="logSos"></param>
        void Create(LogSos logSos);

        /// <summary>
        /// Update logSos
        /// </summary>
        /// <param name="logSos"></param>
        void Update(LogSos logSos);

        /// <summary>
        /// Delete logSos
        /// </summary>
        /// <param name="logSos"></param>

        void Delete(LogSos logSos);

        /// <summary>
        /// Stop progress SOS if more than 2 hours
        /// </summary>
        void StopProgressSosService();

        #endregion        
    }
}
