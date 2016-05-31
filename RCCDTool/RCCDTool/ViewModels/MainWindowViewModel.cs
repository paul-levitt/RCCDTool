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

namespace RCCDTool
{
   
    //public class ObservableObject : INotifyPropertyChanged
    //{
    //    public event PropertyChangedEventHandler PropertyChanged;

    //    protected void RaisePropertyChangedEvent(string propertyName)
    //    {
    //        var handler = PropertyChanged;
    //        if (handler != null)
    //            handler(this, new PropertyChangedEventArgs(propertyName));
    //    }
    //}

    public class MainWindowViewModel : ViewModelBase, IController
    {
        IModel _model;
        //MainWindow _mw;

        public MainWindowViewModel(IModel model)
        {
            _model = model;

            //_mw = new MainWindow("Within and Between Subjects", this);
            

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
                            //int nf = int.Parse(numFactors.Text);
                            EditFactorsViewModel efViewModel = new EditFactorsViewModel(_model);
                            EditFactorsViewer ef = new EditFactorsViewer(efViewModel);
                            ef.DataContext = this;
                            //MessageBox.Show("Command executed: " + param);
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