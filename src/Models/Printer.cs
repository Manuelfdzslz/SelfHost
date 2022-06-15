using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetSelfHostDemo.Models
{
    public class InstalledPrinter
    {
        public string Name { get; set; }
        public List<string> Texts { get; set; }
    }
}
