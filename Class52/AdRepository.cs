using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class52.data
{
    public class AdRepository
    {
        private string _connectionString;

        public AdRepository (string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Ad> GetAds ()
        {
            using SqlConnection connection = new SqlConnection(_connectionString );
            using SqlCommand command = connection.CreateCommand ();
            command.CommandText = "SELECT * FROM Posts ORDER BY DateCreated DESC";
            connection.Open();
            SqlDataReader reader = command.ExecuteReader ();
            var Ads = new List<Ad> ();
            while ( reader.Read ())
            {
                Ads.Add(new Ad
                {
                    AdId = (int)reader["id"],
                    UserId= (int)reader["userId"],
                    Name= (string)reader["name"],
                    Description = (string)reader["description"],
                    PhoneNumber = (string)reader["phoneNumber"],
                    DateCreated = (DateTime)reader["dateCreated"]
                });
            }
            return Ads;
        }

        public void NewAd(Ad ad)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Posts VALUES (@userId, GETDATE(), @name, @phoneNumber, @description)";
            command.Parameters.AddWithValue("@userId", ad.UserId);
            command.Parameters.AddWithValue("@name", ad.Name);
            command.Parameters.AddWithValue("@phoneNumber", ad.PhoneNumber);
            command.Parameters.AddWithValue("@description", ad.Description);
            connection.Open();
            command.ExecuteNonQuery();

        }

        public List<Ad> GetMyAds(int userId)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Posts WHERE UserId = @userId ORDER BY DateCreated DESC";
            command.Parameters.AddWithValue("@userId", userId);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            var Ads = new List<Ad>();
            while (reader.Read())
            {
                Ads.Add(new Ad
                {
                    AdId = (int)reader["id"],
                    UserId = (int)reader["userId"],
                    Description = (string)reader["description"],
                    PhoneNumber = (string)reader["phoneNumber"],
                    DateCreated = (DateTime)reader["dateCreated"]
                });
            }
            return Ads;
        }

        public void DeleteAd(int adId)
        {
            using var connection = new SqlConnection(_connectionString);
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Posts WHERE Id = @adId";
            command.Parameters.AddWithValue("@adId", adId);
            connection.Open();
            command.ExecuteNonQuery();
        }

    }
}
