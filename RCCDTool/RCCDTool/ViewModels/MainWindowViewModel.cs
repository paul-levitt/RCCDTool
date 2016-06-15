using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using RCCDTool.Annotations;
using RCCDTool.ViewModels;
using Xceed.Wpf.DataGrid;
using System.Windows;
using Microsoft.Win32;

namespace RCCDTool
{
  
    public class MainWindowViewModel : ViewModelBase, IController
    {
        Model _model;
        private EditFactorsViewModel _efViewModel;

        public MainWindowViewModel(Model model)
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
            //_model.SaveFactorSet(FilePath);
            //TODO remove IController and associated methods. No longer MVC.
        }

        #endregion


        #region Commands

        private RelayCommand _editFactorsCommand;
        private RelayCommand _openFileCommand;
        private RelayCommand _saveFileCommand;

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

        public ICommand OpenFileCommand
        {
            get
            {
                if (_openFileCommand == null)
                {
                    _openFileCommand = new RelayCommand(param =>
                    {
                        OpenFileDialog openFileDialog = new OpenFileDialog
                        {
                            Filter = "FactorSet files (.fac)|*.fac",
                            DefaultExt = ".fac"
                        };

                        if (openFileDialog.ShowDialog() == true)
                        {
                            _model.LoadFactorSet(openFileDialog.FileName);

                        }
                    });
                }

                return _openFileCommand;
            }
            
        }

        public ICommand SaveFileCommand
        {
            get
            {
                if (_saveFileCommand == null)
                {
                    _saveFileCommand = new RelayCommand(param =>
                    {
                        if (FactorSet.Rows.Count == 0)
                        {
                            MessageBox.Show("Please input factors into the model before saving.", "Please enter data");
                            return;
                        }

                        SaveFileDialog saveFileDialog = new SaveFileDialog
                        {
                            FileName = "FactorSet",
                            DefaultExt = ".fac",
                            Filter = "FactorSet files (.fac)|*.fac"
                        };

                        if (saveFileDialog.ShowDialog() == true)
                        {
                            _model.SaveFactorSet(saveFileDialog.FileName);
                        }
                    });
                }

                return _saveFileCommand;
            }

        }

        #endregion

    }
}