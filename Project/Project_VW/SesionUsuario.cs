using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_VW
{
    public static class SesionUsuario
    {
        static string user, nombre_evento;
        static int ID, tipo_user, ID_evento;

        // Store nombre de evento
        public static void setIDEvento(int ID_event)
        {
            ID_evento = ID_event;
        }
        public static int getIDEvento()
        {
            return ID_evento;
        }

        // Store nombre de evento
        public static void setEvento(string nombre_event)
        {
            nombre_evento = nombre_event;
        }
        public static string getEvento()
        {
            return nombre_evento;
        }

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

        public static string getUserTipoString()
        {
            switch(tipo_user)
            {
                case 1:
                    return "Administrador";
                case 2:
                    return "Probador";
            }
            return "Uuario";
        }


    }
}
