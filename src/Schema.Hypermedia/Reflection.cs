using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Schema.Hypermedia
{
    internal static class Reflection
    {
        internal static string GetPropertyValue(string propName, object entity)
        {
            // Overriding default lookup flags. Make sure all appropriate lookup flags 
            // are specified.
            var property = entity.GetType()
                .GetProperty(propName,
                    BindingFlags.IgnoreCase |
                    BindingFlags.Public |
                    BindingFlags.Instance);

            if (property == null)
            {
                throw new ArgumentException(string.Format("Unable to find property named: {0}. To ignore this,  " +
                                                          "please specify an Inspection Behavior of Loose.", propName));
            }

            var value = property.GetValue(entity, null);
            if (value == null)
            {
                throw new ArgumentException(string.Format("The value of property {0} is invalid. To ignore this,  " +
                                                          "please specify an Inspection Behavior of Loose.", propName));
            }

            return Convert.ToString(value);
        }
    }
}
