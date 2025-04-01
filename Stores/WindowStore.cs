using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VetManagement.ViewModels;

namespace VetManagement.Stores
{
    public class WindowStore
    {
        public readonly Dictionary<Window, string> Windows = new Dictionary<Window, string>();

        private int _passedId;
        public int PassedId
        {
            get => _passedId;
            set
            {
                 _passedId = value;
            }

        }

    }
}
