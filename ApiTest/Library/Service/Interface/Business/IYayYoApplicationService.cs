using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Domain.Business;

namespace Service.Interface.Business
{
    public interface IYayYoApplicationService
    {
        #region async

        /// <summary>
        /// Get application by id async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<YayYoApplication> GetByIdAsync(int id);

        /// <summary>
        /// Create application async
        /// </summary>
        /// <param name="yayYoApplication"></param>
        /// <returns></returns>
        Task CreateAsync(YayYoApplication yayYoApplication);

        /// <summary>
        /// Update application async
        /// </summary>
        /// <param name="yayYoApplication"></param>
        /// <returns></returns>
        Task UpdateAsync(YayYoApplication yayYoApplication);

        /// <summary>
        /// Delete application async
        /// </summary>
        /// <param name="yayYoApplication"></param>
        /// <returns></returns>
        Task DeleteAsync(YayYoApplication yayYoApplication);
        #endregion

        #region sync
        /// <summary>
        /// Get application by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        YayYoApplication GetById(int id);

        /// <summary>
        /// Create application
        /// </summary>
        /// <param name="yayYoApplication"></param>
        void Create(YayYoApplication yayYoApplication);

        /// <summary>
        /// Update application
        /// </summary>
        /// <param name="yayYoApplication"></param>
        void Update(YayYoApplication yayYoApplication);

        /// <summary>
        /// Delete application
        /// </summary>
        /// <param name="yayYoApplication"></param>

        void Delete(YayYoApplication yayYoApplication);

        /// <summary>
        /// GetAllApplications
        /// </summary>
        /// <returns></returns>
        List<YayYoApplication> GetAllApplications();
        #endregion
    }
}
