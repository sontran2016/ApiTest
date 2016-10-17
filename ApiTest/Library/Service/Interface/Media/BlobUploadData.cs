using System;

namespace Service.Interface.Media
{
    /// <summary>
    /// represent class for blob upload
    /// </summary>
    public class BlobUploadData
    {
        /// <summary>
        /// File name
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// File url
        /// </summary>
        public string FileUrl { get; set; }

        /// <summary>
        /// Local FileName
        /// </summary>
        public string LocalFile { get; set; }

        public string LocalName { get; set; }
        /// <summary>
        /// File size in bytes
        /// </summary>
        public long FileSizeInBytes { get; set; }
        /// <summary>
        /// File size in kb
        /// </summary>
        public long FileSizeInKb => (long)Math.Ceiling((double)FileSizeInBytes / 1024);

        /// <summary>
        /// Content minetype
        /// </summary>
        public string MineType { get; set; }

        /// <summary>
        /// Success
        /// </summary>
        public bool Success { get; set; }
    }
}
