using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetSelfHostDemo.Models
{
    public class Item
    {
        public int Cantidad { get; set; }
        public string Descripcion { get; set; }
        public Decimal Importe { get; set; }
        public Decimal Unitario { get; set; }
    }
}
