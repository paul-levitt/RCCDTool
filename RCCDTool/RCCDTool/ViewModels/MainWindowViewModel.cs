using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Windows.Input;
using RCCDTool.Annotations;
using RCCDTool.ViewModels;
using Xceed.Wpf.DataGrid;

namespace RCCDTool
{
  
    public class MainWindowViewModel : ViewModelBase, IController
    {
        IModel _model;
        private EditFactorsViewModel _efViewModel;

        public MainWindowViewModel(IModel model)
        {
            _model = model;

            DGViewSource = new DataGridCollectionView(FactorSet.DefaultView);
        
        }
        
        #region Properties

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

        public DataGridCollectionView DGViewSource { get; set; }

        
        #endregion



        #region Public Interface

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

        public void GenerateDesign(int numSubjects)
        {
            _model.generateDesign(numSubjects);
            DesignDisplay designDisplay = new DesignDisplay(this);
            designDisplay.Show();
        }

        public void LoadFactorSet(string FilePath)
        {
            _model.LoadFactorSet(FilePath);
        }

        public void SaveFactorSet(string FilePath)
        {
            _model.SaveFactorSet(FilePath);
        }

        #endregion


        #region Commands

        private RelayCommand _editFactorsCommand;

        public ICommand EditFactorsCommand
        {
            get
            {
                if(_editFactorsCommand == null)
                {
                    _editFactorsCommand = new RelayCommand(param =>
                    {
                        try
                        {
                            if(_efViewModel == null)
                                _efViewModel = new EditFactorsViewModel(_model);

                            EditFactorsViewer ef = new EditFactorsViewer(_efViewModel);
                            ef.DataContext = _efViewModel; //this;
                            ef.Show();
                            // FactorsGrid.Items.Refresh();
                        }
                        catch (FormatException exception)
                        {
                            MessageBox.Show("Invalid entry for number of factors! Please enter an integer greater than 1.");
                            MessageBox.Show("Exception was:" + exception);
                        }
                        catch (Exception exception)
                        {
                            MessageBox.Show("Exception was:" + exception);
                        }
                    });
                }

                return _editFactorsCommand;
            }
        }

        #endregion

    }
}