using AspNetSelfHostDemo.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetSelfHostDemo.Servicios
{
    public class PrinterService
    {
        private InstalledPrinter p = new InstalledPrinter();

        public List<InstalledPrinter> GetPrinters()
        {
            var r =System.Drawing.Printing.PrinterSettings.InstalledPrinters;
            List<InstalledPrinter> list = new List<InstalledPrinter>();
            foreach (var item in r)
            {
                InstalledPrinter p = new InstalledPrinter();
                p.Name = item.ToString();
                list.Add(p);
            }
            return list;
        }


        public bool Print(InstalledPrinter printer)
        {
            bool response = true;
            try
            {
                this.p = printer;
                PrintDocument document = new PrintDocument();
                PrinterSettings settings = new PrinterSettings();
                if (!string.IsNullOrEmpty(printer.Name))
                {
                    settings.PrinterName = printer.Name;
                }
                document.PrinterSettings = settings;
                document.PrintPage += Imprimir;
                document.Print();
            }
            catch (Exception)
            {

                return false;
            }
            return response;
            
            
        }

       
        private void Imprimir(object sender,PrintPageEventArgs e)
        {
            try
            {
                Font font = new Font("Arial", 14, FontStyle.Regular, GraphicsUnit.Point);
                //e.Graphics.DrawString("un tiket", font, Brushes.Black, new RectangleF(0, 10, 120, 20));


                float linesPerPage = 0;
                float yPos = 0;
                int count = 0;
                float leftMargin = e.MarginBounds.Left;
                float topMargin = e.MarginBounds.Top;

                // Calculate the number of lines per page.
                linesPerPage = e.MarginBounds.Height /
                   font.GetHeight(e.Graphics);

                // Print each line of the file.
                foreach (var line in p.Texts)
                {
                    yPos = topMargin + (count * font.GetHeight(e.Graphics));
                    e.Graphics.DrawString(line, font, Brushes.Black, leftMargin, yPos, new StringFormat());
                    count++;
                }
                /*
             // If more lines exist, print another page.
             if (line != null)
                 e.HasMorePages = true;
             else
                 e.HasMorePages = false;
             */
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            
        }
    }
}
