using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;

namespace RCCDTool
{
    public class Controller : IController
    {
        IModel _model;
        public bool ModelHasData => _model.HasData;
        public int NumFactors => _model.NumFactors;
        public DataTable FactorSet => _model.FactorSet;
        public List<string> Tables
        {
            get { return _model.Tables; }
            set { _model.Tables = value; }
        }

        public DataSet ResearchDesignOutput
        {
            get { return _model.ResearchDesignOutput; }
            set { _model.ResearchDesignOutput = value; }

        }

        MainWindow _mw;
        
        public Controller(IModel model)
        {
            _model = model;
            
            _mw = new MainWindow("Within and Between Subjects", this);
            
        }

        public void addFactor(ResearchFactor factor)
        {
            if(!_model.FactorSet.Rows.Contains(factor.Name))
                _model.addFactor(factor);
        }

        public void removeFactor(ResearchFactor factor)
        {
            _model.removeFactor(factor);
        }


        public void ClearFactors()
        {
            _model.ClearFactors();
        }

        public void Show(System.Windows.Window view)
        {
            view.Show();
        }

        public void GenerateDesign()
        {
            int numSumjects =20;
            _model.generateDesign(numSumjects);
            DesignDisplay designDisplay = new DesignDisplay(this);
            designDisplay.Show();
        }

        public void StartApp()
        {
            _mw.ShowDialog();
        }

    }
}
