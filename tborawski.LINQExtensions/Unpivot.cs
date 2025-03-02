using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace System.Linq
{
    public static class UnpivotClass
    {
        public static IEnumerable<TResult> Unpivot<TSource, TValue, TResult>(this IEnumerable<TSource> source, 
            Func<TSource, string, TValue, TResult> selector, IEnumerable<Expression<Func<TSource, TValue>>> unmap, TValue valueDefault = default(TValue))
        {
            var listPropertynames = new List<string>();
            var listDelegates = new List<Func<TSource, TValue>>();
            foreach (var item in unmap)
            {
                if (item.Body.NodeType != ExpressionType.MemberAccess) throw new InvalidOperationException($"In unmap only single Member Access is supported");
                listPropertynames.Add((item.Body as MemberExpression).Member.Name);
                listDelegates.Add(item.Compile());
            }

            var list = new List<TResult>();
            foreach (var obj in source)
            {
                for (var i = 0; i < listPropertynames.Count; i++)
                {
                    var value = listDelegates[i](obj);
                    if (!value.Equals(valueDefault)) list.Add(selector(obj, listPropertynames[i], value));
                }
            }
            return list;
        }

        public static IEnumerable<TResult> Unpivot<TSource, TValue, TResult>(this ParallelQuery<TSource> source,
            Func<TSource, string, TValue, TResult> selector, IEnumerable<Expression<Func<TSource, TValue>>> unmap, TValue valueDefault = default(TValue))
        {
            var listPropertynames = new List<string>();
            var listDelegates = new List<Func<TSource, TValue>>();
            foreach (var item in unmap)
            {
                if (item.Body.NodeType != ExpressionType.MemberAccess) throw new InvalidOperationException($"In unmap only single Member Access is supported");
                listPropertynames.Add((item.Body as MemberExpression).Member.Name);
                listDelegates.Add(item.Compile());
            }

            var list = new ConcurrentBag<TResult>();
            Parallel.ForEach(source, obj =>
            {
                for (var i = 0; i < listPropertynames.Count; i++)
                {
                    var value = listDelegates[i](obj);
                    if (!value.Equals(valueDefault)) list.Add(selector(obj, listPropertynames[i], value));
                }
            });
            return list;
        }
    }
}
