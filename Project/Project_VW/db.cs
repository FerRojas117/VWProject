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
        SQLiteDataReader reader;
        public DB()
        {
            conexion = new SQLiteConnection("Data Source=PruebasNar_DB.db;Version=3;");   
        }
        public void openConn()
        {
            try
            {
                conexion.Open();
            }
            catch (Exception ex)
            {
                sendMBandCloseConn("No pudo abrirse de forma correcta la base de datos.\n" + ex.Message);
            }
        }
        public SQLiteConnection getConn()
        {
            return conexion;
        }
        public SQLiteCommand getComm()
        {
            return command;
        }
        public SQLiteCommand setComm(string the_query )
        {
            return command = new SQLiteCommand(the_query, conexion);
        }
        public void setReader()
        {
             reader = command.ExecuteReader();
        }

        public SQLiteDataReader getReader()
        {
            return reader;
        }

        public void closeConn()
        {
            try
            {
                conexion.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No pudo cerrarse de forma correccta la base de datos.\n" +
                    ex.Message
                );
            }
        }

        public void sendMBandCloseConn(string message)
        {
            MessageBox.Show(message);
            closeConn();
        }
    }
}
