using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetManagement.Services
{
    public class CopyPropertiesService
    {
        public static void CopyProperties<TSource, TTarget>(TSource source, TTarget target)
        {
            List<string> ignoreList = ["Id"];

            var sourceProperties = typeof(TSource).GetProperties();
            var targetProperties = typeof(TTarget).GetProperties()
                .Where(p => p.CanWrite)
                .ToDictionary(p => p.Name);

            foreach (var sourceProp in sourceProperties)
            {
                if (sourceProp.CanRead && targetProperties.TryGetValue(sourceProp.Name, out var targetProp))
                {
                    if (targetProp.PropertyType == sourceProp.PropertyType && !ignoreList.Contains(sourceProp.Name))
                    {
                        var value = sourceProp.GetValue(source);
                        targetProp.SetValue(target, value);
                    }
                }
            }
        }
    }
}
