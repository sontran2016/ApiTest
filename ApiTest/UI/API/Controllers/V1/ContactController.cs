using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using API.Models.Contact;
using API.Models.SafetySetting;
using Core.Domain.Business;
using Service.Interface.Business;
using Swashbuckle.Swagger.Annotations;
using System.Net.Http;
using System.IO;
using ImageResizer.ExtensionMethods;
using Service.Interface.Export;
using System.Data;

namespace API.Controllers.V1
{
    /// <summary>
    /// contact
    /// </summary>
    [RoutePrefix("api/v1/contacts")]
    public class ContactController : BaseApiController
    {
        #region fields

        private readonly IContactService _contactService;
        private readonly ISafetySettingService _safetySettingService;
        private readonly IExportExel _exportExel;
        private readonly IExportPdf _exportPdf;

        #endregion

        #region ctor

        /// <summary>
        /// Contact Controller
        /// </summary>
        /// <param name="contactService"></param>
        /// <param name="safetySettingService"></param>
        /// <param name="exportExel"></param>
        /// <param name="exportPdf"></param>
        public ContactController(IContactService contactService, ISafetySettingService safetySettingService, IExportExel exportExel, IExportPdf exportPdf)
        {
            _contactService = contactService;
            _safetySettingService = safetySettingService;
            _exportExel = exportExel;
            _exportPdf = exportPdf;
        }

        #endregion

        /// <summary>
        /// Get List Contact
        /// </summary>
        /// <remarks>
        /// For native device, add "Authorization: Basic xxx" into Header of request.
        /// </remarks>
        /// <param name="yayYoId"></param>
        /// <param name="countSkip"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <param name="keySort"></param>
        /// <param name="orderDescending"></param>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        [SwaggerResponse(200, "Returns the result of get list Contact", typeof(ListContactResponseModel))]
        [SwaggerResponse(401, "Don't have permission")]
        [SwaggerResponse(500, "Internal Server Error")]
        [SwaggerResponse(400, "Bad Request")]
        public async Task<IHttpActionResult> GetListContacts(int yayYoId, int? countSkip,
            int? pageSize,
            string keyword = null,
            string keySort = null,
            bool orderDescending = false)
        {
            //prepair page size
            if (!pageSize.HasValue)
            {
                pageSize = 10; //default
            }
            else
            {
                if (pageSize.Value <= 0) pageSize = 10; //default
            }

            if (!countSkip.HasValue)
            {
                countSkip = 0; //default
            }
            else if (countSkip.Value <= 0)
            {
                countSkip = 0; //default
            }
            else
            {
                countSkip -= 1;
            }
            try
            {
                var listContact = await _contactService.GetListAsync(yayYoId, countSkip, pageSize, keyword, keySort, orderDescending);
                var record = listContact.FirstOrDefault();
                if (record == null) return
                      BadRequest("Error when get list contacts");

                var totalRecords = record.RowCounts.GetValueOrDefault();
                listContact.Remove(record);
                var res = new ListContactResponse
                {
                    TotalRecord = totalRecords,
                    Data = listContact.Select(s => new ListContactResponseModel
                    {
                        Id = s.Id,
                        FirstName = s.FirstName,
                        LastName = s.LastName,
                        Phone = s.Phone
                    }).ToList()
                };
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Create Contact.
        /// </summary>
        /// <remarks>
        /// For native device, add "Authorization: Basic xxx" into Header of request.
        /// </remarks>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("")]
        [HttpPost]
        [SwaggerResponse(200, "Returns the result of create Contact")]
        [SwaggerResponse(401, "Don't have permission")]
        [SwaggerResponse(500, "Internal Server Error")]
        [SwaggerResponse(400, "Bad Request")]
        public async Task<IHttpActionResult> CreateContact(CreateContactModel model)
        {
            try
            {
                //validate
                if (!ModelState.IsValid)
                {
                    foreach (var key in ModelState.Keys.Where(key => ModelState[key].Errors.Count > 0))
                    {
                        return BadRequest(ModelState[key].Errors[0].ErrorMessage);
                    }
                }
                var safety = await _safetySettingService.FindByYayYoId(model.YayYoId);
                if(safety == null)
                    return BadRequest("Safety setting is not existing");
                var contact = new Contact
                {
                       FirstName = model.FirstName,
                       LastName = model.LastName,
                       Phone = model.Phone,
                       SafetySettingId = safety.Id
                };
                await _contactService.CreateAsync(contact);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update Contact
        /// </summary>
        /// <remarks>
        /// For native device, add "Authorization: Basic xxx" into Header of request.
        /// </remarks>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("")]
        [HttpPut]
        [SwaggerResponse(200, "Returns the result of update Contact")]
        [SwaggerResponse(401, "Don't have permission")]
        [SwaggerResponse(500, "Internal Server Error")]
        [SwaggerResponse(400, "Bad Request")]
        public async Task<IHttpActionResult> UpdateContact(EditContactModel model)
        {
            try
            {
                //validate
                if (!ModelState.IsValid)
                {
                    foreach (var key in ModelState.Keys.Where(key => ModelState[key].Errors.Count > 0))
                    {
                        return BadRequest(ModelState[key].Errors[0].ErrorMessage);
                    }
                }
                var contact = await _contactService.GetByIdAsync(model.Id);
                if (contact == null)
                {
                    return BadRequest("Contact does not exist");
                }
                contact.FirstName = model.FirstName;
                contact.LastName = model.LastName;
                contact.Phone = model.Phone;

                await _contactService.UpdateAsync(contact);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete Contact
        /// </summary>
        /// <remarks>
        /// For native device, add "Authorization: Basic xxx" into Header of request.
        /// </remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("delete/{id}")]
        [HttpDelete]
        [SwaggerResponse(200, "Returns the result of delete Contact")]
        [SwaggerResponse(401, "Don't have permission")]
        [SwaggerResponse(500, "Internal Server Error")]
        [SwaggerResponse(400, "Bad Request")]
        public async Task<IHttpActionResult> DeleteContact(int id)
        {
            try
            {
                //validate
                var contact = await _contactService.GetByIdAsync(id);
                if (contact == null)
                {
                    return BadRequest("Contact does not exist");
                }
                await _contactService.DeleteAsync(contact);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("import")]
        [HttpPost]
        [SwaggerResponse(200, "Returns the result of import users")]
        [SwaggerResponse(401, "Don't have permission")]
        [SwaggerResponse(500, "Internal Server Error")]
        [SwaggerResponse(400, "Bad Request")]
        public async Task<IHttpActionResult> ImportContact()
        {
            try
            {
                DateTime d1, d2;
                d1 = DateTime.Now;

                var request = Request.Content;
                var data = await request.ReadAsMultipartAsync();
                using (var file = data.Contents[0])
                {
                    if (file.Headers.ContentLength > 0 &&
                        (file.Headers.ContentType.ToString() == "application/vnd.ms-excel"))
                    {
                        var stream = await file.ReadAsStreamAsync();
                        var fileName = file.Headers.ContentDisposition.FileName.Replace("\"", "");
                        var memoryStream = new MemoryStream(stream.CopyToBytes());
                        var success = await _contactService.ImportContactAsyn(memoryStream, fileName);
                        if(!success)
                            return BadRequest("There is an eror occur while importing contact");
                    }
                    //file.Dispose();
                }
                d2 = DateTime.Now;
                var t= d2 - d1;
                var times = string.Format("Time {0} minutes, seconds: {1}", t.Minutes, t.Seconds);
                return Ok(times);
            }
            catch
            {
                return BadRequest("There is an eror occur while importing contact");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("ExportExcel")]
        [HttpGet]
        public async Task<IHttpActionResult> ExportExcel()  //allow option column Sum
        {
            try
            {
                var list = await _contactService.GetListAsync(1, 0, 10, null, null, false);
                if (list.Count > 0) list.Remove(list.FirstOrDefault());
                
                var dt = new DataTable("Contact List");
                dt.Columns.AddRange(new DataColumn[] { new DataColumn("Id",typeof(int)), new DataColumn("FirstName"), new DataColumn("LastName"), new DataColumn("Phone") });
                foreach (var p in list)
                    dt.Rows.Add(p.Id, p.FirstName,p.LastName,p.Phone);

                var ds = new DataSet();
                ds.Tables.Add(dt);
                var optionColumnSum = new List<string>() {"Id"};
                var bytes = await _exportExel.GenerateExcel2010(ds, optionColumnSum);
                //return Ok(bytes);

                //just for test
                var path = @"C:\SonTran\ApiTestGit2\ApiTest\ApiTest\UI\API\App_Data\Export2.xlsx";
                File.WriteAllBytes(path, bytes);
                return Ok();
            }
            catch
            {
                return BadRequest("There is an eror occur while export contact");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("ExportPdf")]
        [HttpGet]
        public async Task<IHttpActionResult> ExportPdf()
        {
            try
            {
                var list = await _contactService.GetListAsync(1, 0, 10, null, null, false);
                if (list.Count > 0) list.Remove(list.FirstOrDefault());

                var dt = new DataTable("Contact List");
                dt.Columns.AddRange(new DataColumn[] { new DataColumn("Id"), new DataColumn("FirstName"), new DataColumn("LastName"), new DataColumn("Phone") });
                foreach (var p in list)
                    dt.Rows.Add(p.Id, p.FirstName, p.LastName, p.Phone);

                var ds = new DataSet();
                ds.Tables.Add(dt);
                var bytes = await _exportPdf.GeneratePdf(dt);
                //return Ok(bytes);

                //just for test
                var path = @"C:\SonTran\ApiTestGit2\ApiTest\ApiTest\UI\API\App_Data\Export2.pdf";
                File.WriteAllBytes(path, bytes);
                return Ok();
            }
            catch
            {
                return BadRequest("There is an eror occur while export contact");
            }
        }
    }
}
