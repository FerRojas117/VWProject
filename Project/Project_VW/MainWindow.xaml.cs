using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SQLite;

namespace Project_VW
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : NavigationWindow
    {
        // Holds our connection with the database
        SQLiteConnection m_dbConnection;
        public MainWindow()
        {  
            InitializeComponent();       
            connectToDatabase();
        }


        // Creates a connection with our database file.
        void connectToDatabase()
        {
            m_dbConnection = new SQLiteConnection("Data Source=MyDatabase.sqlite;Version=3;");
            m_dbConnection.Open();
        }

       
        // Writes the highscores to the console sorted on score in descending order.
        void printHighscores()
        {
            string sql = "select * from highscores order by score desc";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
           // while (reader.Read())
              //  tbSettingText.AppendText("Name: " + reader["name"] + "\tScore: " + reader["score"]);
             
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            printHighscores();
        }
    }
}
