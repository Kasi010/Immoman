using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test4.DataClass
{
    public class Investdata
    {
        public decimal Kaufpreis { get; set; }
        public decimal Makler { get; set; }
        public decimal Notar { get; set; }
        public decimal Grundbuchamt { get; set; }
        public decimal Grunderwerbsteuer { get; set; }
        public decimal SummeKaufpreis { get; set; }
        public decimal Anfangsinvestitionen { get; set; }
    }
}
