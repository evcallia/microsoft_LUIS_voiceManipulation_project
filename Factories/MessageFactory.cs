using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data;
using MySql.Data.MySqlClient;
using theWall.Models;

namespace theWall.Factory
{
    public class MessageRepository : IFactory<User>
    {
        private string connectionString;

        public MessageRepository()
        {
            connectionString = "server=localhost;userid=root;password=root;port=8889;database=thewalldb;SslMode=None";
            // connectionString = "server=172.31.18.148;userid=remote;password=password;port=3306;database=thewalldb;SslMode=None";            
        }

        internal IDbConnection Connection
        {
            get {
                return new MySqlConnection(connectionString);
            }
        }

        public void Add(Message item)
        {
            using (IDbConnection dbConnection = Connection) {
                string query = $"INSERT INTO messages (message, user_id, created_at, updated_at) VALUES ('{item.message}', {item.user_id}, NOW(), NOW())";
                dbConnection.Open();
                dbConnection.Execute(query);
            }
        }
        public IEnumerable<Message> FindAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<Message>("SELECT * FROM messages ORDER BY updated_at DESC");
            }
        }
         
        public void Delete(int id)
        {
            using (IDbConnection dbConnection = Connection) {
                string query = $"DELETE FROM messages WHERE id={id}";
                dbConnection.Open();
                dbConnection.Execute(query);
            }
        }
    }
}