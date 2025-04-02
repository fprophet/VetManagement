﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetManagement.Data;

namespace VetManagement.Services
{
    public class SessionManager
    {
        private static SessionManager _instance;
        public static SessionManager Instance => _instance ?? (_instance = new SessionManager());

        private string _username;

        private string _role = "admin";

        private int _id = -1;

        public string? Username
        {
            get => _username;
            set 
            {}
        }

        public string? Role
        {
            get => _role;
            set
            {}
        }

        public int Id
        {
            get => _id;
            set
            {}
        }

        public bool MediumLevelPermission(object parameter) => Role == "admin" || Role == "manager" || Role == "farmacist";

        public bool HighLevelPermission(object parameter) => Role == "admin" || Role == "manager";

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
            Role = "";
        }

        public bool isIsLoggedIn() =>  _id >= 0;
    }
}
