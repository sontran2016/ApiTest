using System.Data;
using System.Threading.Tasks;

namespace Service.Interface.Export
{
    public interface IExportExel
    {
        Task<byte[]> GenerateExcel2010(DataSet dsSrc);
    }
}
