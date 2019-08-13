using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows;

namespace Project_VW
{
    class DB
    {
        // Holds our connection with the database
        SQLiteConnection conexion;
        SQLiteCommand command;
        public void connectDB()
        {
            conexion = new SQLiteConnection("Data Source=PruebasNar_DB.db;Version=3;");
            try
            {
                conexion.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                throw;
            }
        }

        public void ExecuteQuery(string the_query)
        {
            connectDB();
            command = conexion.CreateCommand();
            
        }
    }
}
