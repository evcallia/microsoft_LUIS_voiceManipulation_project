using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data;
using MySql.Data.MySqlClient;
using theWall.Models;

namespace theWall.Factory
{
    public class UserRepository : IFactory<User>
    {
        private string connectionString;

        public UserRepository()
        {
            // connectionString = "server=localhost;userid=root;password=root;port=8889;database=thewalldb;SslMode=None";
            connectionString = "server=172.31.18.148;userid=remote;password=password;port=3306;database=thewalldb;SslMode=None";                        
        }

        internal IDbConnection Connection
        {
            get {
                return new MySqlConnection(connectionString);
            }
        }

        public void Add(User item)
        {
            using (IDbConnection dbConnection = Connection) {
                string query = $"INSERT INTO users (first_name, last_name, email, password, created_at, updated_at) VALUES ('{item.first_name}', '{item.last_name}', '{item.email}', '{item.encPass}', NOW(), NOW())";
                dbConnection.Open();
                dbConnection.Execute(query);
            }
        }
        public IEnumerable<User> FindAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<User>("SELECT * FROM users");
            }
        }
        public User FindByID(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<User>($"SELECT * FROM users WHERE id = {id}").FirstOrDefault();
            }
        }

        public User FindByEmail(string email){
             using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<User>($"SELECT * FROM users WHERE email = '{email}'").FirstOrDefault();
            }
        }
    }
}