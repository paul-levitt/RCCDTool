using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;

namespace RCCDTool
{
    public interface IController
    {
        void addFactor(ResearchFactor factor);
        void removeFactor(ResearchFactor factor);
        void ClearFactors();
        void Show(System.Windows.Window view);
        void GenerateDesign();

        bool ModelHasData { get; }
        int NumFactors { get; }        
        DataTable FactorSet { get; }
        List<string> Tables { get; set; }
        DataSet ResearchDesignOutput { get; set; }
    }
}
