using Microsoft.Xrm.Sdk;
using System;
using System.ComponentModel;

namespace XrmEarth.Core
{
    public class ConvertHelper
    {
        public static T CustomConvert<T>(string input)
        {
            try
            {
                var converter = TypeDescriptor.GetConverter(typeof(T));
                if (converter != null)
                {
                    return (T)converter.ConvertFromString(input);
                }
                return default(T);
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException(ex.ToString());
            }
        }

        public static decimal ConvertRollupFieldToDecimal(object value)
        {
            try
            {
                if (value != null)
                {
                    var money = value as Money;
                    if (money != null)
                    {
                        return Convert.ToDecimal(money.Value);
                    }
                    else
                    {
                        return Convert.ToDecimal(value);
                    }
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException(ex.ToString());
            }
        }
    }
}
