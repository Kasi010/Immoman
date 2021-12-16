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
        public Details(int immoID)
        {
            InitializeComponent();
            this.ImmoID = immoID;
            Investcompute(immoID);
        }
        public int ImmoID { get; set; }
        private decimal GetWohnflaeche(int immoID)
        {
            string cn_string = "Server=localhost;Database=Immoman;Trusted_Connection=true;";

            using (var c = new SqlConnection(cn_string))
            {
                SqlCommand cmd = c.CreateCommand();

                cmd.CommandText = $"SELECT Wohnflaeche FROM Immobilie WHERE ImmobilienID = @immoID; ";

                cmd.Parameters.Add("@immoID", SqlDbType.Int);
                cmd.Parameters["@immoID"].Value = immoID;


                c.Open();

                var i = Convert.ToDecimal(cmd.ExecuteScalar());

                c.Close();

                return i;

            }

        }
        private decimal GetGesamtinvest(int immoID)
        {
            string cn_string = "Server=localhost;Database=Immoman;Trusted_Connection=true;";

            using (var c = new SqlConnection(cn_string))
            {
                SqlCommand cmd = c.CreateCommand();

                cmd.CommandText = $"SELECT Gesamtinvestition FROM Immobilie WHERE ImmobilienID = @immoID; ";

                cmd.Parameters.Add("@immoID", SqlDbType.Int);
                cmd.Parameters["@immoID"].Value = immoID;


                c.Open();

                var i = Convert.ToDecimal(cmd.ExecuteScalar());

                c.Close();

                return i;

            }
        }
        private void Investcompute(int immoID)
        {
            Investdata investdata = new Investdata();

            string cn_string = "Server=localhost;Database=Immoman;Trusted_Connection=true;";

            using (var c = new SqlConnection(cn_string))
            {
                SqlCommand cmd = c.CreateCommand();

                cmd.CommandText = $"SELECT * FROM Investcalc WHERE ImmobilienID=@immoID;";

                cmd.Parameters.Add("@immoID", SqlDbType.Int);
                cmd.Parameters["@immoID"].Value = immoID;


                c.Open();

                SqlDataReader r = cmd.ExecuteReader();

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

            decimal gesamtwohnfl = GetWohnflaeche(immoID);

            decimal flächekaufpreis = investdata.Kaufpreis / gesamtwohnfl;
            decimal maklerbetrag = investdata.Kaufpreis * investdata.Makler;
            decimal notarbetrag = investdata.Kaufpreis * investdata.Notar;
            decimal grundbuchbetrag = investdata.Kaufpreis * investdata.Grundbuchamt;
            decimal grunderwerbstbetrag = investdata.Kaufpreis * investdata.Grunderwerbsteuer;

            var summeKaufpreis = investdata.Kaufpreis + maklerbetrag + notarbetrag + grundbuchbetrag + grunderwerbstbetrag;

            decimal summeGesamtinvest = summeKaufpreis + investdata.Anfangsinvestitionen;

            using (var c = new SqlConnection(cn_string))
            {
                SqlCommand cmd = c.CreateCommand();

                cmd.CommandText = $"UPDATE Investcalc SET Maklerbetrag=@maklerbetrag, Notarbetrag=@notarbetrag, Grundbuchbetrag=@grundbuch, Grunderwerbetrag=@grunderwerb, SummeKaufpreis=@summekaufpreis, Gesamtinvestition=@gesamtinvestition WHERE ImmobilienID=@immoID;";

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

            //Ausgabe hinzufügen





        }
        private void Mietcompute(int immoID)
        {
            Mietdata mietdata = new Mietdata();

            string cn_string = "Server=localhost;Database=Immoman;Trusted_Connection=true;";

            using (var c = new SqlConnection(cn_string))
            {
                SqlCommand cmd = c.CreateCommand();

                cmd.CommandText = $"SELECT * FROM Mietcalc WHERE ImmobilienID=@immoID;";

                cmd.Parameters.Add("@immoID", SqlDbType.Int);
                cmd.Parameters["@immoID"].Value = immoID;


                c.Open();

                SqlDataReader r = cmd.ExecuteReader();

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

            decimal gesamtwohnfl = GetWohnflaeche(immoID);

            decimal gesamKaltmiete = mietdata.Kaltmiete * gesamtwohnfl;
            decimal nettokaltmiete = gesamKaltmiete + mietdata.SonstigeMietaufwendungen;
            decimal warmmiete = nettokaltmiete + mietdata.UmlageBewirtschaftskosten;
            decimal warmieteinclnebenkosten = mietdata.Heizkosten + mietdata.KostenWassAbwasser;

            using (var c = new SqlConnection(cn_string))
            {
                SqlCommand cmd = c.CreateCommand();

                cmd.CommandText = $"UPDATE Mietcalc SET KaltmieteGesamt=@gesamtKaltmiete, Nettokaltmiete=@nettokaltmiete, Warmmiete=@warmmiete, GesamtWarmmiete=@gesamtwarmmiete WHERE ImmobilienID=@immoID;";

                
                cmd.Parameters.Add("@gesamtKaltmiete", SqlDbType.Decimal, 20);
                cmd.Parameters.Add("@nettokaltmiete", SqlDbType.Decimal, 20);
                cmd.Parameters.Add("@warmmiete", SqlDbType.Decimal, 20);
                cmd.Parameters.Add("@gesamtwarmmiete", SqlDbType.Decimal, 20);
                cmd.Parameters.Add("@immoID", SqlDbType.Int);

                cmd.Parameters["@gesamtKaltmiete"].Value = gesamKaltmiete;
                cmd.Parameters["@nettokaltmiete"].Value = nettokaltmiete;
                cmd.Parameters["@warmmiete"].Value = warmmiete;
                cmd.Parameters["@gesamtwarmmiete"].Value = warmieteinclnebenkosten;
                cmd.Parameters["@immoID"].Value = immoID;


                c.Open();

                var r = cmd.ExecuteNonQuery();

                c.Close();

            }

            //Ausgabe bauen



        }
        private void Umlagekostcompute(int immoID)
        {
            Umlagekostdata umlagekostdata = new Umlagekostdata();

            string cn_string = "Server=localhost;Database=Immoman;Trusted_Connection=true;";

            using (var c = new SqlConnection(cn_string))
            {
                SqlCommand cmd = c.CreateCommand();

                cmd.CommandText = $"SELECT * FROM Umlagekostcalc WHERE ImmobilienID=@immoID;";

                cmd.Parameters.Add("@immoID", SqlDbType.Int);
                cmd.Parameters["@immoID"].Value = immoID;


                c.Open();

                SqlDataReader r = cmd.ExecuteReader();

                while (r.Read())
                {
                    umlagekostdata.HausgeldNichtUmlage = Convert.ToDecimal(r[1]);
                    umlagekostdata.Eigeninstandhaltungsrücklage = Convert.ToDecimal(r[2]);
                    umlagekostdata.SonstigeNichtumlagekosten = Convert.ToDecimal(r[3]);
                }

                c.Close();

            }

            decimal summeUmlagekost = umlagekostdata.HausgeldNichtUmlage + umlagekostdata.Eigeninstandhaltungsrücklage + umlagekostdata.SonstigeNichtumlagekosten;

            using (var c = new SqlConnection(cn_string))
            {
                SqlCommand cmd = c.CreateCommand();

                cmd.CommandText = $"UPDATE Mietcalc SET GesasmtUmlagekost=@summeUmlagekost WHERE ImmobilienID=@immoID;";


                cmd.Parameters.Add("@summeUmlagekost", SqlDbType.Decimal, 20);
                cmd.Parameters.Add("@immoID", SqlDbType.Int);

                cmd.Parameters["@summeUmlagekost"].Value = summeUmlagekost;
                cmd.Parameters["@immoID"].Value = immoID;


                c.Open();

                var r = cmd.ExecuteNonQuery();

                c.Close();

            }

            //Ausgabe holen


        }
        private void Financcompute(int immoID)
        {
            Financedata financedata = new Financedata();

            string cn_string = "Server=localhost;Database=Immoman;Trusted_Connection=true;";

            using (var c = new SqlConnection(cn_string))
            {
                SqlCommand cmd = c.CreateCommand();

                cmd.CommandText = $"SELECT * FROM Financcalc WHERE ImmobilienID=@immoID;";

                cmd.Parameters.Add("@immoID", SqlDbType.Int);
                cmd.Parameters["@immoID"].Value = immoID;


                c.Open();

                SqlDataReader r = cmd.ExecuteReader();

                while (r.Read())
                {
                    financedata.Darlehenssumme = Convert.ToDecimal(r[1]);
                    financedata.Zinssatz = Convert.ToDecimal(r[3]);
                    financedata.Tilgung = Convert.ToDecimal(r[5]);
                }

                c.Close();

            }

            decimal gesamtinvest = GetGesamtinvest(immoID);

            decimal eigenkapital = gesamtinvest - financedata.Darlehenssumme;
            decimal zinsbetrag = financedata.Darlehenssumme * financedata.Zinssatz;
            decimal tilgungsbetrag = financedata.Darlehenssumme * financedata.Tilgung;
            decimal kapitaldienst = (zinsbetrag + tilgungsbetrag) / 12;

            using (var c = new SqlConnection(cn_string))
            {
                SqlCommand cmd = c.CreateCommand();

                cmd.CommandText = $"UPDATE Financcalc SET Zinsbetrag=@zinsbetrag, Tilgungsbetrag=@tilgungsbetrag, Kapitaldienst=@kapitaldienst WHERE ImmobilienID=@immoID;";


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

            //Ausgabe bauen





        }
    }
    
}
