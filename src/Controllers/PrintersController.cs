using AspNetSelfHostDemo.Models;
using AspNetSelfHostDemo.Servicios;
using ESC_POS_USB_NET.Enums;
using ESC_POS_USB_NET.Printer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace AspNetSelfHostDemo
{
    public class PrintersController : ApiController
    {
        // GET api/Printers 
        public IHttpActionResult Get()
        {
            PrinterService service = new PrinterService();
            List<InstalledPrinter> r = service.GetPrinters();
            return Ok(r);
        }

        public IHttpActionResult Post([FromBody] Ticket value)
        {
            Bitmap imagebitMap;
            Printer printer = new Printer(value.PrinterName);
            if (!string.IsNullOrEmpty(value.HeaderImage))
            {
                var img = LoadBase64(value.HeaderImage);
                imagebitMap = new System.Drawing.Bitmap(img);
                printer.Image(imagebitMap);
            }
            printer.Separator();

            /*printer.Append("NORMAL - 48 COLUMNS");
            printer.Append("1...5...10...15...20...25...30...35...40...45.48");
            printer.Separator();
            printer.Append("Text Normal");
            printer.BoldMode("Bold Text");
            printer.UnderlineMode("Underlined text");
            printer.Separator();
            printer.ExpandedMode(PrinterModeState.On);
            printer.Append("Expanded - 23 COLUMNS");
            printer.Append("1...5...10...15...20..23");
            printer.ExpandedMode(PrinterModeState.Off);
            printer.Separator();
            printer.CondensedMode(PrinterModeState.On);
            printer.Append("Condensed - 64 COLUMNS");
            printer.Append("1...5...10...15...20...25...30...35...40...45...50...55...60..64");
            printer.CondensedMode(PrinterModeState.Off);
            printer.Separator();
            printer.DoubleWidth2();
            printer.Append("Font Width 2");
            printer.DoubleWidth3();
            printer.Append("Font Width 3");
            printer.NormalWidth();
            printer.Append("Normal width");
            printer.Separator();
            printer.AlignRight();
            printer.Append("Right aligned text");
            printer.AlignCenter();
            printer.Append("Center-aligned text");
            printer.AlignLeft();
            printer.Append("Left aligned text");
            printer.Separator();
            printer.Font("Font A", Fonts.FontA);
            printer.Font("Font B", Fonts.FontB);
            printer.Font("Font C", Fonts.FontC);
            printer.Font("Font D", Fonts.FontD);
            printer.Font("Font E", Fonts.FontE);
            printer.Font("Font Special A", Fonts.SpecialFontA);
            printer.Font("Font Special B", Fonts.SpecialFontB);
            printer.Separator();
            printer.InitializePrint();
            printer.SetLineHeight(24);
            printer.Append("This is first line with line height of 30 dots");
            printer.SetLineHeight(40);
            printer.Append("This is second line with line height of 24 dots");
            printer.Append("This is third line with line height of 40 dots");
            printer.NewLines(3);
            printer.Append("End of Test :)");
            printer.Separator();*/
            printer.Append("Code 128");
             printer.Code128("123456789");
            printer.Separator();
            printer.FullPaperCut();
            printer.PrintDocument();

            return Ok();
        }

        public Image LoadBase64(string base64)
        {
            byte[] bytes = Convert.FromBase64String(base64);
            Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = Image.FromStream(ms);
            }
            return image;
        }
    }
}
