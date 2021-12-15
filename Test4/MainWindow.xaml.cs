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

namespace Test4
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ListBox_Loaded(object sender, RoutedEventArgs e)
        {
            liste_laden();
        }

        private void liste_laden()
        {
            string sSQL = "SELECT * FROM Immobilie";
            string cn_string = "Server=localhost;Database=Immoman;Trusted_Connection=true;";

            List<Immobilie> immob = new List<Immobilie>();

            using (var c = new SqlConnection(cn_string))
            {
                SqlCommand cmd = c.CreateCommand();
                cmd.CommandText = sSQL;

                c.Open();

                SqlDataReader r = cmd.ExecuteReader();

                //List<string> columnhead = ImmoMethods.ColumnNames(r);

                //for (int i = 0; i < columnhead.Count; i++)
                //{
                //    GridViewColumn viewColumn = new GridViewColumn();
                //    viewColumn.Header = columnhead[i];
                //}

                while (r.Read())
                {
                    Immobilie immo = new Immobilie();

                    immo.ImmobilienID = Convert.ToInt32(r[0]);
                    immo.Straße = Convert.ToString(r[1]);
                    immo.PLZ = Convert.ToString(r[2]);
                    immo.Hausnummer = Convert.ToString(r[3]);
                    immo.Ort = Convert.ToString(r[4]);
                    immo.Wohnflaeche = Convert.ToDecimal(r[5]);

                    immob.Add(immo);
                }

                lstImmo.ItemsSource = immob;

                c.Close();

            }
        }

        private void OnAdd(object sender, RoutedEventArgs e)
        {
            var window = new Add();
            window.Show();
        }
    }
}
