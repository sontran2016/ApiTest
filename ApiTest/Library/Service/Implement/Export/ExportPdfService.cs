using Service.Interface.Export;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Threading.Tasks;

namespace Service.Implement.Export
{
    public class ExportPdfService : IExportPdf
    {
        //public void GeneratePdf(string p_strPath, DataTable p_dtSrc)
        //{
        //    using (ExcelPackage objExcelPackage = new ExcelPackage())
        //    {
        //        //Create the worksheet    
        //        ExcelWorksheet objWorksheet = objExcelPackage.Workbook.Worksheets.Add(p_dtSrc.TableName);
        //        //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1    
        //        objWorksheet.Cells["A1"].LoadFromDataTable(p_dtSrc, true);
        //        objWorksheet.Cells.Style.Font.SetFromFont(new Font("Calibri", 10));
        //        objWorksheet.Cells.AutoFitColumns();
        //        objWorksheet.View.FreezePanes(2, 1);
        //        //Format the header    
        //        using (ExcelRange objRange = objWorksheet.Cells["A1:XFD1"])
        //        {
        //            objRange.Style.Font.Bold = true;
        //            objRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            objRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //            objRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
        //            objRange.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
        //        }

        //        //Write content to excel file    
        //        File.WriteAllBytes(p_strPath, objExcelPackage.GetAsByteArray());

        //        var workbook = new Workbook();
        //        workbook.LoadFromFile(p_strPath);
        //        workbook.SaveToFile(p_strPath, Spire.Xls.FileFormat.PDF);
        //    }
        //}

        public Task<byte[]> GeneratePdf(DataTable dtSrc)
        {

            var baseFontTimesRoman = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
            var titleFont = new Font(baseFontTimesRoman, 12, Font.BOLD);
            var bodyFont = new Font(baseFontTimesRoman, 10, Font.NORMAL);
            var pageSize = PageSize.A4;
            var doc = new Document(pageSize);
            var stream = new MemoryStream();
            PdfWriter.GetInstance(doc, stream);
            doc.Open();

            var header = new PdfPTable(1) { WidthPercentage = 100f };
            var cells = new PdfPCell(new Phrase(dtSrc.TableName)) { Border = Rectangle.NO_BORDER };
            header.AddCell(cells);
            doc.Add(header);
            doc.Add(new Paragraph(" "));

            var headerList = new List<string>();
            foreach (DataColumn c in dtSrc.Columns)
                headerList.Add(c.ColumnName);

            var table = new PdfPTable(headerList.Count)
            {
                WidthPercentage = 100f
            };
            #region Header
            foreach (var cell in headerList.Select(headerColumn => new PdfPCell(new Phrase(headerColumn, titleFont))
            {
                Rowspan = 8,
                BackgroundColor = BaseColor.LIGHT_GRAY,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE
            }))
            {
                table.AddCell(cell);
            }
            doc.Add(table);
            #endregion
            #region Body

            foreach (DataRow row in dtSrc.Rows)
            {
                var detail = new PdfPTable(headerList.Count) { WidthPercentage = 100f };
                for(int i=0; i< dtSrc.Columns.Count;i++)
                {
                    var cel = new PdfPCell(new Phrase(row[i].ToString(), bodyFont))
                    {
                        HorizontalAlignment = Element.ALIGN_CENTER
                    };
                    detail.AddCell(cel);
                }
                doc.Add(detail);
            }
            doc.Close();
            #endregion

            var bytes = stream.ToArray();
            return Task.FromResult(bytes);
        }

    }
}