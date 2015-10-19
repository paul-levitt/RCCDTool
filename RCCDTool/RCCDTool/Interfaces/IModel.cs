using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCCDTool
{
    public interface IModel : IObservable<ResearchFactor>
    {
        void addFactor(ResearchFactor newFactor);
        void removeFactor(ResearchFactor newFactor);
        void notifyObservers(object sender, NotifyCollectionChangedEventArgs e);
    }
}
