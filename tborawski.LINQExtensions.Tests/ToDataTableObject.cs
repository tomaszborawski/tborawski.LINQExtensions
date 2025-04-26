using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tborawski.LINQExtensions.Tests
{
    internal class ToDataTableObject
    {
        public int Id { get; set; }
        public int? xxx { get; set; }
        public string s { get; set; }
        public Guid Guid { get; set; }
        public Guid? NGuid { get; set; }
        public byte[] bytes { get; set; }
        public DateTime data {get;set;}
        public DateTime? ndata { get;set;}  
    }
}
