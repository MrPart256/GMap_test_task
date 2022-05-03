using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask
{
    internal class DB
    {
        
        private const string connectionString = "Data Source=DESKTOP-IIB45SI\\SQLEXPRESS;Integrated Security=True";
        private static SqlConnection conn = new SqlConnection(connectionString);
      


        public static void AddMarker(Marker marker)
        {
            
            string request = $"INSERT INTO Table (Id,Lat,Lng,Vehicle) VALUES(@Id,@Lat,@Lng,@Vehicle)";
            Console.WriteLine(request);
            using (SqlCommand command = new SqlCommand(request, conn))
            {
                command.Parameters.AddWithValue("@Id", marker.id);
                command.Parameters.AddWithValue("@Lat", marker.lat);
                command.Parameters.AddWithValue("@Lng", marker.lng);
                command.Parameters.AddWithValue("@Vehicle", marker.vehicle);

                conn.Open();
                int result = command.ExecuteNonQuery();

                // Check Error
                if (result < 0)
                    Console.WriteLine("Error inserting data into Database!");
            }
            conn.Close();
        }

        public static List<Marker> GetMarkers()
        {
            string request = "SELECT * FROM Table";
            List<Marker> markers = new List<Marker>();
            SqlCommand cmd = new SqlCommand(request, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Marker marker = new Marker();
                marker.id = reader.GetString(0);
                marker.lat = (double)reader.GetDecimal(1);
                marker.lng = (double)reader.GetDecimal(2);
                marker.vehicle = reader.GetString(3);
                markers.Add(marker);
            }
            reader.Close();
            conn.Close();
            return markers;
        }
    }
}
