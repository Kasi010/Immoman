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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Test4.DataClass;

namespace Test4
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /*Hauptfenster auf welchem die Immobillien dargestellt werden*/
        public MainWindow()
        {
            InitializeComponent();
        }

        //Aktion die beim Aufruf der ListView ausgelöst wird
        private void ListView_Loaded(object sender, RoutedEventArgs e)
        {
            Liste_laden();
        }

        //Methode, welche die Immobilien ausliest und diese in die ListView schreibt
        private void Liste_laden()
        {
            //SQL-Befehl
            string sSQL = "SELECT * FROM Immobilie";

            //Connection-String
            string cn_string = "Server=localhost;Database=Immoman;Trusted_Connection=true;";

            List<Immobilie> immolist = new List<Immobilie>();

            using (var c = new SqlConnection(cn_string))
            {
                SqlCommand cmd = c.CreateCommand();
                cmd.CommandText = sSQL;

                c.Open();

                SqlDataReader r = cmd.ExecuteReader();

                //Schreiben der ausgelesenen Werte aus Immobilie in die ListView
                while (r.Read())
                {
                    Immobilie immo = new Immobilie();

                    immo.ImmobilienID = Convert.ToInt32(r[0]);
                    immo.Straße = Convert.ToString(r[1]);
                    immo.PLZ = Convert.ToString(r[2]);
                    immo.Hausnummer = Convert.ToString(r[3]);
                    immo.Ort = Convert.ToString(r[4]);
                    immo.Wohnflaeche = Convert.ToDecimal(r[5]);

                    immolist.Add(immo);
                }

                lstImmo.ItemsSource = immolist;

                c.Close();

            }
        }

        //Wird ausgelöst auf den Button 'Hinzufügen' und ruft ein neues Fenster 'AddImmo' auf
        private void OnAdd(object sender, RoutedEventArgs e)
        {
            var window = new AddImmo();
            window.Show();
        }

        //Wird aufgelöst per Doppelklick auf ein Element der LiestView und ruft ein neues Fenster 'Details' auf
        private void OnDouble(object sender, MouseButtonEventArgs e)
        {
            //Speichern der Daten des selektierten Item in einer Immobilienklasse
            Immobilie immo = (Immobilie)lstImmo.SelectedItem;

            //Auslesen der selektierten ImmoID
            int immoID = immo.ImmobilienID;

            //Aufruf des Detailfensters und Übergabe der ausgelesenen ImmoID
            var window = new Details(immoID);


            window.Show();
            
        }

        //Aktualisiert die Liste in der ListView zur Laufzeit
        private void OnRefresh(object sender, RoutedEventArgs e)
        {
            Liste_laden();
        }

        ////Wird ausgelöst auf den Button 'Hinzufügen' im Menü 'Datei' und ruft ein neues Fenster 'AddImmo' auf
        private void OnAddmini(object sender, RoutedEventArgs e)
        {
            var window = new AddImmo();
            window.Show();
        }
    }
}
