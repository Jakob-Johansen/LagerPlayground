using iText.Barcodes;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Xobject;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using LagerPlayground.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagerPlayground.Helpers
{
    public class PdfHelper
    {
        private readonly IWebHostEnvironment _env;
        public PdfHelper(IWebHostEnvironment env)
        {
            _env = env;
        }

        public void GenerateBarcodesPrint(List<Tote> barcodeList)
        {
            using PdfWriter writer = new(Path.Combine(_env.WebRootPath + "/Pdf/Barcode.pdf"));
            PdfDocument pdf = new(writer);
            Document document = new(pdf);

            document.SetMargins(2f, 2f, 2f, 2f);

            pdf.AddNewPage();

            Table table = new(UnitValue.CreatePercentArray(3));
            table.SetWidth(100f);
            table.SetHorizontalAlignment(HorizontalAlignment.CENTER);
            table.SetTextAlignment(TextAlignment.CENTER);
            table.SetMarginTop(10f);

            foreach (var item in barcodeList)
            {
                table.AddCell(CreateBarcode(item.Name, item.Barcode, pdf));
            }

            document.Add(table);
            document.Close();
        }

        private static Cell CreateBarcode(string name, string barcode, PdfDocument pdfDoc)
        {
            Barcode128 barcode128 = new(pdfDoc);
            barcode128.SetCodeType(Barcode128.CODE128);
            barcode128.SetCode(barcode);
            barcode128.SetAltText(name);
            barcode128.SetX(1f);
            barcode128.SetBarHeight(47f);
            barcode128.SetBaseline(12f);
            barcode128.SetSize(11f);

            PdfFormXObject barcodeObject = barcode128.CreateFormXObject(null, null, pdfDoc);
            Cell cell = new();
            cell.Add(new Image(barcodeObject));
            cell.SetBorder(Border.NO_BORDER);
            cell.SetPaddingTop(10f);
            cell.SetPaddingRight(10f);
            cell.SetPaddingBottom(10f);
            cell.SetPaddingLeft(10f);
            return cell;
        }

    }
}
