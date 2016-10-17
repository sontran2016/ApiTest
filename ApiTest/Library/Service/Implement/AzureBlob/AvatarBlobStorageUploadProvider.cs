using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Service.Interface.Media;

namespace Service.Implement.AzureBlob
{
    /// <summary>
    /// Avatar provider, use only 1 image at same time
    /// </summary>
    public class AvatarBlobStorageUploadProvider : MultipartFileStreamProvider
    {
        public List<BlobUploadData> FileUploads { get; set; }
        public string DocumentId { get; set; }

        public UploadMimeType UploadMimeType { get; set; }

        public AvatarBlobStorageUploadProvider()
            : base(Path.GetTempPath())
        {
            FileUploads = new List<BlobUploadData>();
        }

        private string _CorrectFileName(string input)
        {
            return Regex.Replace(input, @"[^a-z0-9-_\.]", String.Empty, RegexOptions.IgnoreCase);
        }

        public override Task ExecutePostProcessingAsync()
        {
            // NOTE: FileData is a property of MultipartFileStreamProvider and is a list of multipart
            // files that have been uploaded and saved to disk in the Path.GetTempPath() location.
            var blobUploadList = new List<BlobUploadData>();
            foreach (var file in FileData)
            {
                if (file != null && file.Headers.ContentDisposition.Name.IndexOf("file", StringComparison.Ordinal) > -1)
                {

                    bool validateFile = false;
                    if (UploadMimeType == UploadMimeType.Image)
                    {
                        //compare contain image
                        if (file.Headers.ContentType != null && file.Headers.ContentType.MediaType.Contains("image"))
                        {
                            validateFile = true;
                        }

                    }
                    if (validateFile)
                    {
                        var blobUpload = new BlobUploadData
                        {
                            FileName = Guid.NewGuid() + "_" + _CorrectFileName(file.Headers.ContentDisposition.FileName.Replace("\"", "")),
                            MineType = file.Headers.ContentType.MediaType,
                            LocalFile = file.LocalFileName,
                            LocalName = file.Headers.ContentDisposition.FileName,
                            Success = true
                        };
                        // save uploaded blob as return upload
                        blobUploadList.Add(blobUpload);
                    }
                    else
                    {
                        if (file.Headers.ContentType != null)
                        {
                            var blobUpload = new BlobUploadData
                            {
                                FileName = Guid.NewGuid() + "_" + _CorrectFileName(file.Headers.ContentDisposition.FileName.Replace("\"", "")),
                                MineType = file.Headers.ContentType.MediaType,
                                LocalFile = file.LocalFileName,
                                LocalName = file.Headers.ContentDisposition.FileName,
                                Success = true
                            };
                            blobUploadList.Add(blobUpload);
                        }
                    }

                }
            }
            FileUploads = blobUploadList;
            return base.ExecutePostProcessingAsync();
        }
    }

    public enum UploadMimeType
    {
        Image = 1,
        File = 2

    }
}
