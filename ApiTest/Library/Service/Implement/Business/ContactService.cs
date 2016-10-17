using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Common.Helpers;
using Common.Logs;
using Core.Domain.Business;
using Service.CachingLayer;
using Service.Interface.Business;
using Service.Models.ContactModel;
using CsvHelper;
using AutoMapper;
using System.Text;

namespace Service.Implement.Business
{

    public class ContactService : BaseServiceWithLogging, IContactService
    {
        #region field
        private readonly DbContext _context;
        private readonly DbSet<Contact> _dbSet;
        private readonly ICacheManager _cacheManager;
        private const string KeyForCacheYayYo = "YayYo.Contact.Id.{0}";
        #endregion
        #region ctor
        /// <summary>
        /// ctr
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cacheManager"></param>
        /// <param name="noisLoggingService"></param>
        public ContactService(DbContext context, ICacheManager cacheManager, INoisLoggingService noisLoggingService) : base(noisLoggingService)
        {
            _context = context;
            _cacheManager = cacheManager;
            _dbSet = _context.Set<Contact>();

        }
        #endregion
        #region public method
        #region async
        public async Task<Contact> GetByIdAsync(int id)
        {

            var result = _cacheManager.Get(String.Format(KeyForCacheYayYo, id),
                () =>
                {
                    var query = "SELECT * FROM Contact WHERE Id = @p0";
                    var res = _dbSet.SqlQuery(query, id).SingleOrDefault();
                    return res;
                });

            return await Task.FromResult(result);
        }

        public Task<List<Contact>> GetBySafetySettingIdAsync(int safetySettingId)
        {
            var query = "SELECT * FROM Contact WHERE SafetySettingId = @p0";
            var res = _dbSet.SqlQuery(query, safetySettingId).ToListAsync();
            return res;
        }

        public async Task CreateAsync(Contact contact)
        {
            try
            {
                _dbSet.Add(contact);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                LogError("There is error while updating Contact: " + ex.Message, ex);
            }

        }

        public async Task UpdateAsync(Contact contact)
        {
            try
            {
                _cacheManager.Remove(String.Format(KeyForCacheYayYo, contact.Id));
                //ref:http://patrickdesjardins.com/blog/entity-framework-ef-modifying-an-instance-that-is-already-in-the-context
                //A way to do it is to navigate inside the local of the DbSet to see if this one is there. If the entity is present, than you detach
                var local = _context.Set<Contact>()
                         .Local
                         .FirstOrDefault(f => f.Id == contact.Id);
                if (local != null)
                {
                    _context.Entry(local).State = EntityState.Detached;
                }
                _context.Entry(contact).State = EntityState.Modified;
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                LogError("There is error while updating Contact: " + ex.Message, ex);
            }
        }

        public async Task DeleteAsync(Contact contact)
        {
            try
            {
                _cacheManager.Remove(String.Format(KeyForCacheYayYo, contact.Id));

                _context.Database.ExecuteSqlCommand("DELETE Contact WHERE Id = @p0", contact.Id);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                LogError("There is error while updating data: " + ex.Message, ex);
            }
        }

        public Task<List<ContactSearchModel>> GetListAsync(int yayYoId, int? countSkip, int? pageSize, string keyword, string keySort, bool orderDescending)
        {
            const string sql = @"Contact_GetList @p0, @p1, @p2, @p3, @p4, @p5";
            var result = _context.Database.SqlQuery<ContactSearchModel>(sql,
                countSkip,
                pageSize,
                keyword.RemoveApostrophe(),
                keySort.RemoveApostrophe(),
                orderDescending,
                yayYoId).ToListAsync();
            return result;
        }

        public Task ImportContactAsyn(Stream data, string fileName)
        {
            try
            {
                //read file from stream
                var stream = new StreamReader(data);
                var csv = new CsvReader(stream);
                csv.Configuration.RegisterClassMap<ImportContactModelMap>();
                var records = csv.GetRecords<ExcelInfo>().ToList();

                DbSet<ExcelInfo> dbSetExcelInfo = _context.Set<ExcelInfo>();
                var list = dbSetExcelInfo.ToList();
                var sqlQueryStringBuffer = new StringBuilder();
                var queryInsert = "INSERT dbo.ExcelInfo (PartnerID,Style,PartnerSKU,UPC,[Description],ColorCode,ColorDesc,SizeCode,SizeDescription,SizeClassDescription,WeightLBS,PreviewImageURL,CreatedOnUtc) VALUES('{0}'," +
                            "'{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', GETDATE())";
                var queryUpdate = "Update dbo.ExcelInfo set PartnerID='{1}',Style='{2}',PartnerSKU='{3}',UPC='{4}',[Description]='{5}',ColorCode='{6}',ColorDesc='{7}',SizeCode='{8}',SizeDescription='{9}',SizeClassDescription='{10}',"+
                    "WeightLBS='{11}',PreviewImageURL='{12}' where Id={0}";
                int id;
                int i = 0;
                string tempQuery;
                foreach (var record in records)
                {
                    //Add
                    if (!list.Any(x => x.PartnerID == record.PartnerID && x.PartnerSKU == record.PartnerSKU))
                    {
                        tempQuery = CommonHelper.NormalizeSqlQuery(queryInsert, record.PartnerID, record.Style, record.PartnerSKU, record.UPC, record.Description, record.ColorCode, record.ColorDesc, record.SizeCode,
                            record.SizeDescription, record.SizeClassDescription, record.WeightLBS, record.PreviewImageURL);
                    }
                    //update
                    else
                    {
                        id = list.Single(x => x.PartnerID == record.PartnerID && x.PartnerSKU == record.PartnerSKU).Id;
                        tempQuery = CommonHelper.NormalizeSqlQuery(queryUpdate, id, record.PartnerID, record.Style, record.PartnerSKU, record.UPC, record.Description, record.ColorCode, record.ColorDesc, record.SizeCode,
                            record.SizeDescription, record.SizeClassDescription, record.WeightLBS, record.PreviewImageURL);
                    }
                    sqlQueryStringBuffer.AppendLine(tempQuery);
                    i++;
                    if (i % 200 == 0)
                    {
                        _context.Database.ExecuteSqlCommand(sqlQueryStringBuffer.ToString());
                        sqlQueryStringBuffer.Clear();
                    }
                }
                //exec remain temp list
                _context.Database.ExecuteSqlCommand(sqlQueryStringBuffer.ToString());
                sqlQueryStringBuffer.Clear();
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //public Task ImportContactAsyn(Stream data, string fileName)
        //{
        //    try
        //    {
        //        //read file from stream
        //        var stream = new StreamReader(data);
        //        var csv = new CsvReader(stream);
        //        csv.Configuration.RegisterClassMap<ImportContactModelMap>();
        //        var records = csv.GetRecords<ExcelInfo>().ToList();

        //        DbSet<ExcelInfo> dbSetExcelInfo = _context.Set<ExcelInfo>();
        //        var list = dbSetExcelInfo.ToList();
        //        //Mapper.Configuration.CreateMapper();
        //        //var list = dbSetExcelInfo.SqlQuery("select * from ExcelInfo");
        //        foreach (var record in records)
        //        {
        //            //Add
        //            if (!list.Any(x => x.PartnerID == record.PartnerID && x.PartnerSKU == record.PartnerSKU))
        //            {
        //                dbSetExcelInfo.Add(record);
        //            }
        //            //update
        //            else
        //            {
        //                var info = list.Single(x => x.PartnerID == record.PartnerID && x.PartnerSKU == record.PartnerSKU);
        //                //record.Id = info.Id;
        //                //info = Mapper.Map<ExcelInfo>(record);

        //                info.PartnerID = record.PartnerID;
        //                info.ColorDesc = record.ColorDesc;
        //                info.PartnerSKU = record.PartnerSKU;
        //                info.PreviewImageURL = record.PreviewImageURL;
        //                info.SizeClassDescription = record.SizeClassDescription;
        //                info.SizeCode = record.SizeCode;
        //                info.SizeDescription = record.SizeDescription;
        //                info.WeightLBS = record.WeightLBS;
        //                info.ColorCode = record.ColorCode;
        //                info.Description = record.Description;
        //                info.Style = record.Style;
        //                info.UPC = record.UPC;

        //                _context.Entry(info).State= EntityState.Modified;
        //            }
        //        }
        //        _context.SaveChanges();
        //        return  Task.FromResult(true);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw  new Exception(ex.Message);
        //    }
        //}
        #endregion

        #region sync
        public Contact GetById(int id)
        {
            var result = _cacheManager.Get(String.Format(KeyForCacheYayYo, id),
                () =>
                {
                    var query = "SELECT * FROM Contact WHERE Id = @p0";
                    var res = _dbSet.SqlQuery(query, id).SingleOrDefault();
                    return res;
                });

            return result;
        }

        public void Create(Contact contact)
        {
            try
            {
                _dbSet.Add(contact);
                _context.SaveChanges();


            }
            catch (Exception ex)
            {
                LogError("There is error while updating Contact: " + ex.Message, ex);
            }
        }

        public void Update(Contact contact)
        {
            try
            {
                _cacheManager.Remove(String.Format(KeyForCacheYayYo, contact.Id));
                //ref:http://patrickdesjardins.com/blog/entity-framework-ef-modifying-an-instance-that-is-already-in-the-context
                //A way to do it is to navigate inside the local of the DbSet to see if this one is there. If the entity is present, than you detach
                var local = _context.Set<Contact>()
                         .Local
                         .FirstOrDefault(f => f.Id == contact.Id);
                if (local != null)
                {
                    _context.Entry(local).State = EntityState.Detached;
                }
                _context.Entry(contact).State = EntityState.Modified;
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                //Trace.TraceError("There is error while updating data: " + dex.InnerException);
                LogError("There is error while updating Contact: " + ex.Message, ex);
            }
        }

        public void Delete(Contact contact)
        {
            try
            {
                _cacheManager.Remove(String.Format(KeyForCacheYayYo, contact.Id));

                _context.Database.ExecuteSqlCommand("DELETE Contact WHERE Id = @p0", contact.Id);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                LogError("There is error while updating Contact: " + ex.Message, ex);
            }
        }

        



        #endregion
        #endregion
    }
}
