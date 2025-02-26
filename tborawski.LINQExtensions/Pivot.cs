using System.Collections.Generic;

namespace System.Linq
{
    public static class PivotClass
    {
        public static IEnumerable<TResult> Pivot<TSource, TPivotColumns, TPivotResult, TResult>(this IEnumerable<TSource> source,
            Func<TSource, TResult> selector, Func<TSource, TPivotColumns> pivotSelector,
            Func<IEnumerable<TSource>, TPivotResult> pivotAggfunction, Func<TPivotColumns, string> targetColumn) 
            where TResult : IEquatable<TResult>
            where TPivotColumns : IEquatable<TPivotColumns>
        {
            var list = new List<TResult>();
            var agglist = source.GroupBy(o => new { selector = selector(o), pivotSelector = pivotSelector(o) });
            foreach (var agg in agglist)
            {
                TResult result;
                var newobj = agg.Key.selector;
                var oldobj = list.FirstOrDefault(o => newobj.Equals(o));
                if (oldobj != null)
                    result = oldobj;
                else
                {
                    result = newobj;
                    list.Add(result);
                }

                var value = pivotAggfunction(agg);
                var colname = targetColumn(agg.Key.pivotSelector);
                var t = typeof(TResult);
                var pi = t.GetProperty(colname);
                pi.SetValue(result, value, null);
            }
            return list;
        }
    }
}
