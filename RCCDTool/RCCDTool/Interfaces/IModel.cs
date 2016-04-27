using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data;

namespace RCCDTool
{
    public interface IModel
    {
        void addFactor(ResearchFactor newFactor);
        void removeFactor(ResearchFactor newFactor);
        void ClearFactors();
        void generateDesign(int numSubjects);

        bool HasData { get; }
        int NumFactors { get; }
        DataTable FactorSet { get; }
        List<string> Tables { get; set; }
        DataSet ResearchDesignOutput { get; set; }

    }
}
