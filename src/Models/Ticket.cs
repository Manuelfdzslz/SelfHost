using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetSelfHostDemo.Models
{
    public class Ticket
    {
        public string LogoUrl { get; set; }
        public int NoImpresiones { get; set; }
        public string PrinterName { get; set; }
        public string Name { get; set; }
        public string TaxId { get; set; }
        public string Address { get; set; }
        public string SaleId { get; set; }
        public string Caja { get; set; }
        public string Vendedor { get; set; }
        public DateTime SaleDate { get; set; }
        public List<Item> Items { get; set; }
        public Decimal Total { get; set; }
        public Decimal Subtotal { get; set; }
        public Decimal Tax { get; set; }
        public Decimal PaymentValue { get; set; }
        public Decimal Change { get; set; }
        public string HeaderImage { get; set; }
        public string Pagare { get; set; }
        public bool ShowOriginal { get; set; }
        public bool ShowBarCode { get; set; }

        public Ticket()
        {
            NoImpresiones = 1;
            ShowOriginal = false;
            ShowBarCode = false;
        }
    }
}
