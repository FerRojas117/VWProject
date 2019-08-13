// Author: Tigran Gasparian
// This sample is part Part One of the 'Getting Started with SQLite in C#' tutorial at http://www.blog.tigrangasparian.com/

using System;
using System.Data.SQLite;

namespace SQLiteSamples
{
    class Program
    {
        // Holds our connection with the database
        SQLiteConnection m_dbConnection;

        static void Main(string[] args)
        {
            Program p = new Program();
        }

        public Program()
        {
 
            connectToDatabase();
            fillTable();
            printHighscores();
        }

      
        // Creates a connection with our database file.
        void connectToDatabase()
        {
            m_dbConnection = new SQLiteConnection("Data Source=PruebasNar_DB.db;Version=3;");
            m_dbConnection.Open();
        }


        // Inserts some values in the highscores table.
        // As you can see, there is quite some duplicate code here, we'll solve this in part two.
        void fillTable()
        {
            string sql = "INSERT INTO usuarios (password, user, tipo_user) values ('fernando', 'fernando', 1)";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
            
        }

        // Writes the highscores to the console sorted on score in descending order.
        void printHighscores()
        {
            string sql = "select * from usuarios";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
                Console.WriteLine("user: " + reader["user"] +
                    "\tpassword: " + reader["password"] +
                    "\tipo user: " +
                    reader["tipo_user"]);
            Console.ReadLine();
        }
    }
}
