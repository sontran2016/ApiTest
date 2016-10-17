using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Domain.Business;

namespace Service.Interface.Business
{
    public interface IScheduleTaskService
    {
        #region async

        /// <summary>
        /// Get scheduleTask by id async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ScheduleTask> GetByIdAsync(int id);

        /// <summary>
        /// GetAllScheduleTaskEnabled
        /// </summary>
        /// <returns></returns>
        Task<List<ScheduleTask>> GetAllScheduleTaskEnabledAsync();

        /// <summary>
        /// Create scheduleTask async
        /// </summary>
        /// <param name="scheduleTask"></param>
        /// <returns></returns>
        Task CreateAsync(ScheduleTask scheduleTask);

        /// <summary>
        /// Update scheduleTask async
        /// </summary>
        /// <param name="scheduleTask"></param>
        /// <returns></returns>
        Task UpdateAsync(ScheduleTask scheduleTask);

        /// <summary>
        /// Delete scheduleTask async
        /// </summary>
        /// <param name="scheduleTask"></param>
        /// <returns></returns>
        Task DeleteAsync(ScheduleTask scheduleTask);

        #endregion

        #region sync
        /// <summary>
        /// Get scheduleTask by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ScheduleTask GetById(int id);

        /// <summary>
        /// GetAllScheduleTaskEnabled
        /// </summary>
        /// <returns></returns>
        List<ScheduleTask> GetAllScheduleTaskEnabled();

        /// <summary>
        /// Create scheduleTask
        /// </summary>
        /// <param name="scheduleTask"></param>
        void Create(ScheduleTask scheduleTask);

        /// <summary>
        /// Update scheduleTask
        /// </summary>
        /// <param name="scheduleTask"></param>
        void Update(ScheduleTask scheduleTask);

        /// <summary>
        /// Delete scheduleTask
        /// </summary>
        /// <param name="scheduleTask"></param>

        void Delete(ScheduleTask scheduleTask);
        #endregion
    }
}
