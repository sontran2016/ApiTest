using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Common.Logs;
using Core.Domain.Business;
using Service.CachingLayer;
using Service.Interface.Business;

namespace Service.Implement.Business
{

    public class LogSosService : BaseServiceWithLogging, ILogSosService
    {
        #region field
        private readonly DbContext _context;
        private readonly DbSet<LogSos> _dbSet;
        private readonly ICacheManager _cacheManager;
        private const string KeyForCacheYayYo = "YayYo.LogSos.Id.{0}";
        #endregion
        #region ctor
        /// <summary>
        /// ctr
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cacheManager"></param>
        /// <param name="noisLoggingService"></param>
        public LogSosService(DbContext context, ICacheManager cacheManager, INoisLoggingService noisLoggingService) : base(noisLoggingService)
        {
            _context = context;
            _cacheManager = cacheManager;
            _dbSet = _context.Set<LogSos>();

        }
        #endregion
        #region public method
        #region async
        public async Task<LogSos> GetByIdAsync(int id)
        {

            var result = _cacheManager.Get(String.Format(KeyForCacheYayYo, id),
                () =>
                {
                    var query = "SELECT * FROM LogSos WHERE Id = @p0";
                    var res = _dbSet.SqlQuery(query, id).SingleOrDefault();
                    return res;
                });

            return await Task.FromResult(result);
        }

        public Task<LogSos> GetLastestLogSosAsync(int yayYoId)
        {
            var query = "SELECT TOP 1 * FROM LogSos WHERE YayYoId = @p0 AND UpdatedOnUtc IS NULL ORDER BY id desc";
            var res = _dbSet.SqlQuery(query, yayYoId).SingleOrDefaultAsync();
            return res;
        }

        public async Task CreateAsync(LogSos logSos)
        {
            try
            {
                _dbSet.Add(logSos);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                LogError("There is error while updating LogSos: " + ex.Message, ex);
            }

        }

        public async Task UpdateAsync(LogSos logSos)
        {
            try
            {
                _cacheManager.Remove(String.Format(KeyForCacheYayYo, logSos.Id));
                //ref:http://patrickdesjardins.com/blog/entity-framework-ef-modifying-an-instance-that-is-already-in-the-context
                //A way to do it is to navigate inside the local of the DbSet to see if this one is there. If the entity is present, than you detach
                var local = _context.Set<LogSos>()
                         .Local
                         .FirstOrDefault(f => f.Id == logSos.Id);
                if (local != null)
                {
                    _context.Entry(local).State = EntityState.Detached;
                }
                _context.Entry(logSos).State = EntityState.Modified;
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                LogError("There is error while updating LogSos: " + ex.Message, ex);
            }
        }

        public async Task DeleteAsync(LogSos logSos)
        {
            try
            {
                _cacheManager.Remove(String.Format(KeyForCacheYayYo, logSos.Id));

                _context.Database.ExecuteSqlCommand("DELETE LogSos WHERE Id = @p0", logSos.Id);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                LogError("There is error while updating data: " + ex.Message, ex);
            }
        }
        #endregion

        #region sync
        public LogSos GetById(int id)
        {
            var result = _cacheManager.Get(String.Format(KeyForCacheYayYo, id),
                () =>
                {
                    var query = "SELECT * FROM LogSos WHERE Id = @p0";
                    var res = _dbSet.SqlQuery(query, id).SingleOrDefault();
                    return res;
                });

            return result;
        }

        public void Create(LogSos logSos)
        {
            try
            {
                _dbSet.Add(logSos);
                _context.SaveChanges();


            }
            catch (Exception ex)
            {
                LogError("There is error while updating LogSos: " + ex.Message, ex);
            }
        }

        public void Update(LogSos logSos)
        {
            try
            {
                _cacheManager.Remove(String.Format(KeyForCacheYayYo, logSos.Id));
                //ref:http://patrickdesjardins.com/blog/entity-framework-ef-modifying-an-instance-that-is-already-in-the-context
                //A way to do it is to navigate inside the local of the DbSet to see if this one is there. If the entity is present, than you detach
                var local = _context.Set<LogSos>()
                         .Local
                         .FirstOrDefault(f => f.Id == logSos.Id);
                if (local != null)
                {
                    _context.Entry(local).State = EntityState.Detached;
                }
                _context.Entry(logSos).State = EntityState.Modified;
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                //Trace.TraceError("There is error while updating data: " + dex.InnerException);
                LogError("There is error while updating LogSos: " + ex.Message, ex);
            }
        }

        public void Delete(LogSos logSos)
        {
            try
            {
                _cacheManager.Remove(String.Format(KeyForCacheYayYo, logSos.Id));

                _context.Database.ExecuteSqlCommand("DELETE LogSos WHERE Id = @p0", logSos.Id);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                LogError("There is error while updating LogSos: " + ex.Message, ex);
            }
        }

        public void StopProgressSosService()
        {
            try
            {
                const int minuteStop = 120;
                var sql = "select * from LogSos where UpdatedOnUtc is null and DATEDIFF(MINUTE,CreatedOnUtc,GETUTCDATE())>" + minuteStop;
                var logSoses = _dbSet.SqlQuery(sql).ToList();
                if (!logSoses.Any()) return;

                var sqlLogSos = "\nUpdate LogSos set UpdatedOnUtc='{0}' where Id={1}";
                var sqlSafetySetting = "\nUpdate SafetySetting set Active=0 where Id={0}";
                string queryLogSos = "", querySafetySetting = "";
                foreach (var log in logSoses)
                {
                    queryLogSos += string.Format(sqlLogSos, DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff"),log.Id);
                    querySafetySetting += string.Format(sqlSafetySetting, log.SafetySettingId);
                }
                _context.Database.ExecuteSqlCommand(queryLogSos);
                _context.Database.ExecuteSqlCommand(querySafetySetting);
            }
            catch (Exception ex)
            {
                LogError("There is error while updating LogSos: " + ex.Message, ex);
            }
        }



        #endregion
        #endregion
    }
}
