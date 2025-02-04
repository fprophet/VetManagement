using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetManagement.Data;

namespace VetManagement.Services
{
    class SessionManager
    {
        private static SessionManager _instance;
        public static SessionManager Instance => _instance ?? (_instance = new SessionManager());

        private string _username;

        private string _role;

        private int _id = -1;

        public string Username
        {
            get => _username;

            set 
            {
                _username = value;
            }
        }

        public string Role
        {
            get => _role;

            set
            {
                _role = value;
            }
        }

        public int Id
        {
            get => _id;

            set
            {
                _id = value;
            }
        }



        public void LogUser(int id, string username, string role) 
        { 
            Id = id;
            Username = username;
            Role = role;
        }

        public void LogoutUser() 
        {
            Id = -1;
            Username = null;
            Role = null;
        }

        public bool isIsLoggedIn() =>  _id >= 0;
    }
}
