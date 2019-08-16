using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_VW
{
    public static class SesionUsuario
    {
        static string user;
        static int ID, tipo_user;

        // Store ussers
        public static void setUser(string user_name)
        {
            user = user_name;
        }
        public static string getUser()
        {
            return user; 
        }

        //store ID of user
        public static void setID(int user_ID)
        {
            ID = user_ID;
        }
        public static int getUserID()
        {
            return ID;
        }

        // store tipoUser
        public static void setUserTipo(int tipoUser)
        {
            tipo_user = tipoUser;
        }
        public static int getUserTipo()
        {
            return tipo_user;
        }

   
    }
}
