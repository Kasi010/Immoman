using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test4
{
    //Methoden zur Verwendung innerhalb des Codes. Nur Get-LastEntry wird verwendet
    public class ImmoMethods
    {
        //Holt Spaltennamen der ausgelesenen Tabelle
        public static List<string> ColumnNames(SqlDataReader r)
        {
            List<string> names = new List<string>();

            for (int i = 0; i < r.FieldCount; i++)
            {
                names.Add(r.GetName(i));
            }

            return names;
        }

        //Liest Werte aus und gibt diese zurück. Funktioniert nicht
        public static SqlDataReader DataRead(string sql_command)
        {
            string cn_string = "Server=localhost;Database=Immoman;Trusted_Connection=true;";
            using (var c = new SqlConnection(cn_string))
            {
                SqlCommand cmd = c.CreateCommand();
                cmd.CommandText = sql_command;

                c.Open();

                SqlDataReader r = cmd.ExecuteReader();

                c.Close();
                return r;
                

            }
        }

        //Liest einen Wert und gibt diesen zurück. Funktioniert nicht
        public static object DataReadScalar(string sql_command)
        {
            string cn_string = "Server=localhost;Database=Immoman;Trusted_Connection=true;";
            using (var c = new SqlConnection(cn_string))
            {
                SqlCommand cmd = c.CreateCommand();
                cmd.CommandText = sql_command;

                c.Open();

                object r = cmd.ExecuteScalar();

                c.Close();
                return r;


            }
        }

        //Holt den Wert der ImmobilienID der zuletzt hinzugefügten Immobilie
        public static object GetLastEntry()
        {
            string sqlcommand = $"SELECT TOP 1 (ImmobilienID) FROM Immobilie ORDER BY ImmobilienID DESC;";
            string cn_string = "Server=localhost;Database=Immoman;Trusted_Connection=true;";

            using (var c = new SqlConnection(cn_string))
            {
                SqlCommand cmd = c.CreateCommand();
                cmd.CommandText = sqlcommand;

                c.Open();

                object r = cmd.ExecuteScalar();

                c.Close();
                return r;

            }


        }
    }
   
}
