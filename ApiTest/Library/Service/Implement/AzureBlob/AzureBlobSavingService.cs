using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Common.Helpers;
using Microsoft.WindowsAzure.Storage.Blob;
using Service.Interface.Media;

namespace Service.Implement.AzureBlob
{
    /// <summary>
    /// Implement via Azure blob storage
    /// </summary>
    public class AzureBlobSavingService : IMediaService
    {
        public Task<string> SavingFileToAzureBlobAsync(byte[] bytes, string name, string contentType, CloudBlobContainer cloudBlobContainer)
        {

            //save to azure
            // Retrieve a reference to a container. 
            var container = cloudBlobContainer;

            // Create the container if it doesn't already exist.
            container.CreateIfNotExists();
            container.SetPermissions(
                new BlobContainerPermissions
                {
                    PublicAccess =
                        BlobContainerPublicAccessType.Blob
                });

            //upload file and return file detail
            var blob = container.GetBlockBlobReference(name + "." + contentType.Split('/')[1]);
            blob.Properties.ContentType = contentType;
            blob.UploadFromByteArray(bytes, 0, bytes.Length);
            return Task.FromResult(name + "." + contentType.Split('/')[1]);
        }

        public string SavingFileToAzureBlob(byte[] bytes, string name, string contentType, CloudBlobContainer cloudBlobContainer)
        {
            //save to azure
            // Retrieve a reference to a container. 
            var container = cloudBlobContainer;

            // Create the container if it doesn't already exist.
            container.CreateIfNotExists();
            container.SetPermissions(
                new BlobContainerPermissions
                {
                    PublicAccess =
                        BlobContainerPublicAccessType.Blob
                });

            //upload file and return file detail
            var blob = container.GetBlockBlobReference(name + "." + contentType.Split('/')[1]);
            blob.Properties.ContentType = contentType;
            blob.UploadFromByteArray(bytes, 0, bytes.Length);
            return name + "." + contentType.Split('/')[1];
        }
    }
}
