using System.IO;

namespace Service.Models.SendgridEmail
{
    public class AttachmentModel
    {
        public Stream Stream { get; set; }
        public string Name { get; set; }
    }

    public class AttachmentRequestModel
    {
        public string DataBase64String { get; set; }
        public string Name { get; set; }
    }
}
