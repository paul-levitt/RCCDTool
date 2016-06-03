using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
         
                }

                return _saveFactors;
            }
        }


    }
}
