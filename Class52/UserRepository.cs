using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class52.data
{
    public class UserRepository
    {
        private string _connectionString;
        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public decimal NewUser(User user, string password)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Users VALUES (@name, @email, @phoneNumber, @passwordHash) SELECT SCOPE_IDENTITY()";

            command.Parameters.AddWithValue("@name", user.Name);
            command.Parameters.AddWithValue("@email", user.Email);
            command.Parameters.AddWithValue("@phoneNumber", user.PhoneNumber);
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            command.Parameters.AddWithValue("@passwordHash", passwordHash);
            connection.Open();
            return (int)(decimal)command.ExecuteScalar();
        }

        public User Login(string email, string password)
        {
            var user = GetUserByEmail(email);
            if (user == null)
            {
                return null;
            }
            var isValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            if (isValid)
            {
                return user;
            }
            return null;
        }

        public User GetUserByEmail(string email)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Users WHERE Email = @email";
            command.Parameters.AddWithValue("@email", email);
            connection.Open();
            var reader = command.ExecuteReader();
            if (!reader.Read())
            {
                return null;
            }
            return new User()
            {
                UserId = (int)reader["UserId"],
                Name = (string)reader["Name"],
                Email = (string)reader["Email"],
                PhoneNumber = (string)reader["PhoneNumber"],
                PasswordHash = (string)reader["PasswordHash"],
            };
        }
    }
}
