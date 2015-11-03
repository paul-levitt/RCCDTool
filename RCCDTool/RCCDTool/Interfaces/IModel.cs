using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace RCCDTool
{
    public interface IModel : IObservable<ResearchFactor>
    {
        void addFactor(ResearchFactor newFactor);
        void removeFactor(ResearchFactor newFactor);
        void notifyObservers(object sender, NotifyCollectionChangedEventArgs e);
        bool HasData { get; }
        int NumFactors { get; }
        ObservableCollection<ResearchFactor> ResearchFactors { get; }
        void ClearFactors();
    }
}
