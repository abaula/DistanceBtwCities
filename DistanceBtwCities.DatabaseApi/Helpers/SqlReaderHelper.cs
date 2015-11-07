using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace DistanceBtwCities.DatabaseApi.Helpers
{
    static class SqlReaderHelper
    {
        private static bool IsNullableType(Type theValueType)
        {
            return (
                theValueType.IsGenericType 
                && theValueType.GetGenericTypeDefinition().Equals(typeof(Nullable<>))
                );
        }

        public static T GetValue<T>(this SqlDataReader record, int index)
        {
            object theValue = record[index];

            Type theValueType = typeof(T);

            if (DBNull.Value != theValue)
            {
                if (!IsNullableType(theValueType))
                {
                    return (T)Convert.ChangeType(theValue, theValueType);
                }
                else
                {
                    NullableConverter theNullableConverter = new NullableConverter(theValueType);

                    return (T)Convert.ChangeType(theValue, theNullableConverter.UnderlyingType);
                }
            }

            return default(T);
        }
    }
}
