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

        // POST api/Printers 
        public IHttpActionResult Post([FromBody] Printer value)
        {
            PrinterService service = new PrinterService();
           bool r=  service.Print(value);
            if (!r)
            {
                return InternalServerError();
            }
            return Ok(r);
        }



    } 
}
