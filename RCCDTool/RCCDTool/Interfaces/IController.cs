using System;
using System.Collections.ObjectModel;
using System.Data;

namespace RCCDTool
{
    public interface IController
    {
        void addFactor(ResearchFactor factor);
        void removeFactor(ResearchFactor factor);
        //void AddSubscriber(IObserver<ResearchFactor> subscriber);
        bool ModelHasData { get; }
        int NumFactors { get; }
        //ObservableCollection<ResearchFactor> ResearchFactors { get; }
        DataTable FactorSet { get; }
        void ClearFactors();
        //void RemoveSubscriber(IObserver<ResearchFactor> subscriber);
    }
}
