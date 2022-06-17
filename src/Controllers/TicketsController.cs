using AspNetSelfHostDemo.Models;
using AspNetSelfHostDemo.Servicios;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Web.Http;

namespace AspNetSelfHostDemo
{
    public class TicketsController : ApiController
    {
       

        
        public IHttpActionResult Post([FromBody] Ticket value)
        {


            TicketService ticketService = new TicketService();
            
            if (!string.IsNullOrEmpty(value.HeaderImage))
            {
                ticketService.HeaderImage = ticketService.LoadBase64(value.HeaderImage);
            }
            
            foreach (var headerLine in value.HeaderLines)
            {
                ticketService.AddHeaderLine(headerLine);
            }
            /*

            ticketService.AddSubHeaderLine(value.SubheaderLine);
            foreach (var item in value.Items)
            {
                ticketService.AddItem(item.Cantidad.ToString(), item.Descripcion, item.Importe.ToString());
            }*/
            //ticketService.AddTotal("TOTAL", value.Total.ToString());
            //ticketService.AddFooterLine("Gracias por su preferencia...");

            Zen.Barcode.Code128BarcodeDraw codigo = Zen.Barcode.BarcodeDrawFactory.Code128WithChecksum;
            value.VentaCode = value.VentaCode.PadLeft(10, '0');
            Image imagen= codigo.Draw(value.VentaCode, 65,2);
            ticketService.BarcCodeImage = imagen;

            if (!string.IsNullOrEmpty(value.PrinterName))
            {
                ticketService.PrintLabel(value.PrinterName);

            }
            else
            {
                ticketService.PrintLabel();
            }
            


            return Ok();
        }



    }
}
