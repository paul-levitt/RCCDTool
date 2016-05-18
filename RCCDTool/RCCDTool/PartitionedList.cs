using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCCDTool
{
    class PartitionedList<T>
    {
        public List<int> Partitions { get; set; }
        public List<string> ColumnSetNames { get; set; }
        public List<T> OutputData { get; }

        //public PartitionedList<T>() 
    }
}
