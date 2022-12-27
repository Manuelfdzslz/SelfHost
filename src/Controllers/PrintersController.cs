using AspNetSelfHostDemo.Models;
using AspNetSelfHostDemo.Servicios;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Web.Http;
using System.Web.Http.Cors;

namespace AspNetSelfHostDemo
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
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

            for (int i = 0; i < value.NoImpresiones; i++)
            {
                TicketService ticketService = new TicketService();
                
                if (value.ShowOriginal && i == 0)
                {
                    ticketService.AddHeaderLine("******** ORIGINAL ********");
                }
                if (value.NoImpresiones > 1 && value.ShowOriginal && i > 0)
                {
                    ticketService.AddHeaderLine("******** DUPLICADO ********");
                }


                if (!string.IsNullOrEmpty(value.HeaderImage))
                {
                    ticketService.HeaderImage = ticketService.LoadBase64(value.HeaderImage);
                }

                if (!string.IsNullOrEmpty(value.LogoUrl))
                {
                    ticketService.AddLogo(value.LogoUrl);
                }

                ticketService.AddSubHeaderLine(value.Name);
                ticketService.AddSubHeaderLine(value.TaxId);
                ticketService.AddSubHeaderLine(value.Address);
                ticketService.AddSubHeaderLine(value.SaleDate.ToString());
                ticketService.AddSubHeaderLine("venta: " + value.SaleId);
                foreach (var item in value.Items)
                {
                    ticketService.AddItem(item.Cantidad.ToString(), item.Descripcion, item.Importe.ToString());
                }

                ticketService.AddTotal("Subtotal", value.Subtotal.ToString());
                ticketService.AddTotal("Impuestos", value.Tax.ToString());
                ticketService.AddTotal("TOTAL", value.Total.ToString());
                /*
                Zen.Barcode.Code128BarcodeDraw codigo = Zen.Barcode.BarcodeDrawFactory.Code128WithChecksum;
                value.TaxId = value.TaxId.PadLeft(10, '0');
                Image imagen = codigo.Draw(value.TaxId, 55, 1);
                ticketService.BarcCodeImage = imagen;
                */

                ticketService.AddFooterLine("Gracias por su preferencia...");
                ticketService.AddFooterLine(value.Pagare);
                ticketService.PrintTicket(value.PrinterName);
            }
            
            return Ok();

           
        }

        [Route("/api/printers/Valid")]
        [HttpGet]
        public IHttpActionResult validaImpresora(string name)
        {
            PrinterService service = new PrinterService();
            return  Ok(service.EstaEnLineaLaImpresora(name) );
        }



    }
}
