using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRates.Models
{
    public class Forex
    {
        public string symbole { get; set; }
        public string currency_base { get; set; }
        public string currency_quote { get; set; }
        public string exchange { get; set; }
        public string datetime { get; set; }
        public string open { get; set; }
        public string high { get; set; }
        public string low { get; set; }
        public string close { get; set; }


    }
}
