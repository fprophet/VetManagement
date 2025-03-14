using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VetManagement.Services
{
    public class ObjectHelper
    {
        public static List<string> GetPropertiesAsStrings(Type type)
        {
            List<PropertyInfo> orderedProperties = new List<PropertyInfo>();

            // Traverse base classes first
            Type? currentType = type.BaseType;
            while (currentType != null && currentType != typeof(object))
            {
                orderedProperties.AddRange(currentType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly));
                currentType = currentType.BaseType;
            }

            // Add derived class properties last
            orderedProperties.AddRange(type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly));

            return orderedProperties.Select(p => p.Name).ToList();
        }
    }
}
