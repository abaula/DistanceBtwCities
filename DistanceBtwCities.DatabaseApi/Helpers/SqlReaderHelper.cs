using System;
using System.ComponentModel;
using System.Data.SqlClient;

namespace DistanceBtwCities.DatabaseApi.Helpers
{
    internal static class SqlReaderHelper
    {
        private static bool IsNullableType(Type theValueType)
        {
            return theValueType.IsGenericType
                   && theValueType.GetGenericTypeDefinition().Equals(typeof (Nullable<>));
        }

        public static T GetValue<T>(this SqlDataReader record, int index)
        {
            var theValue = record[index];

            var theValueType = typeof (T);

            if (DBNull.Value != theValue)
            {
                if (!IsNullableType(theValueType))
                {
                    return (T) Convert.ChangeType(theValue, theValueType);
                }
                var theNullableConverter = new NullableConverter(theValueType);

                return (T) Convert.ChangeType(theValue, theNullableConverter.UnderlyingType);
            }

            return default(T);
        }
    }
}