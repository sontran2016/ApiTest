using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Service.Interface.Media
{
    /// <summary>
    /// Interface for process media 
    /// </summary>
    public interface IMediaService
    {
        #region async method
        /// <summary>
        /// SavingFileToAzureBlobAsync
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="name"></param>
        /// <param name="contentType"></param>
        /// <param name="cloudBlobContainer"></param>
        /// <returns></returns>
        Task<string> SavingFileToAzureBlobAsync(byte[] bytes, string name, string contentType, CloudBlobContainer cloudBlobContainer);
        #endregion

        #region sync method
        /// <summary>
        /// SavingFileToAzureBlobAsync
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="name"></param>
        /// <param name="contentType"></param>
        /// <param name="cloudBlobContainer"></param>
        /// <returns></returns>
        string SavingFileToAzureBlob(byte[] bytes, string name, string contentType, CloudBlobContainer cloudBlobContainer);
        #endregion
    }
}
