using AspNetSelfHostDemo.Models;
using AspNetSelfHostDemo.Servicios;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Web.Http;

namespace AspNetSelfHostDemo
{
    public class PrintersController : ApiController
    {
        // GET api/Printers 
        public IHttpActionResult Get()
        {
            PrinterService service = new PrinterService();
            List<Printer> r= service.GetPrinters();
            return Ok(r);
        }

        
        public IHttpActionResult Post([FromBody] Ticket value)
        {


            TicketService ticketService = new TicketService();
            if (!string.IsNullOrEmpty(value.HeaderImage))
            {
                //ticketService.HeaderImage =  value.HeaderImage;
            }
            //
            foreach (var headerLine in value.HeaderLines)
            {
                ticketService.AddHeaderLine(headerLine);
            }

            ticketService.AddSubHeaderLine(value.SubheaderLine);
            foreach (var item in value.Items)
            {
                ticketService.AddItem(item.Cantidad.ToString(), item.Descripcion, item.Importe.ToString());
            }
            ticketService.AddTotal("TOTAL", value.Total.ToString());
            ticketService.AddFooterLine("Gracias por su preferencia...");

            if (!string.IsNullOrEmpty(value.PrinterName))
            {
                ticketService.PrintTicket(value.PrinterName);

            }
            else
            {
                ticketService.PrintTicket();
            }


            return Ok();
        }





    }
}
