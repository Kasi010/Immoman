using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test4.DataClass
{
    //Datenklasse für die abgefragten Daten zur Berechnung des Cashflows
    public class Cashflowdata
    {
        public decimal Warmmiete { get; set; }
        public decimal Bewirtschaftungskosten { get; set; }
        public decimal Kapitaldienst { get; set; }
    }
}
