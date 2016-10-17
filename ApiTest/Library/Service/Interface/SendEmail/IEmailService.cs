using System.Collections.Generic;
using System.Threading.Tasks;
using Service.Models.SendgridEmail;

namespace Service.Interface.SendEmail
{
    public interface IEmailService
    {
        /// <summary>
        /// SendEmailAsync
        /// </summary>
        /// <param name="to"></param>
        /// <param name="bcc"></param>
        /// <param name="cc"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        Task SendEmailAsync(List<string> to, string bcc, string cc, string subject, string body);

        /// <summary>
        /// SendEmailWithAttachmentsAsync
        /// </summary>
        /// <param name="to"></param>
        /// <param name="bcc"></param>
        /// <param name="cc"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="attachments"></param>
        /// <returns></returns>
        Task SendEmailWithAttachmentsAsync(List<string> to, string bcc, string cc, string subject, string body, List<AttachmentModel> attachments);

        /// <summary>
        /// GetMailTemplate
        /// </summary>
        /// <param name="templateName"></param>
        /// <returns></returns>
        string GetMailTemplate(string templateName);

        /// <summary>
        /// SendEmail
        /// </summary>
        /// <param name="to"></param>
        /// <param name="bcc"></param>
        /// <param name="cc"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        void SendEmail(List<string> to, string bcc, string cc, string subject, string body);

        //List<AttachmentModel> ConvertAttachmentToStream(List<AttachmentRequestModel> attachments);
        List<string> GetEmailAddress(List<string> groups);
    }
}
