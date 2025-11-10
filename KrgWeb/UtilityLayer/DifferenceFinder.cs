using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UtilityLayer
{
    public static class DifferenceFinder
    {
        public static List<string> GetDifferentProperties<T>(T obj1, T obj2, List<string> ignoreProps)
        {
            var differences = new List<string>();
            if (obj1 == null || obj2 == null) return differences;

            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in properties)
            {
                if (ignoreProps.Contains(prop.Name)) continue; // Skip ignored properties

                var value1 = prop.GetValue(obj1);
                var value2 = prop.GetValue(obj2);

                if (!Equals(value1, value2))
                {
                    differences.Add(prop.Name);
                }
            }

            return differences;
        }
    }
}
