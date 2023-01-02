using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRates.Models
{
    public class Taux
    {
        public int id { get; set; }
        public string ref_cur { get; set; }
        public string sec_cur { get; set; }
        public float purchase_price { get; set; }
        public float sale_price { get; set; }
        public string date_added { get; set ; }
        public string hour { get; set ; }
        public string is_active { get; set ; }

    }
}
