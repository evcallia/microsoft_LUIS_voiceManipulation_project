using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data;
using MySql.Data.MySqlClient;
using theWall.Models;
using System;

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
                item.message = item.message.Replace("'", "''");
                string query = $@"INSERT INTO messages (message, user_id, created_at, updated_at) VALUES ('{item.message}', {item.user_id}, NOW(), NOW())";
                dbConnection.Open();
                dbConnection.Execute(query);
            }
        }
        public IEnumerable<Message> FindAll()
        {
            using (IDbConnection dbConnection = Connection) {
                dbConnection.Open();
                var query =
                @"
                SELECT * FROM messages JOIN users ON messages.user_id = users.id;
                SELECT * FROM comments JOIN users ON comments.user_id = users.id";

                using (var multi = dbConnection.QueryMultiple(query)) {
                    var messages = multi.Read<Message, User, Message>((message, user) => { message.user = user; return message;}).ToList();
                    var comments = multi.Read<Comment, User, Comment>((comment, user) => { comment.user = user; return comment;}).ToList();
                    foreach(var message in messages){
                        foreach(var comment in comments){
                            if(message.id == comment.message_id){
                                message.comments.Add(comment);
                            }
                        }
                    }
                    return messages;
                }
            }
        }
         
        public void Delete(int id)
        {
            using (IDbConnection dbConnection = Connection) {
                string query =
                $@"
                DELETE FROM comments WHERE message_id = {id};
                DELETE FROM messages WHERE id={id};";
                dbConnection.Open();
                dbConnection.Execute(query);
            }
        }
    }
}