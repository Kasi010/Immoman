using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test4.DataClass
{
    //Datenklasse für die abgefragten Daten aus der Mietcalc-Tabelle
    public class Mietdata
    {
        public decimal Kaltmiete { get; set; }
        public decimal KaltmieteGesamt { get; set; }
        public decimal SonstigeMietaufwendungen { get; set; }
        public decimal UmlageBewirtschaftskosten { get; set; }
        public decimal Heizkosten { get; set; }
        public decimal KostenWassAbwasser { get; set; }
        public decimal Nettokaltmiete { get; set; }
        public decimal Warmmiete { get; set; }
    }
}
