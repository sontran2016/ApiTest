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
    public class YayYoApplicationService : BaseServiceWithLogging, IYayYoApplicationService
    {
        #region field
        private readonly DbContext _context;
        private readonly DbSet<YayYoApplication> _dbSet;
        private readonly ICacheManager _cacheManager;
        private const string KeyForCacheYayYo = "YayYo.Application.Id.{0}";
        #endregion
        #region ctor
        /// <summary>
        /// ctr
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cacheManager"></param>
        /// <param name="noisLoggingService"></param>
        public YayYoApplicationService(DbContext context, ICacheManager cacheManager, INoisLoggingService noisLoggingService) : base(noisLoggingService)
        {
            _context = context;
            _cacheManager = cacheManager;
            _dbSet = _context.Set<YayYoApplication>();

        }
        #endregion
        #region public method
        #region async
        public async Task<YayYoApplication> GetByIdAsync(int id)
        {

            var result = _cacheManager.Get(String.Format(KeyForCacheYayYo, id),
                () =>
                {
                    var query = "SELECT * FROM YayYoApplication WHERE Id = @p0";
                    var res = _dbSet.SqlQuery(query, id).SingleOrDefault();
                    return res;
                });

            return await Task.FromResult(result);
        }

        public async Task CreateAsync(YayYoApplication yayYoApplication)
        {
            try
            {
                _dbSet.Add(yayYoApplication);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                LogError("There is error while updating YayYoApplication: " + ex.Message, ex);
            }

        }

        public async Task UpdateAsync(YayYoApplication yayYoApplication)
        {
            try
            {
                _cacheManager.Remove(String.Format(KeyForCacheYayYo, yayYoApplication.Id));
                //ref:http://patrickdesjardins.com/blog/entity-framework-ef-modifying-an-instance-that-is-already-in-the-context
                //A way to do it is to navigate inside the local of the DbSet to see if this one is there. If the entity is present, than you detach
                var local = _context.Set<YayYoApplication>()
                         .Local
                         .FirstOrDefault(f => f.Id == yayYoApplication.Id);
                if (local != null)
                {
                    _context.Entry(local).State = EntityState.Detached;
                }
                _context.Entry(yayYoApplication).State = EntityState.Modified;
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                LogError("There is error while updating YayYoApplication: " + ex.Message, ex);
            }
        }

        public async Task DeleteAsync(YayYoApplication yayYoApplication)
        {
            try
            {
                _cacheManager.Remove(String.Format(KeyForCacheYayYo, yayYoApplication.Id));

                _context.Database.ExecuteSqlCommand("DELETE YayYoApplication WHERE Id = @p0", yayYoApplication.Id);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                LogError("There is error while updating data: " + ex.Message, ex);
            }
        }

        #endregion

        #region sync
        public YayYoApplication GetById(int id)
        {
            var result = _cacheManager.Get(String.Format(KeyForCacheYayYo, id),
                () =>
                {
                    var query = "SELECT * FROM YayYoApplication WHERE Id = @p0";
                    var res = _dbSet.SqlQuery(query, id).SingleOrDefault();
                    return res;
                });

            return result;
        }

        public void Create(YayYoApplication yayYoApplication)
        {
            try
            {
                _dbSet.Add(yayYoApplication);
                _context.SaveChanges();


            }
            catch (Exception ex)
            {
                LogError("There is error while updating YayYoApplication: " + ex.Message, ex);
            }
        }

        public void Update(YayYoApplication yayYoApplication)
        {
            try
            {
                _cacheManager.Remove(String.Format(KeyForCacheYayYo, yayYoApplication.Id));
                //ref:http://patrickdesjardins.com/blog/entity-framework-ef-modifying-an-instance-that-is-already-in-the-context
                //A way to do it is to navigate inside the local of the DbSet to see if this one is there. If the entity is present, than you detach
                var local = _context.Set<YayYoApplication>()
                         .Local
                         .FirstOrDefault(f => f.Id == yayYoApplication.Id);
                if (local != null)
                {
                    _context.Entry(local).State = EntityState.Detached;
                }
                _context.Entry(yayYoApplication).State = EntityState.Modified;
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                //Trace.TraceError("There is error while updating data: " + dex.InnerException);
                LogError("There is error while updating YayYoApplication: " + ex.Message, ex);
            }
        }

        public void Delete(YayYoApplication yayYoApplication)
        {
            try
            {
                _cacheManager.Remove(String.Format(KeyForCacheYayYo, yayYoApplication.Id));

                _context.Database.ExecuteSqlCommand("DELETE YayYoApplication WHERE Id = @p0", yayYoApplication.Id);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                LogError("There is error while updating YayYoApplication: " + ex.Message, ex);
            }
        }

        public List<YayYoApplication> GetAllApplications()
        {
            // Create and execute raw SQL query.
            var query = "SELECT * FROM YayYoApplication";
            var result = _context.Database.SqlQuery<YayYoApplication>(query);
            return result.ToList();
        }

        #endregion
        #endregion
    }
}
