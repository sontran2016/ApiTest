using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Common.Logs;
using Core.Domain.Business;
using Service.CachingLayer;
using Service.Interface.Business;
using Service.Models.LogRideInformation;

namespace Service.Implement.Business
{
    /// <summary>
    /// Saftety Setting Service
    /// </summary>
    public class LogRideInformationService : BaseServiceWithLogging, ILogRideInformationService
    {
        #region field
        private readonly DbContext _context;
        private readonly DbSet<LogRideInformation> _dbSet;
        private readonly ICacheManager _cacheManager;
        private const string KeyForCacheYayYo = "YayYo.LogRideInformation.Id.{0}";
        #endregion
        #region ctor

        /// <summary>
        /// ctr
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cacheManager"></param>
        /// <param name="noisLoggingService"></param>
        public LogRideInformationService(DbContext context, ICacheManager cacheManager, INoisLoggingService noisLoggingService) : base(noisLoggingService)
        {
            _context = context;
            _cacheManager = cacheManager;
            _dbSet = _context.Set<LogRideInformation>();

        }
        #endregion
        #region public method
        #region async
        public async Task<LogRideInformation> GetByIdAsync(int id)
        {

            var result = _cacheManager.Get(String.Format(KeyForCacheYayYo, id),
                () =>
                {
                    var query = "SELECT * FROM LogRideInformation WHERE Id = @p0";
                    var res = _dbSet.SqlQuery(query, id).SingleOrDefault();
                    return res;
                });

            return await Task.FromResult(result);
        }

        public async Task CreateAsync(LogRideInformation logRideInformation)
        {
            try
            {
                _dbSet.Add(logRideInformation);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                LogError("There is error while updating LogRideInformation: " + ex.Message, ex);
            }

        }

        public async Task UpdateAsync(LogRideInformation logRideInformation)
        {
            try
            {
                _cacheManager.Remove(String.Format(KeyForCacheYayYo, logRideInformation.Id));
                //ref:http://patrickdesjardins.com/blog/entity-framework-ef-modifying-an-instance-that-is-already-in-the-context
                //A way to do it is to navigate inside the local of the DbSet to see if this one is there. If the entity is present, than you detach
                var local = _context.Set<LogRideInformation>()
                         .Local
                         .FirstOrDefault(f => f.Id == logRideInformation.Id);
                if (local != null)
                {
                    _context.Entry(local).State = EntityState.Detached;
                }
                _context.Entry(logRideInformation).State = EntityState.Modified;
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                LogError("There is error while updating LogRideInformation: " + ex.Message, ex);
            }
        }

        public async Task DeleteAsync(LogRideInformation logRideInformation)
        {
            try
            {
                _cacheManager.Remove(String.Format(KeyForCacheYayYo, logRideInformation.Id));

                _context.Database.ExecuteSqlCommand("DELETE LogRideInformation WHERE Id = @p0", logRideInformation.Id);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                LogError("There is error while updating data: " + ex.Message, ex);
            }
        }

        #endregion

        #region sync
        public LogRideInformation GetById(int id)
        {
            var result = _cacheManager.Get(String.Format(KeyForCacheYayYo, id),
                () =>
                {
                    var query = "SELECT * FROM LogRideInformation WHERE Id = @p0";
                    var res = _dbSet.SqlQuery(query, id).SingleOrDefault();
                    return res;
                });

            return result;
        }

        public LogRideInformation GetByYayYoId(int yayYoId)
        {
            var query = "SELECT * FROM LogRideInformation WHERE YayYoId = @p0";
            var res = _dbSet.SqlQuery(query, yayYoId).SingleOrDefault();
            return res;
        }

        public void Create(LogRideInformation logRideInformation)
        {
            try
            {
                _dbSet.Add(logRideInformation);
                _context.SaveChanges();


            }
            catch (Exception ex)
            {
                LogError("There is error while updating LogRideInformation: " + ex.Message, ex);
            }
        }

        public void Update(LogRideInformation logRideInformation)
        {
            try
            {
                _cacheManager.Remove(String.Format(KeyForCacheYayYo, logRideInformation.Id));
                //ref:http://patrickdesjardins.com/blog/entity-framework-ef-modifying-an-instance-that-is-already-in-the-context
                //A way to do it is to navigate inside the local of the DbSet to see if this one is there. If the entity is present, than you detach
                var local = _context.Set<LogRideInformation>()
                         .Local
                         .FirstOrDefault(f => f.Id == logRideInformation.Id);
                if (local != null)
                {
                    _context.Entry(local).State = EntityState.Detached;
                }
                _context.Entry(logRideInformation).State = EntityState.Modified;
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                //Trace.TraceError("There is error while updating data: " + dex.InnerException);
                LogError("There is error while updating LogRideInformation: " + ex.Message, ex);
            }
        }

        public void Delete(LogRideInformation logRideInformation)
        {
            try
            {
                _cacheManager.Remove(String.Format(KeyForCacheYayYo, logRideInformation.Id));

                _context.Database.ExecuteSqlCommand("DELETE LogRideInformation WHERE Id = @p0", logRideInformation.Id);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                LogError("There is error while updating LogRideInformation: " + ex.Message, ex);
            }
        }

        public LogRideInformation GetLastestLogRideInformation(int yayYoId)
        {
            var query = "SELECT top 1 ri.*   FROM [dbo].[LogRideInformation] ri inner join SafetySetting s on ri.SafetySettingId = s.Id where YayYoId = @p0 order by ri.Id desc";
            var res = _dbSet.SqlQuery(query, yayYoId).SingleOrDefault();
            return res;
        }

        public List<UserRideIdModel> GetRideBookId()
        {
            var query = "Select s.YayYoId from LogRideInformation ri inner join SafetySetting s on ri.SafetySettingId = s.Id where s.Active = 1 and ri.RideBookId = 0 and DATEADD(day, 1, ri.CreatedDateUtc) > GETUTCDATE() group by s.YayYoId";
            var res = _context.Database.SqlQuery<UserRideIdModel>(query).ToList();
            return res;
        }       

        #endregion
        #endregion
    }
}
