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
    /// <summary>
    /// Saftety Setting Service
    /// </summary>
    public class SosGeolocationService : BaseServiceWithLogging, ISosGeolocationService
    {
        #region field
        private readonly DbContext _context;
        private readonly DbSet<LogSosGeolocation> _dbSet;
        private readonly ICacheManager _cacheManager;
        private const string KeyForCacheYayYo = "YayYo.SosGeolocation.Id.{0}";
        #endregion
        #region ctor
        /// <summary>
        /// ctr
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cacheManager"></param>
        /// <param name="noisLoggingService"></param>
        public SosGeolocationService(DbContext context, ICacheManager cacheManager, INoisLoggingService noisLoggingService) : base(noisLoggingService)
        {
            _context = context;
            _cacheManager = cacheManager;
            _dbSet = _context.Set<LogSosGeolocation>();

        }
        #endregion
        #region public method
        #region async
        public async Task<LogSosGeolocation> GetByIdAsync(int id)
        {

            var result = _cacheManager.Get(String.Format(KeyForCacheYayYo, id),
                () =>
                {
                    var query = "SELECT * FROM SosGeolocation WHERE Id = @p0";
                    var res = _dbSet.SqlQuery(query, id).SingleOrDefault();
                    return res;
                });

            return await Task.FromResult(result);
        }

        public async Task<List<LogSosGeolocation>> GetByLogSosIdAsync(int logSosId)
        {
            var result = _cacheManager.Get(String.Format(KeyForCacheYayYo, logSosId),
                () =>
                {
                    var query = "SELECT * FROM SosGeolocation WHERE LogSosId = @p0";
                    var res = _dbSet.SqlQuery(query, logSosId).ToList();
                    return res;
                });

            return await Task.FromResult(result);
        }

        public async Task CreateAsync(LogSosGeolocation logSosGeolocation)
        {
            try
            {
                _dbSet.Add(logSosGeolocation);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                LogError("There is error while updating SosGeolocation: " + ex.Message, ex);
            }

        }

        public async Task InsertRangeAsync(List<LogSosGeolocation> logSosGeolocations)
        {
            try
            {
                var query = "";
                var queryString = "\nINSERT dbo.LogSosGeolocation( Location, LogSosId, CreatedOnUtc ) VALUES  ( N\'{0}\', {1}, '{2}')";
                foreach (var log in logSosGeolocations)
                {
                    query = query + string.Format(queryString, log.Location, log.LogSosId, log.CreatedOnUtc.ToString("yyyy-MM-dd HH:mm:ss.fff"));                    
                }
                await _context.Database.ExecuteSqlCommandAsync(query);
            }
            catch (Exception ex)
            {
                LogError("There is error while updating SosGeolocation: " + ex.Message, ex);
            }
        }

        public async Task UpdateAsync(LogSosGeolocation logSosGeolocation)
        {
            try
            {
                _cacheManager.Remove(String.Format(KeyForCacheYayYo, logSosGeolocation.Id));
                //ref:http://patrickdesjardins.com/blog/entity-framework-ef-modifying-an-instance-that-is-already-in-the-context
                //A way to do it is to navigate inside the local of the DbSet to see if this one is there. If the entity is present, than you detach
                var local = _context.Set<LogSosGeolocation>()
                         .Local
                         .FirstOrDefault(f => f.Id == logSosGeolocation.Id);
                if (local != null)
                {
                    _context.Entry(local).State = EntityState.Detached;
                }
                _context.Entry(logSosGeolocation).State = EntityState.Modified;
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                LogError("There is error while updating SosGeolocation: " + ex.Message, ex);
            }
        }

        public async Task DeleteAsync(LogSosGeolocation logSosGeolocation)
        {
            try
            {
                _cacheManager.Remove(String.Format(KeyForCacheYayYo, logSosGeolocation.Id));

                _context.Database.ExecuteSqlCommand("DELETE SosGeolocation WHERE Id = @p0", logSosGeolocation.Id);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                LogError("There is error while updating data: " + ex.Message, ex);
            }
        }

        #endregion

        #region sync
        public LogSosGeolocation GetById(int id)
        {
            var result = _cacheManager.Get(String.Format(KeyForCacheYayYo, id),
                () =>
                {
                    var query = "SELECT * FROM SosGeolocation WHERE Id = @p0";
                    var res = _dbSet.SqlQuery(query, id).SingleOrDefault();
                    return res;
                });

            return result;
        }

        public LogSosGeolocation GetByYayYoId(int yayYoId)
        {
            var query = "SELECT * FROM SosGeolocation WHERE YayYoId = @p0";
            var res = _dbSet.SqlQuery(query, yayYoId).SingleOrDefault();
            return res;
        }

        public void Create(LogSosGeolocation logSosGeolocation)
        {
            try
            {
                _dbSet.Add(logSosGeolocation);
                _context.SaveChanges();


            }
            catch (Exception ex)
            {
                LogError("There is error while updating SosGeolocation: " + ex.Message, ex);
            }
        }

        public void Update(LogSosGeolocation logSosGeolocation)
        {
            try
            {
                _cacheManager.Remove(String.Format(KeyForCacheYayYo, logSosGeolocation.Id));
                //ref:http://patrickdesjardins.com/blog/entity-framework-ef-modifying-an-instance-that-is-already-in-the-context
                //A way to do it is to navigate inside the local of the DbSet to see if this one is there. If the entity is present, than you detach
                var local = _context.Set<LogSosGeolocation>()
                         .Local
                         .FirstOrDefault(f => f.Id == logSosGeolocation.Id);
                if (local != null)
                {
                    _context.Entry(local).State = EntityState.Detached;
                }
                _context.Entry(logSosGeolocation).State = EntityState.Modified;
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                //Trace.TraceError("There is error while updating data: " + dex.InnerException);
                LogError("There is error while updating SosGeolocation: " + ex.Message, ex);
            }
        }

        public void Delete(LogSosGeolocation logSosGeolocation)
        {
            try
            {
                _cacheManager.Remove(String.Format(KeyForCacheYayYo, logSosGeolocation.Id));

                _context.Database.ExecuteSqlCommand("DELETE SosGeolocation WHERE Id = @p0", logSosGeolocation.Id);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                LogError("There is error while updating SosGeolocation: " + ex.Message, ex);
            }
        }

        

        #endregion
        #endregion
    }
}
