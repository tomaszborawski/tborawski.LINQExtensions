using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using System.Text;

namespace System.Linq
{
    public static class DataTableExtensios
    {
        public static IEnumerable<T> ToEnumerable<T>(this DataTable table) where T : new () 
        {
            var t = typeof(T);
            var props = t.GetProperties().ToDictionary(o => o.Name, o => o);
            foreach (DataRow dr in table.Rows)
            {
                var obj = new T();
                foreach (DataColumn c in table.Columns)
                {
                    var dtv = dr[c];
                    if (dtv == DBNull.Value) dtv = null;
                    if (props.ContainsKey(c.ColumnName)) props[c.ColumnName].SetValue(obj, dtv);
                }
                yield return obj;
            }
        }
    }
}
