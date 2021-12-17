using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test4.DataClass
{
    //Datenklasse für die abgefragten Daten aus der Financcalc-Tabelle
    public class Financedata
    {
        public decimal Darlehenssumme { get; set; }
        public decimal Zinssatz { get; set; }
        public decimal Tilgung { get; set; }
    }
}
