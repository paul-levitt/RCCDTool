using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCCDTool
{
    interface IResearchDesign
    {

        int Subjects { get; set; }

        void Randomize();

        void CounterBalance();


        

    }
}
