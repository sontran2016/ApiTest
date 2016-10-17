using System;
using Service.Interface.BackgroundTask;
using Service.Interface.Business;
using Common.Helpers;

namespace Service.Implement.BackgroundTask
{
    public class TaskService : ITaskService
    {
        #region fields

        private readonly IScheduleTaskService _scheduleTaskService;
        private readonly ILogSosService _logSosService;
        private static Object _factLock = new Object();
        #endregion

        #region ctors
        public TaskService(IScheduleTaskService scheduleTaskService, ILogSosService logSosService)
        {
            _scheduleTaskService = scheduleTaskService;
            _logSosService = logSosService;
        }

        #endregion

        #region public methods
        public void ScheduleTask()
        {
            lock (_factLock)
            {
                var allScheduleTasks = _scheduleTaskService.GetAllScheduleTaskEnabled();
                foreach (var task in allScheduleTasks)
                {
                    if (task.Name == TaskType.SmsSafetyService)
                    {
                        task.IsRunning = true;
                        task.LastStartOnUtc = DateTime.UtcNow;
                        _scheduleTaskService.Update(task);

                        _logSosService.StopProgressSosService();

                        task.LastSuccessOnUtc = DateTime.UtcNow;
                        task.IsRunning = false;
                        task.LastEndOnUtc = DateTime.UtcNow;
                        _scheduleTaskService.Update(task);
                    }

                }
                //todo excute schedule task
            }
        }
        #endregion
    }
}
