using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data;

namespace RCCDTool
{
    public interface IModel
    {
        void addFactor(ResearchFactor newFactor);
        void removeFactor(ResearchFactor newFactor);
       
        bool HasData { get; }
        int NumFactors { get; }
        DataTable FactorSet { get; }
        
        void ClearFactors();
        
    }
}
