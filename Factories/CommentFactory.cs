using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data;
using MySql.Data.MySqlClient;
using theWall.Models;

namespace theWall.Factory
{
    public class CommentRepository : IFactory<User>
    {
        private string connectionString;

        public CommentRepository()
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

        public void Add(Comment item)
        {
            using (IDbConnection dbConnection = Connection) {
                string query = $"INSERT INTO comments (comment, user_id, message_id, created_at, updated_at) VALUES ('{item.comment}', {item.user_id}, {item.message_id}, NOW(), NOW())";
                dbConnection.Open();
                dbConnection.Execute(query);
            }
        }
        public IEnumerable<Comment> FindAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<Comment>("SELECT * FROM comments");
            }
        } 

        public void Delete(int id)
        {
            using (IDbConnection dbConnection = Connection) {
                string query = $"DELETE FROM comments WHERE id={id}";
                dbConnection.Open();
                dbConnection.Execute(query);
            }
        }
    }
}