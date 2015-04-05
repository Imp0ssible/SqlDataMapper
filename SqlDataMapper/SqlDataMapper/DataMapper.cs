using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq.Expressions;

namespace SqlDataMapper
{
    /// <summary>
    /// Little ORM class
    /// </summary>
    public static class DataMapper
    {
        /// <summary>
        /// Cache compilation, improve performance
        /// </summary>
        private static readonly Dictionary<Type, Delegate> Cache = new Dictionary<Type, Delegate>();

        /// <summary>
        /// Map SqlDataReader result to object defined type
        /// </summary>
        private static Func<SqlDataReader, T> Map<T>(this SqlDataReader reader)
        {
            var entityType = typeof(T);

            if (Cache.ContainsKey(entityType))
                return (Func<SqlDataReader, T>)Cache[entityType];

            var readerType = typeof(SqlDataReader);

            var columns = new List<string>();

            for (int i = 0, y = reader.FieldCount; i < y; i++)
                columns.Add(reader.GetName(i));

            var parameter = Expression.Parameter(readerType, "reader");

            var method = readerType.GetMethod("GetValue");

            var bindings = new List<MemberBinding>();

            foreach (var property in entityType.GetProperties())
            {
                var attributes = property.GetCustomAttributes(typeof(DbColumnAttribute), false);

                var attr = attributes.Length > 0 ? (DbColumnAttribute)attributes[0] : null;

                if (attr == null || attr.Hidden || !columns.Contains(attr.ColumnName))
                    continue;

                var value = Expression.Call(parameter, method, new Expression[] { Expression.Constant(reader.GetOrdinal(attr.ColumnName)) });

                var condition = Expression.Condition(Expression.NotEqual(Expression.Field(null, typeof(DBNull).GetField("Value")), value),
                                                     Expression.Convert(value, property.PropertyType),
                                                     Expression.Convert(Expression.Constant(null), property.PropertyType));

                bindings.Add(Expression.Bind(entityType.GetMember(property.Name)[0], condition));
            }

            var lambda = Expression.Lambda<Func<SqlDataReader, T>>(Expression.MemberInit(Expression.New(entityType), bindings), new[] { parameter }).Compile();

            Cache.Add(entityType, lambda);

            return lambda;
        }

        /// <summary>
        /// Will return object of the given generic type from SqlDataReader result 
        /// or default value of an object if reader has no data
        /// If InvalidCastException was thrown, check property types and db columns divergency
        /// </summary>     
        public static T MapToObject<T>(this SqlDataReader reader) // 
        {
            return reader.Read() ? reader.Map<T>().Invoke(reader) : default(T);
        }

        /// <summary>
        /// Will return elements of the given generic type from SqlDataReader result
        /// or null if reader has no rows and data
        /// If InvalidCastException was thrown, check property types and db columns divergency
        /// </summary> 
        public static IEnumerable<T> MapToList<T>(this SqlDataReader reader) // if InvalidCastException, check columns vs property types
        {
            var list = new List<T>();

            while (reader.Read())
                list.Add(reader.Map<T>().Invoke(reader));

            return list.Count > 0 ? list : null;
        }
    }
}
