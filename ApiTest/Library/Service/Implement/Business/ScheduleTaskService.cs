using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Common.Logs;
using Core.Domain.Business;
using Service.CachingLayer;
using Service.Interface.Business;

namespace Service.Implement.Business
{
    public class ScheduleTaskService : BaseServiceWithLogging, IScheduleTaskService
    {
        #region field
        private readonly DbContext _context;
        private readonly DbSet<ScheduleTask> _dbSet;
        private readonly ICacheManager _cacheManager;
        private const string KeyForCacheYayYo = "YayYo.ScheduleTask.Id.{0}";
        #endregion
        #region ctor
        /// <summary>
        /// ctr
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cacheManager"></param>
        /// <param name="noisLoggingService"></param>
        public ScheduleTaskService(DbContext context, ICacheManager cacheManager, INoisLoggingService noisLoggingService) : base(noisLoggingService)
        {
            _context = context;
            _cacheManager = cacheManager;
            _dbSet = _context.Set<ScheduleTask>();

        }
        #endregion
        #region public method
        #region async
        public async Task<ScheduleTask> GetByIdAsync(int id)
        {

            var result = _cacheManager.Get(String.Format(KeyForCacheYayYo, id),
                () =>
                {
                    var query = "SELECT * FROM ScheduleTask WHERE Id = @p0";
                    var res = _dbSet.SqlQuery(query, id).SingleOrDefault();
                    return res;
                });

            return await Task.FromResult(result);
        }

        public Task<List<ScheduleTask>> GetAllScheduleTaskEnabledAsync()
        {
            var query = "SELECT * FROM [ScheduleTask] WHERE IsEnabled = 1";
            var res = _dbSet.SqlQuery(query).ToListAsync();
            return res;
        }

        public async Task CreateAsync(ScheduleTask scheduleTask)
        {
            try
            {
                _dbSet.Add(scheduleTask);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                LogError("There is error while updating ScheduleTask: " + ex.Message, ex);
            }

        }

        public async Task UpdateAsync(ScheduleTask scheduleTask)
        {
            try
            {
                _cacheManager.Remove(String.Format(KeyForCacheYayYo, scheduleTask.Id));
                //ref:http://patrickdesjardins.com/blog/entity-framework-ef-modifying-an-instance-that-is-already-in-the-context
                //A way to do it is to navigate inside the local of the DbSet to see if this one is there. If the entity is present, than you detach
                var local = _context.Set<ScheduleTask>()
                         .Local
                         .FirstOrDefault(f => f.Id == scheduleTask.Id);
                if (local != null)
                {
                    _context.Entry(local).State = EntityState.Detached;
                }
                _context.Entry(scheduleTask).State = EntityState.Modified;
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                LogError("There is error while updating ScheduleTask: " + ex.Message, ex);
            }
        }

        public async Task DeleteAsync(ScheduleTask scheduleTask)
        {
            try
            {
                _cacheManager.Remove(String.Format(KeyForCacheYayYo, scheduleTask.Id));

                _context.Database.ExecuteSqlCommand("DELETE ScheduleTask WHERE Id = @p0", scheduleTask.Id);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                LogError("There is error while updating data: " + ex.Message, ex);
            }
        }

        #endregion

        #region sync
        public ScheduleTask GetById(int id)
        {
            var result = _cacheManager.Get(String.Format(KeyForCacheYayYo, id),
                () =>
                {
                    var query = "SELECT * FROM ScheduleTask WHERE Id = @p0";
                    var res = _dbSet.SqlQuery(query, id).SingleOrDefault();
                    return res;
                });

            return result;
        }

        public void Create(ScheduleTask scheduleTask)
        {
            try
            {
                _dbSet.Add(scheduleTask);
                _context.SaveChanges();


            }
            catch (Exception ex)
            {
                LogError("There is error while updating ScheduleTask: " + ex.Message, ex);
            }
        }

        public void Update(ScheduleTask scheduleTask)
        {
            try
            {
                _cacheManager.Remove(String.Format(KeyForCacheYayYo, scheduleTask.Id));
                //ref:http://patrickdesjardins.com/blog/entity-framework-ef-modifying-an-instance-that-is-already-in-the-context
                //A way to do it is to navigate inside the local of the DbSet to see if this one is there. If the entity is present, than you detach
                var local = _context.Set<ScheduleTask>()
                         .Local
                         .FirstOrDefault(f => f.Id == scheduleTask.Id);
                if (local != null)
                {
                    _context.Entry(local).State = EntityState.Detached;
                }
                _context.Entry(scheduleTask).State = EntityState.Modified;
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                //Trace.TraceError("There is error while updating data: " + dex.InnerException);
                LogError("There is error while updating ScheduleTask: " + ex.Message, ex);
            }
        }

        public void Delete(ScheduleTask scheduleTask)
        {
            try
            {
                _cacheManager.Remove(String.Format(KeyForCacheYayYo, scheduleTask.Id));

                _context.Database.ExecuteSqlCommand("DELETE ScheduleTask WHERE Id = @p0", scheduleTask.Id);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                LogError("There is error while updating ScheduleTask: " + ex.Message, ex);
            }
        }

        public List<ScheduleTask> GetAllScheduleTaskEnabled()
        {
            var query = "SELECT * FROM [ScheduleTask] WHERE IsEnabled = 1 AND (IsRunning = 0 OR DATEADD(MINUTE, 15, LastSuccessOnUtc) < GETUTCDATE() OR LastSuccessOnUtc IS NULL )";
            var res = _dbSet.SqlQuery(query).ToList();
            return res;
        }

        #endregion
        #endregion
    }
}
