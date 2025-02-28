using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            var list = new Dictionary<TResult, TResult>();
            var agglist = source.GroupBy(o => new { selector = selector(o), pivotSelector = pivotSelector(o) });
            foreach (var agg in agglist)
            {
                TResult result;
                var newobj = agg.Key.selector;
                TResult oldobj;
                if (list.TryGetValue(newobj, out oldobj))
                    result = oldobj;
                else
                {
                    result = newobj;
                    list.Add(result, result);
                }

                var value = pivotAggfunction(agg);
                var colname = targetColumn(agg.Key.pivotSelector);
                var t = typeof(TResult);
                var pi = t.GetProperty(colname);
                pi.SetValue(result, value, null);
            }
            return list.Values.ToList();
        }

        public static IEnumerable<TResult> Pivot<TSource, TPivotColumns, TPivotResult, TResult>(this ParallelQuery<TSource> source,
            Func<TSource, TResult> selector, Func<TSource, TPivotColumns> pivotSelector,
            Func<IEnumerable<TSource>, TPivotResult> pivotAggfunction, Func<TPivotColumns, string> targetColumn)
            where TResult : IEquatable<TResult>
            where TPivotColumns : IEquatable<TPivotColumns>
        {
            var list = new ConcurrentDictionary<TResult, TResult>();
            var agglist = source.GroupBy(o => new { selector = selector(o), pivotSelector = pivotSelector(o) });
            Parallel.ForEach(agglist, agg =>
            {
                TResult result;
                var newobj = agg.Key.selector;
                TResult oldobj;
                if (list.TryGetValue(newobj, out oldobj))
                    result = oldobj;
                else
                {
                    result = newobj;
                    if (!list.TryAdd(result, result)) list.TryGetValue(newobj, out result);
                }


                var value = pivotAggfunction(agg);
                var colname = targetColumn(agg.Key.pivotSelector);
                var t = typeof(TResult);
                var pi = t.GetProperty(colname);
                pi.SetValue(result, value, null);
            });
            return list.Values.ToList();
        }
    }
}
