using System.Data;
using System.Threading.Tasks;
using Spire.Pdf.Exporting.XPS.Schema;
using System.Collections.Generic;

namespace Service.Interface.Export
{
    public interface IExportExel
    {
        Task<byte[]> GenerateExcel2010(DataSet dsSrc,List<string> colSum);
    }
}
