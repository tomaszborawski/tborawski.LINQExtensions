using System.Linq;
namespace tborawski.LINQExtension.Tests
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var list = new List<DataToConvert>()
            {
                new DataToConvert() { CustomerID =1, Data =new DateTime(2024,2,1),  Value = 100 },
                new DataToConvert() { CustomerID =1, Data =new DateTime(2024,2,3),  Value = 200 },
                new DataToConvert() { CustomerID =1, Data =new DateTime(2024,3,1),  Value = 50 },
                new DataToConvert() { CustomerID =2, Data =new DateTime(2024,2,1),  Value = 100 },
            };
            var pivot = list.Pivot(o => new PivotData() { CustomerID = o.CustomerID, Year = o.Data.Year },
                o => o.Data.Month, o => Enumerable.Sum(o, o => o.Value), o => $"M{o}");
        }
    }
}
