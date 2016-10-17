using System;
using System.Configuration;
using System.Globalization;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using Microsoft.WindowsAzure.Storage.Table;

namespace Common.Helpers
{
    public static class AzureHelper
    {

        #region table client

        private static CloudTableClient _cloudTableClient;

        public static CloudTableClient CloudTableClient
        {
            get
            {
                if (_cloudTableClient == null)
                {
                    var blobStorageConnectionString = ConfigurationManager.AppSettings["BlobStorageConnectionString"];
                    // Create blob client and return reference to the container
                    var blobStorageAccount = CloudStorageAccount.Parse(blobStorageConnectionString);
                    _cloudTableClient = blobStorageAccount.CreateCloudTableClient();
                }

                return _cloudTableClient;
            }


        }

        #endregion

        #region blob client

        private static CloudBlobClient _cloudBlobClient;

        public static CloudBlobClient CloudBlobClient
        {
            get
            {
                if (_cloudBlobClient == null)
                {
                    var blobStorageConnectionString = ConfigurationManager.AppSettings["BlobStorageConnectionString"];
                    // Create blob client and return reference to the container
                    var blobStorageAccount = CloudStorageAccount.Parse(blobStorageConnectionString);
                    _cloudBlobClient = blobStorageAccount.CreateCloudBlobClient();
                }

                return _cloudBlobClient;
            }


        }

       
        #endregion

        #region queue Client
        private static CloudQueueClient _cloudQueueClient;

        public static CloudQueueClient CloudQueueClient
        {
            get
            {
                if (_cloudQueueClient == null)
                {
                    var blobStorageConnectionString = ConfigurationManager.AppSettings["BlobStorageConnectionString"];
                    // Create blob client and return reference to the container
                    var blobStorageAccount = CloudStorageAccount.Parse(blobStorageConnectionString);
                    _cloudQueueClient = blobStorageAccount.CreateCloudQueueClient();
                    _cloudQueueClient.DefaultRequestOptions.RetryPolicy = new LinearRetry(TimeSpan.FromSeconds(3), 3);

                }

                return _cloudQueueClient;
            }
        }

        #endregion

        #region container
        /// <summary>
        /// avatar container, need to public
        /// </summary>
        private static CloudBlobContainer _avatarBlobContainer;

        /// <summary>
        /// avatar container, public
        /// </summary>
        public static CloudBlobContainer AvatarBlobContainer
        {
            get
            {
                if (_avatarBlobContainer == null)
                {
                    var avatarFolderName = ConfigurationManager.AppSettings["AvatarFolderName"];
                    // Retrieve a reference to a container. 
                    _avatarBlobContainer = CloudBlobClient.GetContainerReference(avatarFolderName);
                    // Create the container if it doesn't already exist.
                    _avatarBlobContainer.CreateIfNotExists();
                    _avatarBlobContainer.SetPermissions(
                        new BlobContainerPermissions
                        {
                            PublicAccess =
                                BlobContainerPublicAccessType.Blob
                        });
                }

                return _avatarBlobContainer;
            }


        }

        


        /// <summary>
        /// Generate download url for blob with permission
        /// </summary>
        /// <param name="blob">blob need to generate url</param>
        /// <param name="permission">permission like read, write, list</param>
        /// <param name="sasMinutesValid">time for expired link</param>
        /// <returns></returns>
        public static string GetDownloadLink(this CloudBlockBlob blob, SharedAccessBlobPermissions permission,
            int sasMinutesValid)
        {
            var sasToken = blob.GetSharedAccessSignature(new SharedAccessBlobPolicy()
            {
                Permissions = permission,
                SharedAccessStartTime = DateTime.UtcNow.AddMinutes(-15),
                SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(sasMinutesValid),
            });
            return string.Format(CultureInfo.InvariantCulture, "{0}{1}", blob.Uri, sasToken);
        }



        #endregion
    }
}
