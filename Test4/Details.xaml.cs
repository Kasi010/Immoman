using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Test4.DataClass;

namespace Test4
{
    /// <summary>
    /// Interaktionslogik für Details.xaml
    /// </summary>
    public partial class Details : Window
    {
        /*Fenster zur Darstellung der berechneten Ergebnisse. Wird auf Doppelklick auf das Element im MainWindow ausgelöst*/

        //immoID wird per Kosntruktor aus dem MainWindow an dieses Fenster übergeben
        public Details(int immoID)
        {
            InitializeComponent();
            this.ImmoID = immoID;
            Investcompute(immoID);
            Mietcompute(immoID);
            Umlagekostcompute(immoID);
            Financcompute(immoID);
            DataSet(immoID);
            Cashflowcompute(immoID);
        }

        //Property zum Speichern der ImmoID aus der Immobilientabelle
        public int ImmoID { get; set; }

        //Gibt den Wert für die Wohnfläche für die übergebene ImmoID zurück
        private decimal GetWohnflaeche(int immoID)
        {
            //Connection-String
            string cn_string = "Server=localhost;Database=Immoman;Trusted_Connection=true;";

            //Aufbau der verbindung
            using (var c = new SqlConnection(cn_string))
            {
                SqlCommand cmd = c.CreateCommand();

                //SQL-Befehl
                cmd.CommandText = $"SELECT Wohnflaeche FROM Immobilie WHERE ImmobilienID = @immoID; ";

                cmd.Parameters.Add("@immoID", SqlDbType.Int);
                cmd.Parameters["@immoID"].Value = immoID;


                c.Open();

                var i = Convert.ToDecimal(cmd.ExecuteScalar());

                c.Close();

                return i;

            }

        }

        //Gibt den Wert für die Gesamtinvestition für die übergebene ImmoID zurück
        private decimal GetGesamtinvest(int immoID)
        {
            //Connection-String
            string cn_string = "Server=localhost;Database=Immoman;Trusted_Connection=true;";

            //Aufbau der Verbindung
            using (var c = new SqlConnection(cn_string))
            {
                SqlCommand cmd = c.CreateCommand();

                //SQL-Befehl
                cmd.CommandText = $"SELECT Gesamtinvestition FROM dbo.Investcalc WHERE ImmobilienID = @immoID; ";

                cmd.Parameters.Add("@immoID", SqlDbType.Int);
                cmd.Parameters["@immoID"].Value = immoID;


                c.Open();

                var i = Convert.ToDecimal(cmd.ExecuteScalar());

                c.Close();

                return i;

            }
        }

        //Gibt die Werte für die Ausgabe zurück und schreibt diese Daten in die Textboxen
        private void DataSet(int immoID)
        {
            //Connection-String
            string cn_string = "Server=localhost;Database=Immoman;Trusted_Connection=true;";

            //Aufbau der Verbindung
            using (var c = new SqlConnection(cn_string))
            {
                SqlCommand cmd = c.CreateCommand();

                //SQL-Befehl
                cmd.CommandText = $"SELECT Investcalc.Gesamtinvestition,Mietcalc.Nettokaltmiete, Mietcalc.Warmmiete, Mietcalc.GesamtWarmmiete FROM Investcalc INNER JOIN Mietcalc ON Investcalc.ImmobilienID = Mietcalc.ImmobilienID where Investcalc.ImmobilienID = @immoID AND Mietcalc.ImmobilienID = @immoID;";

                cmd.Parameters.Add("@immoID", SqlDbType.Int);
                cmd.Parameters["@immoID"].Value = immoID;


                c.Open();

                SqlDataReader r = cmd.ExecuteReader();

                //Schreiben der Daten in die Textboxen
                while (r.Read())
                {
                    detailinvest.Text = r[0].ToString();
                    detailnettokaltmiete.Text = r[1].ToString();
                    detailwarmmiete.Text = r[2].ToString();
                    detailgesamtwarmmiete.Text = r[3].ToString();
                }

                c.Close();

            }

            //Aufbau der Verbindung
            using (var c = new SqlConnection(cn_string))
            {
                SqlCommand cmd = c.CreateCommand();

                //SQL-Befehl
                cmd.CommandText = $"Select Umlagekostcalc.GesamtUmlage, Financcalc.Kapitaldienst From Umlagekostcalc Inner Join Financcalc ON Umlagekostcalc.ImmobilienID = Financcalc.ImmobilienID where Umlagekostcalc.ImmobilienID = @immoID AND Financcalc.ImmobilienID = @immoID;";

                cmd.Parameters.Add("@immoID", SqlDbType.Int);
                cmd.Parameters["@immoID"].Value = immoID;


                c.Open();

                SqlDataReader r = cmd.ExecuteReader();

                //Schreiben der Daten in die Textboxen
                while (r.Read())
                {
                    detailumlagekost.Text = r[0].ToString();
                    detailkapitaldienst.Text = r[1].ToString();
                }

                c.Close();

            }



        }

        //Berechnet die Werte für die Investcalctabelle und schreibt diese in Investcalc
        private void Investcompute(int immoID)
        {
            Investdata investdata = new Investdata();

            //Connection-String
            string cn_string = "Server=localhost;Database=Immoman;Trusted_Connection=true;";

            //Aufbau der Verbindung
            using (var c = new SqlConnection(cn_string))
            {
                SqlCommand cmd = c.CreateCommand();

                //SQL-Befehl
                cmd.CommandText = $"SELECT * FROM dbo.Investcalc WHERE ImmobilienID=@immoID;";

                cmd.Parameters.Add("@immoID", SqlDbType.Int);
                cmd.Parameters["@immoID"].Value = immoID;


                c.Open();

                SqlDataReader r = cmd.ExecuteReader();

                //Zuweisen der Werte in die Datenklasse Investdata
                while (r.Read())
                {
                    investdata.Kaufpreis = Convert.ToDecimal(r[1]);
                    investdata.Makler = Convert.ToDecimal(r[2]);
                    investdata.Notar = Convert.ToDecimal(r[3]);
                    investdata.Grundbuchamt = Convert.ToDecimal(r[6]);
                    investdata.Grunderwerbsteuer = Convert.ToDecimal(r[7]);
                    investdata.Anfangsinvestitionen = Convert.ToDecimal(r[11]);
                }

                c.Close();

            }


            //Berechnung der Werte für die Kaufpreiskalkulation
            decimal gesamtwohnfl = GetWohnflaeche(immoID);

            decimal flächekaufpreis = investdata.Kaufpreis / gesamtwohnfl;
            decimal maklerbetrag = investdata.Kaufpreis * investdata.Makler;
            decimal notarbetrag = investdata.Kaufpreis * investdata.Notar;
            decimal grundbuchbetrag = investdata.Kaufpreis * investdata.Grundbuchamt;
            decimal grunderwerbstbetrag = investdata.Kaufpreis * investdata.Grunderwerbsteuer;

            var summeKaufpreis = investdata.Kaufpreis + maklerbetrag + notarbetrag + grundbuchbetrag + grunderwerbstbetrag;

            decimal summeGesamtinvest = summeKaufpreis + investdata.Anfangsinvestitionen;


            //Schreiben der Werte in die Tabelle Investcalc
            using (var c = new SqlConnection(cn_string))
            {
                SqlCommand cmd = c.CreateCommand();

                cmd.CommandText = $"UPDATE dbo.Investcalc SET Maklerbetrag=@maklerbetrag, Notarbetrag=@notarbetrag, Grundbuchbetrag=@grundbuch, Grunderwerbetrag=@grunderwerb, SummeKaufpreis=@summekaufpreis, Gesamtinvestition=@gesamtinvestition WHERE ImmobilienID=@immoID;";

                cmd.Parameters.Add("@maklerbetrag", SqlDbType.Decimal, 20);
                cmd.Parameters.Add("@notarbetrag", SqlDbType.Decimal, 20);
                cmd.Parameters.Add("@grundbuch", SqlDbType.Decimal, 20);
                cmd.Parameters.Add("@grunderwerb", SqlDbType.Decimal, 20);
                cmd.Parameters.Add("@summekaufpreis", SqlDbType.Decimal, 20);
                cmd.Parameters.Add("@gesamtinvestition", SqlDbType.Decimal, 20);
                cmd.Parameters.Add("@immoID", SqlDbType.Int);

                cmd.Parameters["@maklerbetrag"].Value = maklerbetrag;
                cmd.Parameters["@notarbetrag"].Value = notarbetrag;
                cmd.Parameters["@grundbuch"].Value = grundbuchbetrag;
                cmd.Parameters["@grunderwerb"].Value = grunderwerbstbetrag;
                cmd.Parameters["@summekaufpreis"].Value = summeKaufpreis;
                cmd.Parameters["@gesamtinvestition"].Value = summeGesamtinvest;
                cmd.Parameters["@immoID"].Value = immoID;


                c.Open();

                var r = cmd.ExecuteNonQuery();

                c.Close();

            }

        }

        //Berechnet die Werte für die Mietcalctabelle und schreibt diese in Miettcalc
        private void Mietcompute(int immoID)
        {
            Mietdata mietdata = new Mietdata();

            //Connection-String
            string cn_string = "Server=localhost;Database=Immoman;Trusted_Connection=true;";

            using (var c = new SqlConnection(cn_string))
            {
                SqlCommand cmd = c.CreateCommand();

                //SQL-Befehl
                cmd.CommandText = $"SELECT * FROM dbo.Mietcalc WHERE ImmobilienID=@immoID;";

                cmd.Parameters.Add("@immoID", SqlDbType.Int);
                cmd.Parameters["@immoID"].Value = immoID;


                c.Open();

                SqlDataReader r = cmd.ExecuteReader();

                //Zuweisen der Werte in die Datenklasse Mietdata
                while (r.Read())
                {
                    mietdata.Kaltmiete = Convert.ToDecimal(r[1]);
                    mietdata.SonstigeMietaufwendungen = Convert.ToDecimal(r[3]);
                    mietdata.UmlageBewirtschaftskosten = Convert.ToDecimal(r[4]);
                    mietdata.Heizkosten = Convert.ToDecimal(r[5]);
                    mietdata.KostenWassAbwasser = Convert.ToDecimal(r[6]);
                }

                c.Close();
            }

            //Holen der Wohnfläche
            decimal gesamtwohnfl = GetWohnflaeche(immoID);

            //Berechnung der Werte für die Mieteinnahmenkalkulation
            decimal gesamWohnKaltmiete = mietdata.Kaltmiete * gesamtwohnfl;
            decimal nettokaltmiete = gesamWohnKaltmiete + mietdata.SonstigeMietaufwendungen;
            decimal warmmiete = nettokaltmiete + mietdata.UmlageBewirtschaftskosten;
            decimal warmieteinclnebenkosten = warmmiete+mietdata.Heizkosten + mietdata.KostenWassAbwasser;

            //Schreiben der Werte in die Tabelle Mietcalc
            using (var c = new SqlConnection(cn_string))
            {
                SqlCommand cmd = c.CreateCommand();

                cmd.CommandText = $"UPDATE dbo.Mietcalc SET KaltmieteGesamt=@gesamtKaltmiete, Nettokaltmiete=@nettokaltmiete, Warmmiete=@warmmiete, GesamtWarmmiete=@gesamtwarmmiete WHERE ImmobilienID=@immoID;";

                
                cmd.Parameters.Add("@gesamtKaltmiete", SqlDbType.Decimal, 20);
                cmd.Parameters.Add("@nettokaltmiete", SqlDbType.Decimal, 20);
                cmd.Parameters.Add("@warmmiete", SqlDbType.Decimal, 20);
                cmd.Parameters.Add("@gesamtwarmmiete", SqlDbType.Decimal, 20);
                cmd.Parameters.Add("@immoID", SqlDbType.Int);

                cmd.Parameters["@gesamtKaltmiete"].Value = gesamWohnKaltmiete;
                cmd.Parameters["@nettokaltmiete"].Value = nettokaltmiete;
                cmd.Parameters["@warmmiete"].Value = warmmiete;
                cmd.Parameters["@gesamtwarmmiete"].Value = warmieteinclnebenkosten;
                cmd.Parameters["@immoID"].Value = immoID;


                c.Open();

                var r = cmd.ExecuteNonQuery();

                c.Close();

            }

        }

        //Berechnet die Werte für die Umlagekosttabelle und schreibt diese in Umlagekostcalc
        private void Umlagekostcompute(int immoID)
        {
            Umlagekostdata umlagekostdata = new Umlagekostdata();

            //Connection-String
            string cn_string = "Server=localhost;Database=Immoman;Trusted_Connection=true;";

            //Aufbau der Verbindung
            using (var c = new SqlConnection(cn_string))
            {
                SqlCommand cmd = c.CreateCommand();

                //SQL-Befehl
                cmd.CommandText = $"SELECT * FROM dbo.Umlagekostcalc WHERE ImmobilienID=@immoID;";

                cmd.Parameters.Add("@immoID", SqlDbType.Int);
                cmd.Parameters["@immoID"].Value = immoID;


                c.Open();

                SqlDataReader r = cmd.ExecuteReader();

                //Zuweisen der Werte in die Datenklasse Umlagekostdata
                while (r.Read())
                {
                    umlagekostdata.HausgeldNichtUmlage = Convert.ToDecimal(r[1]);
                    umlagekostdata.Eigeninstandhaltungsrücklage = Convert.ToDecimal(r[2]);
                    umlagekostdata.SonstigeNichtumlagekosten = Convert.ToDecimal(r[3]);
                }

                c.Close();

            }

            //Berechnung der Werte
            decimal summeUmlagekost = umlagekostdata.HausgeldNichtUmlage + umlagekostdata.Eigeninstandhaltungsrücklage + umlagekostdata.SonstigeNichtumlagekosten;


            //Schreiben der Werte in die Tabelle Umlagekostcalc
            using (var c = new SqlConnection(cn_string))
            {
                SqlCommand cmd = c.CreateCommand();

                cmd.CommandText = $"UPDATE dbo.Umlagekostcalc SET GesamtUmlage = @summeUmlagekost WHERE ImmobilienID=@immoID;";


                cmd.Parameters.Add("@summeUmlagekost", SqlDbType.Decimal, 20);
                cmd.Parameters.Add("@immoID", SqlDbType.Int);

                cmd.Parameters["@summeUmlagekost"].Value = summeUmlagekost;
                cmd.Parameters["@immoID"].Value = immoID;


                c.Open();

                var r = cmd.ExecuteNonQuery();

                c.Close();

            }

        }

        //Berechnet die Werte für die Financtabelle und schreibt diese in Financcalc
        private void Financcompute(int immoID)
        {
            Financedata financedata = new Financedata();

            //Connection-String
            string cn_string = "Server=localhost;Database=Immoman;Trusted_Connection=true;";

            //Aufbau der Verbindung
            using (var c = new SqlConnection(cn_string))
            {
                SqlCommand cmd = c.CreateCommand();

                //SQL-Befehl
                cmd.CommandText = $"SELECT * FROM dbo.Financcalc WHERE ImmobilienID=@immoID;";

                cmd.Parameters.Add("@immoID", SqlDbType.Int);
                cmd.Parameters["@immoID"].Value = immoID;


                c.Open();

                SqlDataReader r = cmd.ExecuteReader();

                //Zuweisen der Werte in die Datenklasse Financedata
                while (r.Read())
                {
                    financedata.Darlehenssumme = Convert.ToDecimal(r[1]);
                    financedata.Zinssatz = Convert.ToDecimal(r[3]);
                    financedata.Tilgung = Convert.ToDecimal(r[5]);
                }

                c.Close();

            }

            //Holen des Wertes der Gesamtinvestition
            decimal gesamtinvest = GetGesamtinvest(immoID);

            //Berechnung der Werte
            decimal eigenkapital = gesamtinvest - financedata.Darlehenssumme;
            decimal zinsbetrag = financedata.Darlehenssumme * financedata.Zinssatz;
            decimal tilgungsbetrag = financedata.Darlehenssumme * financedata.Tilgung;
            decimal kapitaldienst = (zinsbetrag + tilgungsbetrag) / 12;

            //Schreiben der Werte in die Tabelle Financcalc
            using (var c = new SqlConnection(cn_string))
            {
                SqlCommand cmd = c.CreateCommand();

                //SQL-Befehl
                cmd.CommandText = $"UPDATE dbo.Financcalc SET Zinsbetrag=@zinsbetrag, Tilgungsbetrag=@tilgungsbetrag, Kapitaldienst=@kapitaldienst WHERE ImmobilienID=@immoID;";


                cmd.Parameters.Add("@zinsbetrag", SqlDbType.Decimal, 20);
                cmd.Parameters.Add("@tilgungsbetrag", SqlDbType.Decimal, 20);
                cmd.Parameters.Add("@kapitaldienst", SqlDbType.Decimal, 20);
                cmd.Parameters.Add("@immoID", SqlDbType.Int);

                cmd.Parameters["@zinsbetrag"].Value = zinsbetrag;
                cmd.Parameters["@tilgungsbetrag"].Value = tilgungsbetrag;
                cmd.Parameters["@kapitaldienst"].Value = kapitaldienst;
                cmd.Parameters["@immoID"].Value = immoID;


                c.Open();

                var r = cmd.ExecuteNonQuery();

                c.Close();

            }
        }

        //Berechnet die Werte für den Cashflow und schreibt diese in die Textbox
        private void Cashflowcompute(int immoID)
        {
            Cashflowdata cashflowdata = new Cashflowdata();

            //Connection-String
            string cn_string = "Server=localhost;Database=Immoman;Trusted_Connection=true;";
           
            //Aufbau der Verbindung
            using (var c = new SqlConnection(cn_string))
            {
                SqlCommand cmd = c.CreateCommand();

                //SQL-Befehl
                cmd.CommandText = $"SELECT Mietcalc.Warmmiete,Mietcalc.UmlageBewirtschaftskosten,Financcalc.Kapitaldienst FROM Mietcalc INNER JOIN Financcalc ON Mietcalc.ImmobilienID = Financcalc.ImmobilienID WHERE Financcalc.ImmobilienID = @immoID AND Mietcalc.ImmobilienID = @immoID;";

                cmd.Parameters.Add("@immoID", SqlDbType.Int);
                cmd.Parameters["@immoID"].Value = immoID;


                c.Open();

                SqlDataReader r = cmd.ExecuteReader();

                //Zuweisung der Daten in die Klasse Cashflowdata
                while (r.Read())
                {
                    cashflowdata.Warmmiete = Convert.ToDecimal(r[0]);
                    cashflowdata.Bewirtschaftungskosten = Convert.ToDecimal(r[1]);
                    cashflowdata.Kapitaldienst =  Convert.ToDecimal(r[2]);
                }

                c.Close();

                //Berechnung des Wertes
                decimal operCashflow = cashflowdata.Warmmiete - cashflowdata.Bewirtschaftungskosten - cashflowdata.Kapitaldienst;

                //Schreiben des Wertes in die Textbox
                detailoperativcashflow.Text = operCashflow.ToString();

            }

        }
    }
    
}
