using System.Data;
using System.Threading.Tasks;

namespace Service.Interface.Export
{
    public interface IExportPdf
    {
        Task<byte[]> GeneratePdf(DataTable dtSrc);
    }
}
