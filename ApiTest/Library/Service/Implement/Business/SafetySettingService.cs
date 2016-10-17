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
    /// <summary>
    /// Saftety Setting Service
    /// </summary>
    public class SafetySettingService : BaseServiceWithLogging, ISafetySettingService
    {
        #region field
        private readonly DbContext _context;
        private readonly DbSet<SafetySetting> _dbSet;
        private readonly ICacheManager _cacheManager;
        private const string KeyForCacheYayYo = "YayYo.SafetySetting.Id.{0}";
        #endregion
        #region ctor
        /// <summary>
        /// ctr
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cacheManager"></param>
        /// <param name="noisLoggingService"></param>
        public SafetySettingService(DbContext context, ICacheManager cacheManager, INoisLoggingService noisLoggingService) : base(noisLoggingService)
        {
            _context = context;
            _cacheManager = cacheManager;
            _dbSet = _context.Set<SafetySetting>();

        }
        #endregion
        #region public method
        #region async
        public async Task<SafetySetting> GetByIdAsync(int id)
        {

            var result = _cacheManager.Get(String.Format(KeyForCacheYayYo, id),
                () =>
                {
                    var query = "SELECT * FROM SafetySetting WHERE Id = @p0";
                    var res = _dbSet.SqlQuery(query, id).SingleOrDefault();
                    return res;
                });

            return await Task.FromResult(result);
        }

        public async Task<SafetySetting> FindByYayYoId(int yayyoId)
        {
            var query = "SELECT * FROM SafetySetting WHERE YayYoId = @p0";
            var res = _dbSet.SqlQuery(query, yayyoId).SingleOrDefault();
            return await Task.FromResult(res);            
        }

        public async Task CreateAsync(SafetySetting safetySetting)
        {
            try
            {
                _dbSet.Add(safetySetting);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                LogError("There is error while updating SafetySetting: " + ex.Message, ex);
            }

        }

        public async Task UpdateAsync(SafetySetting safetySetting)
        {
            try
            {
                _cacheManager.Remove(String.Format(KeyForCacheYayYo, safetySetting.Id));
                //ref:http://patrickdesjardins.com/blog/entity-framework-ef-modifying-an-instance-that-is-already-in-the-context
                //A way to do it is to navigate inside the local of the DbSet to see if this one is there. If the entity is present, than you detach
                var local = _context.Set<SafetySetting>()
                         .Local
                         .FirstOrDefault(f => f.Id == safetySetting.Id);
                if (local != null)
                {
                    _context.Entry(local).State = EntityState.Detached;
                }
                _context.Entry(safetySetting).State = EntityState.Modified;
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                LogError("There is error while updating SafetySetting: " + ex.Message, ex);
            }
        }

        public async Task DeleteAsync(SafetySetting safetySetting)
        {
            try
            {
                _cacheManager.Remove(String.Format(KeyForCacheYayYo, safetySetting.Id));

                _context.Database.ExecuteSqlCommand("DELETE SafetySetting WHERE Id = @p0", safetySetting.Id);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                LogError("There is error while updating data: " + ex.Message, ex);
            }
        }

        #endregion

        #region sync
        public SafetySetting GetById(int id)
        {
            var result = _cacheManager.Get(String.Format(KeyForCacheYayYo, id),
                () =>
                {
                    var query = "SELECT * FROM SafetySetting WHERE Id = @p0";
                    var res = _dbSet.SqlQuery(query, id).SingleOrDefault();
                    return res;
                });

            return result;
        }

        public SafetySetting GetByYayYoId(int yayYoId)
        {
            var query = "SELECT * FROM SafetySetting WHERE YayYoId = @p0";
            var res = _dbSet.SqlQuery(query, yayYoId).SingleOrDefault();
            return res;
        }

        public void Create(SafetySetting safetySetting)
        {
            try
            {
                _dbSet.Add(safetySetting);
                _context.SaveChanges();


            }
            catch (Exception ex)
            {
                LogError("There is error while updating SafetySetting: " + ex.Message, ex);
            }
        }

        public void Update(SafetySetting safetySetting)
        {
            try
            {
                _cacheManager.Remove(String.Format(KeyForCacheYayYo, safetySetting.Id));
                //ref:http://patrickdesjardins.com/blog/entity-framework-ef-modifying-an-instance-that-is-already-in-the-context
                //A way to do it is to navigate inside the local of the DbSet to see if this one is there. If the entity is present, than you detach
                var local = _context.Set<SafetySetting>()
                         .Local
                         .FirstOrDefault(f => f.Id == safetySetting.Id);
                if (local != null)
                {
                    _context.Entry(local).State = EntityState.Detached;
                }
                _context.Entry(safetySetting).State = EntityState.Modified;
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                //Trace.TraceError("There is error while updating data: " + dex.InnerException);
                LogError("There is error while updating SafetySetting: " + ex.Message, ex);
            }
        }

        public void Delete(SafetySetting safetySetting)
        {
            try
            {
                _cacheManager.Remove(String.Format(KeyForCacheYayYo, safetySetting.Id));

                _context.Database.ExecuteSqlCommand("DELETE SafetySetting WHERE Id = @p0", safetySetting.Id);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                LogError("There is error while updating SafetySetting: " + ex.Message, ex);
            }
        }

        #endregion
        #endregion
    }
}
