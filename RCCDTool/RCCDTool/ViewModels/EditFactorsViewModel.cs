using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RCCDTool.ViewModels
{
    public class EditFactorsViewModel : ViewModelBase
    {

        private IModel _model;

        public List<ResearchFactor> ResearchFactors { get; set; }

        public ObservableCollection<string> DesignTypes
        {
            get{ return _model.DesignTypes; }
            set { _model.DesignTypes = value; }
        }

        public EditFactorsViewModel(IModel model)
        {
            _model = model;
            
            ResearchFactors = new List<ResearchFactor>();

        }

        private RelayCommand _saveFactors;

        public ICommand SaveFactors
        {
            get
            {
                if (_saveFactors == null)
                {
                    _saveFactors = new RelayCommand(param =>
                    {
                        foreach (ResearchFactor researchFactor in ResearchFactors)
                        {
                            DataRow row;
                            bool addNewRow = false;

                            if (_model.FactorSet.AsEnumerable().All(r => r.Field<string>("Name") != researchFactor.Name))
                                //make sure that the row isn't already in the table
                            {
                                row = _model.FactorSet.NewRow();
                                addNewRow = true;
                            }
                            else //if it is in the table, then update that row
                                row = _model.FactorSet.Rows.Find(researchFactor.Name);
                            

                            row["Name"] = researchFactor.Name;
                            row["Labels"] = researchFactor.Labels;
                            row["isRandomized"] = researchFactor.IsRandomized;
                            row["isWithinSubjects"] = researchFactor.IsWithinSubjects;
                            row["Levels"] = researchFactor.Labels.Count;

                            if(addNewRow) _model.FactorSet.Rows.Add(row);
                        }
                        
                        //TODO: Actually test out what happens when I try to add duplicates or update rows.
                           
                    });
                }

                return _saveFactors;
            }
        }


    }
}
