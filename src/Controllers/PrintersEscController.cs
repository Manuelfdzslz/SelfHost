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
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace AspNetSelfHostDemo
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PrintersEscController : ApiController
    {
       

        public IHttpActionResult Post([FromBody] Ticket value)
        {
            
            
            try
            {

                for (int i = 0; i < value.NoImpresiones; i++)
                {

                    Bitmap imagebitMap;
                    Printer printer = new Printer(value.PrinterName);
                    if ( value.ShowOriginal && i == 0)
                    {
                        printer.AlignCenter();
                        printer.Append("******** ORIGINAL ********");
                    }
                    if (value.NoImpresiones > 1 && value.ShowOriginal && i > 0)
                    {
                        printer.AlignCenter();
                        printer.Append("******** DUPLICADO ********");
                    }
                    if (!string.IsNullOrEmpty(value.HeaderImage))
                    {
                        var img = LoadBase64(value.HeaderImage);
                        imagebitMap = new System.Drawing.Bitmap(img);
                        printer.Image(imagebitMap);
                    }

                    if (!string.IsNullOrEmpty(value.LogoUrl))
                    {
                        printer.Image(_getImage(value.LogoUrl));
                    }

                    printer.AlignCenter();
                    printer.Append(value.Name);
                    printer.Append(value.TaxId);
                    printer.Append(value.Address);
                    printer.AlignCenter();
                    printer.Append("*** VENTA ***");
                    printer.AlignRight();
                    printer.Append(value.SaleDate.ToString());
                    printer.AlignLeft();
                    printer.Append("Folio: " + value.SaleId);

                    printer.AlignLeft();
                    printer.Append("Vendedor: " + value.Vendedor);

                    printer.Separator();
                    printer.Append("Descripción         cantidad      importe");
                    printer.Separator();

                    foreach (var item in value.Items)
                    {
                        printer.Font(_toxtToItem(item), Fonts.FontC);
                    }

                    printer.AlignRight();
                    printer.Append(_ajustaTextTotales("Subtotal", String.Format("{0:0.00}", value.Subtotal)));
                    printer.Append(_ajustaTextTotales("Impuesto", String.Format("{0:0.00}", value.Tax)));
                    printer.Append(_ajustaTextTotales("Total", String.Format("{0:0.00}", value.Total)));
                    if (value.ShowBarCode)
                    {
                        printer.Code128(value.SaleId.PadLeft(10, '0'));
                    }
                    printer.Append(" ");
                    printer.AlignLeft();
                    printer.Append("GRACIAS POR SU PREFERENCIA ");
                    printer.Append("FUE UN PLACER ATENDERLE");
                    printer.Append(" ");
                    printer.Append(value.Pagare);
                    printer.Append(" ");
                    printer.InitializePrint();
                    printer.FullPaperCut();

                    printer.PrintDocument();


                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            
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


        private Bitmap _getImage(string urlLogo)
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                System.Net.WebRequest request = System.Net.WebRequest.Create(urlLogo);//"https://www.camiongo.com/images/register/transportista.jpg"
                System.Net.WebResponse response = request.GetResponse();
                System.IO.Stream responseStream = response.GetResponseStream();
                Bitmap bitmap2 = new Bitmap(responseStream);
                return bitmap2;

            }
            catch (Exception)
            {
            }
            return null;
        }


        private string _toxtToItem(Item item)
        {
            /*
            //fontA
            int salto = 22;
            int limite = 41;
            int limiteCantidad = 29;
            */
            int salto = 28;
            int limite = 55;
            int limiteCantidad = 40;
            int lengthText = item.Descripcion.Length;


            string[] partsDescription= item.Descripcion.Split(' ');
            string newLine = "";
            if (lengthText > salto)
            {
                bool primera = true;
                foreach (var part in partsDescription)
                {
                   
                    if (newLine.Length + part.Length < salto) 
                    {
                        newLine += part+" ";
                        continue;
                    }
                    else if(primera)
                    {
                        primera = false;
                        int faltante = limiteCantidad - newLine.Length;
                        string cant = item.Cantidad.ToString() + "x" + item.Unitario.ToString();
                        for (int i = 0; i < faltante - cant.Length; i++)
                        {
                            newLine += " ";
                        }
                        newLine += cant + " ";

                        for ( int i = newLine.Length; i <= limite - item.Importe.ToString().Length; i++)
                        {
                            newLine += " ";
                        }
                        newLine += item.Importe.ToString();
                        newLine += part + " ";
                        continue;

                    }
                    newLine += part+" ";


                }

            }
            else
            {
                newLine = item.Descripcion;
                int faltante = limiteCantidad - newLine.Length;
                string cant = item.Cantidad.ToString() + "x" + item.Unitario.ToString();
                for (int i = 0; i < faltante- cant.Length; i++)
                {
                    newLine += " ";
                }
                newLine += cant + " ";

                for (int i = newLine.Length; i <= limite - item.Importe.ToString().Length; i++)
                {
                    newLine += " ";
                }
                newLine += item.Importe.ToString();

            }
            int strLen = newLine.Length;
            return newLine;
        }


        private string _ajustaTextTotales(string text,string valor)
        {
            string result = "";
            int inicio = 15;
            int limite = 41;

            for (int i = 0; i < inicio; i++)
            {
                result += " ";
            }
            result += text;
            int faltante = limite - (result.Length + valor.Length);
            for (int i = 0; i < faltante; i++)
            {
                result += " ";

            }
            result += valor;

            return result;
        }
    }
}
