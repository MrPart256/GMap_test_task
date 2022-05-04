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
        
        private const string connectionString = "Data Source=DESKTOP-IIB45SI\\SQLEXPRESS;Initial Catalog=TaskDB;Integrated Security=True";



        public static void AddMarker(Marker marker)
        {
            SqlConnection conn = new SqlConnection(connectionString);
           
            SqlCommand command = new SqlCommand("INSERT INTO [Database] (Id, Lat, Lng, Vehicle) VALUES(@Id,@Lat,@Lng,@Vehicle)", conn);
            command.Parameters.AddWithValue("@Id", marker.id);
            command.Parameters.AddWithValue("@Lat", marker.lat);
            command.Parameters.AddWithValue("@Lng", marker.lng);
            command.Parameters.AddWithValue("@Vehicle", marker.vehicle);

            try
            {
                conn.Open();
                command.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                Console.WriteLine("Exception " + e.ToString());
            }
            finally
            {
                conn.Close();
            }
        }

        public static void UpdateMarkerPosition(Marker marker)
        {
            SqlConnection conn = new SqlConnection(connectionString);


            SqlCommand command = new SqlCommand("UPDATE [Database] set Lat=@Lat, Lng=@Lng WHERE Id=@Id", conn);

            command.Parameters.AddWithValue("@Lat", marker.lat);
            command.Parameters.AddWithValue("@Lng", marker.lng);
            command.Parameters.AddWithValue("@Id", marker.id);

            try
            {
                conn.Open();
                command.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                Console.WriteLine("Exception " + e.ToString());
            }
            finally
            {
                conn.Close();
            }

        }

        public static List<Marker> GetMarkers()
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            List<Marker> markers = new List<Marker>();
            SqlCommand command = new SqlCommand("SELECT * FROM [Database]", conn);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Marker marker = new Marker();
                marker.id = reader.GetString(0);
                marker.lat = (double)reader.GetValue(1);
                marker.lng = (double)reader.GetValue(2);
                marker.vehicle = reader.GetString(3);
                markers.Add(marker);
            }
            reader.Close();
            conn.Close();
            return markers;
        }
    }
}
