using API.Models.SendgridEmail;
using ImageResizer.ExtensionMethods;
using Service.Interface.SendEmail;
using Service.Models.SendgridEmail;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace API.Controllers.V1
{
    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("api/v1/email")]
    public class EmailController : BaseApiController
    {
        private readonly IEmailService _emailService;

        /// <summary>
        /// Email Controller
        /// </summary>
        /// <param name="emailService"></param>
        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        /// <summary>
        /// Send email with attachments
        /// </summary>
        [Route("")]
        [HttpPost]
        [SwaggerResponse(200, "Returns the result of send email")]
        [SwaggerResponse(401, "Don't have permission")]
        [SwaggerResponse(500, "Internal Server Error")]
        [SwaggerResponse(400, "Bad Request")]        
        public async Task<IHttpActionResult> SendEmailWithAttachmentsAsync(string to,string cc, string bcc, string subject, string body)   //can not using model when using attachment
        {
            //email to sprate by , or ;
            //can attach multiple file
            try
            {
                var listTo = new List<string>();
                if(to.Contains(","))
                    listTo = to.Split(',').ToList();
                else if (to.Contains(";"))
                    listTo = to.Split(';').ToList();
                else
                    listTo.Add(to);
                if (listTo.Count == 0) return BadRequest("Email address is required");

                var request = Request.Content;
                var data = await request.ReadAsMultipartAsync();
                var attachments = new List<AttachmentModel>();
                for (var i = 0; i < data.Contents.Count; i++)
                {
                    using (var file = data.Contents[i])
                    {
                        if (file.Headers.ContentLength > 0)
                        {
                            var stream = await file.ReadAsStreamAsync();
                            var fileName = file.Headers.ContentDisposition.FileName.Replace("\"", "");
                            var memoryStream = new MemoryStream(stream.CopyToBytes());
                            attachments.Add(new AttachmentModel { Name = fileName, Stream = memoryStream });
                        }
                    }
                }                
                await _emailService.SendEmailWithAttachmentsAsync(listTo, bcc, cc, subject, body, attachments);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Send email to group
        /// </summary>
        /// <param name="to"></param>
        /// <param name="cc"></param>
        /// <param name="bcc"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        [Route("group")]
        [HttpPost]
        [SwaggerResponse(200, "Returns the result of send email")]
        [SwaggerResponse(401, "Don't have permission")]
        [SwaggerResponse(500, "Internal Server Error")]
        [SwaggerResponse(400, "Bad Request")]        
        public async Task<IHttpActionResult> SendEmailGroupAsync(string to, string cc, string bcc, string subject, string body)   //can not using model when using attachment
        {
            //group to sprate by , or ;
            //can attach multiple file
            try
            {
                var listGroup = new List<string>();
                if (to.Contains(","))
                    listGroup = to.Split(',').ToList();
                else if (to.Contains(";"))
                    listGroup = to.Split(';').ToList();
                else
                    listGroup.Add(to);
                if (listGroup.Count == 0) return BadRequest("Email address is required");
                var emails = _emailService.GetEmailAddress(listGroup);
                if (emails.Count == 0) return BadRequest("Not existing an email");

                var request = Request.Content;
                var data = await request.ReadAsMultipartAsync();
                var attachments = new List<AttachmentModel>();
                for (var i = 0; i < data.Contents.Count; i++)
                {
                    using (var file = data.Contents[i])
                    {
                        if (file.Headers.ContentLength > 0)
                        {
                            var stream = await file.ReadAsStreamAsync();
                            var fileName = file.Headers.ContentDisposition.FileName.Replace("\"", "");
                            var memoryStream = new MemoryStream(stream.CopyToBytes());
                            attachments.Add(new AttachmentModel { Name = fileName, Stream = memoryStream });
                        }
                    }
                }
                await _emailService.SendEmailWithAttachmentsAsync(emails, bcc, cc, subject, body, attachments);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}