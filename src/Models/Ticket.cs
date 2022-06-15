using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetSelfHostDemo.Models
{
    public class Ticket
    {
        public string PrinterName { get; set; }
        public List<string> HeaderLines { get; set; }
        public string SubheaderLine { get; set; }
        public string VentaCode { get; set; }
        public List<Item> Items { get; set; }
        public Decimal Total { get; set; }
        public string HeaderImage { get; set; }
    }
}
