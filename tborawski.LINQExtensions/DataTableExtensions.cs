using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace System.Linq
{
    public static class DataTableExtensions
    {
        public static DataTable ToDataTable<T>(this IEnumerable<T> list)
        {
            var t = typeof(T);
            var nullt = typeof(Nullable<>);
            var props = t.GetProperties().Where(o => !o.PropertyType.IsGenericType || o.PropertyType.GetGenericTypeDefinition() == nullt).ToList();
            var data = new DataTable();
            foreach (var prop in props)
            {
                data.Columns.Add(new DataColumn()
                {
                    ColumnName = prop.Name,
                    DataType = prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == nullt ? prop.PropertyType.GetGenericArguments()[0] : prop.PropertyType,
                    AllowDBNull = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == nullt) || 
                    prop.PropertyType == typeof(string) || prop.PropertyType.IsArray
                });
            }

            foreach (var item in list)
            {
                var objarray = new object[props.Count];
                int i = 0;
                foreach (var prop in props)
                {
                    var obj = prop.GetValue(item, null);
                    if (obj != null)
                        objarray[i] = obj;
                    else
                        objarray[i] = DBNull.Value;
                    i++;
                }
                data.Rows.Add(objarray);
            }
            return data;
        }
    }
}
