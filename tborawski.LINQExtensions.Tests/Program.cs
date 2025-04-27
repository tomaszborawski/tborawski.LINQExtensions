using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using tborawski.LINQExtensions.Tests;
namespace tborawski.LINQExtension.Tests
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Prepare random data
            var list = new List<DataToConvert>();
            var rand = new Random();
            for (int i = 0; i < 10000000; i++)
            {
                    list.Add(new DataToConvert() { CustomerID = rand.Next(10), Data = DateTime.Now.AddDays(rand.Next(365)), Value = rand.NextDouble() });
            }

            //Pivot
            var sw = new Stopwatch();
            sw.Start();
            var pivot = list.Pivot(o => new PivotData() { CustomerID = o.CustomerID, Year = o.Data.Year },
                o => o.Data.Month, o => Enumerable.Sum(o, o => o.Value), o => $"M{o}");
            sw.Stop();
            Console.WriteLine($"Pivot Enumerable {sw.Elapsed.Milliseconds}");

            sw.Restart();
            sw.Start();
            var pivotp = list.AsParallel().Pivot(o => new PivotData() { CustomerID = o.CustomerID, Year = o.Data.Year },
                o => o.Data.Month, o => Enumerable.Sum(o, o => o.Value), o => $"M{o}");
            sw.Stop();
            Console.WriteLine($"Pivot Parallel {sw.Elapsed.Milliseconds}");

            //map
            List<Expression<Func<PivotData, double>>> unmap = new List<Expression<Func<PivotData, double>>>();
            unmap.Add(o => o.M1);
            unmap.Add(o => o.M2);
            unmap.Add(o => o.M3);
            unmap.Add(o => o.M4);
            unmap.Add(o => o.M5);
            unmap.Add(o => o.M6);
            unmap.Add(o => o.M7);
            unmap.Add(o => o.M8);
            unmap.Add(o => o.M9);
            unmap.Add(o => o.M10);
            unmap.Add(o => o.M11);
            unmap.Add(o => o.M12);

            //Unpivot
            sw.Restart();
            sw.Start();
            var unpivot = pivot.Unpivot((o, p, v) => new DataToConvert() { CustomerID = o.CustomerID, Data = new DateTime(o.Year, Convert.ToInt32(p.Substring(1)),1), Value = v }, unmap);
            sw.Stop();
            Console.WriteLine($"Unpivot Enumerable {sw.Elapsed.Milliseconds}");

            sw.Restart();
            sw.Start();
            var unpivotp = pivot.AsParallel().Unpivot((o, p, v) => new DataToConvert() { CustomerID = o.CustomerID, Data = new DateTime(o.Year, Convert.ToInt32(p.Substring(1)), 1), Value = v }, unmap);
            sw.Stop();
            Console.WriteLine($"Unpivot Parallel {sw.Elapsed.Milliseconds}");

            var listobj = new List<ToDataTableObject>();
            listobj.Add(new ToDataTableObject() { bytes = new byte[10], Guid = Guid.NewGuid(), NGuid = Guid.NewGuid(), Id = 1, s = "test", xxx = 2, data = DateTime.Now, ndata = DateTime.Now });
            listobj.Add(new ToDataTableObject() { bytes = null, Guid = Guid.NewGuid(), NGuid = null, Id = 2, xxx = null, s = null, data = DateTime.Now, ndata = null });
            var dt = listobj.ToDataTable();
            Console.WriteLine($"Datatable columns {dt.Columns.Count} rows {dt.Rows.Count}");
            var dtenumerable = dt.ToEnumerable<ToDataTableObject>();
            var dtobservable = dtenumerable.ToObservableCollection();
        }
    }
}
