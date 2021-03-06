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

namespace Test4
{
    /// <summary>
    /// Interaktionslogik für Add.xaml
    /// </summary>
    public partial class AddImmo : Window
    {
        /*Fenster zur Dateneingabe von einer Immobilie. Die Daten werden anschließend auf den SQL-Server geschrieben*/
        public AddImmo()
        {
            InitializeComponent();
        }

        //Wird ausgelöst durch den Button 'Hinzufügen'
        private void OnImoAdd(object sender, RoutedEventArgs e)
        {
            //Connection-String zum Verbindungsaufbau an den SQL-Server
            string cn_string = "Server=localhost;Database=Immoman;Trusted_Connection=true;";

            imotableAdd(cn_string); 
            investcalcAdd(cn_string);
            finanzcalcAdd(cn_string);
            mietcalcAdd(cn_string);
            umlagekostcalcAdd(cn_string);

            MessageBox.Show("Datensatz wurde erfolgreich angelegt");
           
        }

        //Schreibt Daten der Adresse in die Immobilientabelle
        private void imotableAdd(string cn_string)
        {
            //Holen der Daten
            string strasse = imostreet.Text.Trim();
            string plz = imoplz.Text.Trim();
            string hausno = imohausnummer.Text.Trim();
            string ort = imoort.Text.Trim();
            string wohnfl = imowohnfläche.Text.Trim();

            //Aufbau der Verbindung und Einfügen der Daten
            using (var c = new SqlConnection(cn_string))
            {
                SqlCommand cmd = c.CreateCommand();

                //SQL-Befehl
                cmd.CommandText = $"INSERT INTO dbo.Immobilie (Straße, PLZ, Hausnummer, Ort, Wohnflaeche) VALUES (@strasse, @plz, @hausno, @ort, @wohnfl);";

                cmd.Parameters.Add("@strasse", SqlDbType.VarChar, 50);
                cmd.Parameters.Add("@plz", SqlDbType.Char, 5);
                cmd.Parameters.Add("@hausno", SqlDbType.VarChar, 5);
                cmd.Parameters.Add("@ort", SqlDbType.VarChar, 50);
                cmd.Parameters.Add("@wohnfl", SqlDbType.Decimal, 8);

                cmd.Parameters["@strasse"].Value = strasse;
                cmd.Parameters["@plz"].Value = plz;
                cmd.Parameters["@hausno"].Value = hausno;
                cmd.Parameters["@ort"].Value = ort;
                cmd.Parameters["@wohnfl"].Value = wohnfl;
           
                c.Open();

                var i = cmd.ExecuteNonQuery();

                c.Close();

            }
        }

        //Schreibt Daten der Investitionsdaten in die Investcalc-Tabelle
        private void investcalcAdd(string cn_string)
        {
            //Holen der Daten
            string kaufpreis = imokaufpreis.Text.Trim();
            string makler = imomaklersatz.Text.Trim();
            string notar = imonotarsatz.Text.Trim();
            string grundbuchamt = imogrundbuchsatz.Text.Trim();
            string grunderwerbsteuer = imogrunderwerbssteuer.Text.Trim();
            string anfinvest = imoanfangsinvest.Text.Trim();

            //Holen der Immobilien-ID des zuletzt hinzugefügten Datensatzes
            int immoID = Convert.ToInt32(ImmoMethods.GetLastEntry());

            //Aufbau der Verbindung
            using (var c = new SqlConnection(cn_string))
            {
                SqlCommand cmd = c.CreateCommand();

                //SQL-Befehl
                cmd.CommandText = "INSERT INTO dbo.Investcalc (Kaufpreis,Makler,Notar,Grundbuchamt,Grunderwerbsteuer,Anfangsinvestitionen,ImmobilienID) VALUES (@kaufpreis,@makler/100,@notar/100,@grundbuchamt/100,@grunderwerbssteuer/100,@anfangsinvest,@immoID);";

                cmd.Parameters.Add("@kaufpreis", SqlDbType.Decimal, 20);
                cmd.Parameters.Add("@makler", SqlDbType.Decimal, 20);
                cmd.Parameters.Add("@notar", SqlDbType.Decimal, 20);
                cmd.Parameters.Add("@grundbuchamt", SqlDbType.Decimal, 20);
                cmd.Parameters.Add("@grunderwerbssteuer", SqlDbType.Decimal, 20);
                cmd.Parameters.Add("@anfangsinvest", SqlDbType.Decimal, 15);
                cmd.Parameters.Add("@immoID", SqlDbType.Int);

                cmd.Parameters["@kaufpreis"].Value = kaufpreis;
                cmd.Parameters["@makler"].Value = makler;
                cmd.Parameters["@notar"].Value = notar;
                cmd.Parameters["@grundbuchamt"].Value = grundbuchamt;
                cmd.Parameters["@grunderwerbssteuer"].Value = grunderwerbsteuer;
                cmd.Parameters["@anfangsinvest"].Value = anfinvest;
                cmd.Parameters["@immoID"].Value = immoID;


                c.Open();

                var i = cmd.ExecuteNonQuery();

                c.Close();

            }
        }

        //Schreibt Daten der Finanzdaten in die Financcalc-Tabelle
        private void finanzcalcAdd(string cn_string)
        {
            //Holen der Daten
            string darlehenssum = imodarlehen.Text.Trim();
            string zinssatz = imozins.Text.Trim();
            string tilgungssatz = imotilgung.Text.Trim();


            //Holen der Immobilien-ID des zuletzt hinzugefügten Datensatzes
            int immoID = Convert.ToInt32(ImmoMethods.GetLastEntry());

            //Aufbau der Verbindung
            using (var c = new SqlConnection(cn_string))
            {
                SqlCommand cmd = c.CreateCommand();

                //SQL-Befehl
                cmd.CommandText = $"INSERT INTO dbo.Financcalc (Darlehenssumme,Zinssatz,Tilgung,ImmobilienID) VALUES (@darlehenssumme,@zinssatz/100,@tilgungssatz/100,@immoID);";

                cmd.Parameters.Add("@darlehenssumme", SqlDbType.Decimal, 20);
                cmd.Parameters.Add("@zinssatz", SqlDbType.Decimal, 20);
                cmd.Parameters.Add("@tilgungssatz", SqlDbType.Decimal, 20);
                cmd.Parameters.Add("@immoID", SqlDbType.Int);

                cmd.Parameters["@darlehenssumme"].Value = darlehenssum;
                cmd.Parameters["@zinssatz"].Value = zinssatz;
                cmd.Parameters["@tilgungssatz"].Value = tilgungssatz;
                cmd.Parameters["@immoID"].Value = immoID;


                c.Open();

                var i = cmd.ExecuteNonQuery();

                c.Close();

            }

        }

        //Schreibt Daten der Mietdaten in die Mietcalc-Tabelle
        private void mietcalcAdd(string cn_string)
        {
            //Holen der Daten
            string kaltmiete = imokaltmiete.Text.Trim();
            string mietsonstiges = imomietsonstiges.Text.Trim();
            string umlgbewirtkosten = imobewirtschaftung.Text.Trim();
            string heizkosten = imoheizkosten.Text.Trim();
            string wasserabwasser = imowasserabwasser.Text.Trim();

            //Holen der Immobilien-ID des zuletzt hinzugefügten Datensatzes
            int immoID = Convert.ToInt32(ImmoMethods.GetLastEntry());

            //Aufbau der Verbindung
            using (var c = new SqlConnection(cn_string))
            {
                SqlCommand cmd = c.CreateCommand();

                //SQL-Befehl
                cmd.CommandText = $"INSERT INTO dbo.Mietcalc (Kaltmiete,SonstigeMietaufwendungen,UmlageBewirtschaftskosten,Heizkosten,KostenWassAbwasser,ImmobilienID) VALUES (@kaltmiete,@mietsonstiges,@umlgbewirtkosten,@heizkosten,@wasserabwasser,@immoID);";

                cmd.Parameters.Add("@kaltmiete", SqlDbType.Decimal, 10);
                cmd.Parameters.Add("@mietsonstiges", SqlDbType.Decimal, 20);
                cmd.Parameters.Add("@umlgbewirtkosten", SqlDbType.Decimal, 20);
                cmd.Parameters.Add("@heizkosten", SqlDbType.Decimal, 10);
                cmd.Parameters.Add("@wasserabwasser", SqlDbType.Decimal, 10);
                cmd.Parameters.Add("@immoID", SqlDbType.Int);

                cmd.Parameters["@kaltmiete"].Value = kaltmiete;
                cmd.Parameters["@mietsonstiges"].Value = mietsonstiges;
                cmd.Parameters["@umlgbewirtkosten"].Value = umlgbewirtkosten;
                cmd.Parameters["@heizkosten"].Value = heizkosten;
                cmd.Parameters["@wasserabwasser"].Value = wasserabwasser;

                cmd.Parameters["@immoID"].Value = immoID;


                c.Open();

                var i = cmd.ExecuteNonQuery();

                c.Close();

            }
        }

        //Schreibt Daten der Umlagekostendaten in die Umlagekostcalc-Tabelle
        private void umlagekostcalcAdd(string cn_string)
        {
            //Holen der Daten
            string hausgeld = imohausgeld.Text.Trim();
            string instandgsrückl = imoinstandhaltungsrück.Text.Trim();
            string sonsumlagekost = imosonsumlagekost.Text.Trim();

            //Holen der Immobilien-ID des zuletzt hinzugefügten Datensatzes
            int immoID = Convert.ToInt32(ImmoMethods.GetLastEntry());

            //Aufbau der Verbindung
            using (var c = new SqlConnection(cn_string))
            {
                SqlCommand cmd = c.CreateCommand();

                //SQL-Befehl
                cmd.CommandText = $"INSERT INTO dbo.Umlagekostcalc (HausgeldNichtUmlage,Eigeninstandhaltungsrücklage,SonstigeNichtumlagekosten,ImmobilienID) VALUES (@hausgeld,@instandgsrückl,@sonsumlagekost,@immoID);";

                cmd.Parameters.Add("@hausgeld", SqlDbType.Decimal, 20);
                cmd.Parameters.Add("@instandgsrückl", SqlDbType.Decimal, 20);
                cmd.Parameters.Add("@sonsumlagekost", SqlDbType.Decimal, 20);
                cmd.Parameters.Add("@immoID", SqlDbType.Int);

                cmd.Parameters["@hausgeld"].Value = hausgeld;
                cmd.Parameters["@instandgsrückl"].Value = instandgsrückl;
                cmd.Parameters["@sonsumlagekost"].Value = sonsumlagekost;
                cmd.Parameters["@immoID"].Value = immoID;


                c.Open();

                var i = cmd.ExecuteNonQuery();

                c.Close();

            }

        }
    }
}
