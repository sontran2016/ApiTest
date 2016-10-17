using Service.Interface.Export;
using System.Data;
using OfficeOpenXml;
using System.Drawing;
using OfficeOpenXml.Style;
using System.Threading.Tasks;

namespace Service.Implement.Export
{
    public class ExportExelService : IExportExel
    {
        public Task<byte[]> GenerateExcel2010(DataSet dsSrc)
        {
            using (ExcelPackage objExcelPackage = new ExcelPackage())
            {
                foreach (DataTable dtSrc in dsSrc.Tables)
                {
                    //Create the worksheet    
                    ExcelWorksheet objWorksheet = objExcelPackage.Workbook.Worksheets.Add(dtSrc.TableName);
                    //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1    
                    objWorksheet.Cells["A1"].LoadFromDataTable(dtSrc, true);
                    objWorksheet.Cells.Style.Font.SetFromFont(new Font("Calibri", 10));
                    objWorksheet.Cells.AutoFitColumns();                    
                    objWorksheet.View.FreezePanes(2, 1);
                    //Format the header    
                    using (ExcelRange objRange = objWorksheet.Cells["A1:XFD1"])
                    {
                        objRange.Style.Font.Bold = true;
                        objRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        objRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        objRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        objRange.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                    }
                }
                var bytes = objExcelPackage.GetAsByteArray();
                return Task.FromResult(bytes);
            }
        }

    }
}