using System.Threading.Tasks;
using Core.Domain.Business;

namespace Service.Interface.Business
{

    public interface ISafetySettingService
    {
        #region async

        /// <summary>
        /// Get safetySetting by id async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<SafetySetting> GetByIdAsync(int id);
        /// <summary>
        /// Find YayYo user Id
        /// </summary>
        /// <param name="yayyoId"></param>
        /// <returns></returns>
        Task<SafetySetting> FindByYayYoId(int yayyoId);

        /// <summary>
        /// Create safetySetting async
        /// </summary>
        /// <param name="safetySetting"></param>
        /// <returns></returns>
        Task CreateAsync(SafetySetting safetySetting);

        /// <summary>
        /// Update safetySetting async
        /// </summary>
        /// <param name="safetySetting"></param>
        /// <returns></returns>
        Task UpdateAsync(SafetySetting safetySetting);

        /// <summary>
        /// Delete safetySetting async
        /// </summary>
        /// <param name="safetySetting"></param>
        /// <returns></returns>
        Task DeleteAsync(SafetySetting safetySetting);
        #endregion

        #region sync
        /// <summary>
        /// Get safetySetting by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        SafetySetting GetById(int id);
        /// <summary>
        /// Get by YayYo Id
        /// </summary>
        /// <param name="yayYoId"></param>
        /// <returns></returns>
        SafetySetting GetByYayYoId(int yayYoId);

        /// <summary>
        /// Create safetySetting
        /// </summary>
        /// <param name="safetySetting"></param>
        void Create(SafetySetting safetySetting);

        /// <summary>
        /// Update safetySetting
        /// </summary>
        /// <param name="safetySetting"></param>
        void Update(SafetySetting safetySetting);

        /// <summary>
        /// Delete safetySetting
        /// </summary>
        /// <param name="safetySetting"></param>

        void Delete(SafetySetting safetySetting);
        #endregion
    }
}
