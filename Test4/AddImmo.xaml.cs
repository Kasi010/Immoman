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
    public partial class Add : Window
    {
        public Add()
        {
            InitializeComponent();
        }

        private void OnImoAdd(object sender, RoutedEventArgs e)
        {
            string cn_string = "Server=localhost;Database=Immoman;Trusted_Connection=true;";

            imotableAdd(cn_string);
            //investcalcAdd(cn_string);
            finanzcalcAdd(cn_string);
            mietcalcAdd(cn_string);
            umlagekostcalcAdd(cn_string);
           
        }
        private void imotableAdd(string cn_string)
        {
            //Immobilientabelle
            string strasse = imostreet.Text.Trim();
            string plz = imoplz.Text.Trim();
            string hausno = imohausnummer.Text.Trim();
            string ort = imoort.Text.Trim();
            string wohnfl = imowohnfläche.Text.Trim();

            using (var c = new SqlConnection(cn_string))
            {
                SqlCommand cmd = c.CreateCommand();

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
        private void investcalcAdd(string cn_string)
        {
            //Investcalc
            string kaufpreis = imokaufpreis.Text.Trim();
            string makler = imomaklersatz.Text.Trim();
            string notar = imonotarsatz.Text.Trim();
            string grundbuchamt = imogrundbuchsatz.Text.Trim();
            string grunderwerbsteuer = imogrunderwerbssteuer.Text.Trim();
            string anfinvest = imoanfangsinvest.Text.Trim();

            int immoID = Convert.ToInt32(ImmoMethods.GetLastEntry());

            using (var c = new SqlConnection(cn_string))
            {
                SqlCommand cmd = c.CreateCommand();

                cmd.CommandText = $"INSERT INTO dbo.Investcalc (Kaufpreis,Makler,Notar,Grundbuchamt,Grunderwerbsteuer,Anfangsinvestitionen,ImmobilienID) VALUES (@kaufpreis,@makler/100,@notar/100,@grundbuchamt/100,@grunderwerbssteuer/100,@anfangsinvest,@immoID);";

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
        private void finanzcalcAdd(string cn_string)
        {
            //Financcalc
            string darlehenssum = imodarlehen.Text.Trim();

            int immoID = Convert.ToInt32(ImmoMethods.GetLastEntry());

            using (var c = new SqlConnection(cn_string))
            {
                SqlCommand cmd = c.CreateCommand();

                cmd.CommandText = $"INSERT INTO dbo.Financcalc (Darlehenssumme,ImmobilienID) VALUES (@darlehenssumme,@immoID);";

                cmd.Parameters.Add("@darlehenssumme", SqlDbType.Decimal, 20);
                cmd.Parameters.Add("@immoID", SqlDbType.Int);

                cmd.Parameters["@darlehenssumme"].Value = darlehenssum;
                cmd.Parameters["@immoID"].Value = immoID;


                c.Open();

                var i = cmd.ExecuteNonQuery();

                c.Close();

            }

        }
        private void mietcalcAdd(string cn_string)
        {
            //Mietcalc
            string kaltmiete = imokaltmiete.Text.Trim();
            string sonstiges = imosonstiges.Text.Trim();
            string umlgbewirtkosten = imobewirtschaftung.Text.Trim();
            string heizkosten = imoheizkosten.Text.Trim();
            string wasser_abwasser = imowasserabwasser.Text.Trim();

            int immoID = Convert.ToInt32(ImmoMethods.GetLastEntry());

            using (var c = new SqlConnection(cn_string))
            {
                SqlCommand cmd = c.CreateCommand();

                cmd.CommandText = $"INSERT INTO dbo.Mietcalc (Kaltmiete,SonstigeMietaufwendungen,UmlageBewirtschaftskosten,Heizkosten,WasserAbwasserKosten,ImmobilienID) VALUES (@kaltmiete,@sonstiges,@umlgbewirtkosten,@heizkosten,@wasser_abwasser,@immoID);";

                cmd.Parameters.Add("@kaltmiete", SqlDbType.Decimal, 10);
                cmd.Parameters.Add("@sonstiges", SqlDbType.Decimal, 20);
                cmd.Parameters.Add("@umlgbewirtkosten", SqlDbType.Decimal, 20);
                cmd.Parameters.Add("@heizkosten", SqlDbType.Decimal, 10);
                cmd.Parameters.Add("@wasser_abwasser", SqlDbType.Decimal, 10);
                cmd.Parameters.Add("@immoID", SqlDbType.Int);

                cmd.Parameters["@kaltmiete"].Value = kaltmiete;
                cmd.Parameters["@sonstiges"].Value = sonstiges;
                cmd.Parameters["@umlgbewirtkosten"].Value = umlgbewirtkosten;
                cmd.Parameters["@heizkosten"].Value = heizkosten;
                cmd.Parameters["@wasser_abwasser"].Value = wasser_abwasser;

                cmd.Parameters["@immoID"].Value = immoID;


                c.Open();

                var i = cmd.ExecuteNonQuery();

                c.Close();

            }
        }
        private void umlagekostcalcAdd(string cn_string)
        {
            //Umlagekostcalc
            string hausgeld = imohausgeld.Text.Trim();
            string instandgsrückl = imoinstandhaltungsrück.Text.Trim();

            int immoID = Convert.ToInt32(ImmoMethods.GetLastEntry());

            using (var c = new SqlConnection(cn_string))
            {
                SqlCommand cmd = c.CreateCommand();

                cmd.CommandText = $"INSERT INTO dbo.Umlagekostcalc (HausgeldNichtUmlage,Eigeninstandhaltungsrücklage,ImmobilienID) VALUES (@hausgeld,@instandgsrückl,@immoID);";

                cmd.Parameters.Add("@hausgeld", SqlDbType.Decimal, 20);
                cmd.Parameters.Add("@instandgsrückl", SqlDbType.Decimal, 20);
                cmd.Parameters.Add("@immoID", SqlDbType.Int);

                cmd.Parameters["@hausgeld"].Value = hausgeld;
                cmd.Parameters["@instandgsrückl"].Value = instandgsrückl;
                cmd.Parameters["@immoID"].Value = immoID;


                c.Open();

                var i = cmd.ExecuteNonQuery();

                c.Close();

            }

        }
    }
}
