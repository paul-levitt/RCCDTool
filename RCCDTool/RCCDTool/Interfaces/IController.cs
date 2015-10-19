using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCCDTool
{
    public interface IController
    {
        void addFactor(ResearchFactor factor);
        void removeFactor(ResearchFactor factor);
    }
}
