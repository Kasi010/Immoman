using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test4.DataClass
{
    //Datenklasse für die abgefragten Daten aus der Umlagekostcalc-Tabelle
    public class Umlagekostdata
    {
        public decimal HausgeldNichtUmlage { get; set; }
        public decimal Eigeninstandhaltungsrücklage { get; set; }
        public decimal SonstigeNichtumlagekosten { get; set; }
    }
}
