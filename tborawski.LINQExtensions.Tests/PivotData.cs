using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tborawski.LINQExtension.Tests
{
    internal class PivotData : IEquatable<PivotData>
    {
        public int CustomerID { get; set; }
        public int Year { get; set; }
        public double M1 { get; set; }
        public double M2 { get; set; }
        public double M3 { get; set; }
        public double M4 { get; set; }
        public double M5 { get; set; }
        public double M6 { get; set; }
        public double M7 { get; set; }
        public double M8 { get; set; }
        public double M9 { get; set; }
        public double M10 { get; set; }
        public double M11 { get; set; }
        public double M12 { get; set; }

        public bool Equals(PivotData other)
        {
            return CustomerID == other.CustomerID && Year == other.Year;
        }

        public override int GetHashCode()
        {
            return CustomerID.GetHashCode() ^ Year.GetHashCode();
        }
    }
}
