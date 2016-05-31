using System;
using System.Collections.Generic;
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
                    //_saveFactors = new RelayCommand(param =>
                    //{
                    //    if (_model.FactorSet.Rows.Count > 0)
                    //        _model.ClearFactors(); //resets the research factors, needed for updates

                    //    //add data from the grid to the model
                    //    for (int i = 1; i <= _numFactors; i++)
                    //    {
                    //        var itemsInFirstRow = factorGrid.Children.Cast<UIElement>().Where(a => Grid.GetRow(a) == i);

                    //        ResearchFactor newFactor = new ResearchFactor();
                    //        foreach (UIElement uie in itemsInFirstRow)
                    //        {
                    //            if (uie is TextBox)
                    //            {
                    //                if ((uie as TextBox).Name == "factorName")
                    //                {
                    //                    newFactor.Name = (uie as TextBox).Text;

                    //                    if (newFactor.Name == "" || newFactor.Name == null)
                    //                    {
                    //                        throw new ArgumentNullException(
                    //                            "Please enter a name for all factors listed!");
                    //                    }
                    //                }

                    //            }
                    //            else if (uie is ComboBox)
                    //            {
                    //                if ((uie as ComboBox).Text == "Within Subjects Factor")
                    //                    newFactor.isWithinSubjects = true;
                    //            }
                    //            else if (uie is CheckBox)
                    //            {
                    //                newFactor.IsRandomized = (bool) (uie as CheckBox).IsChecked;
                    //            }
                    //            else if (uie is StackPanel)
                    //            {
                    //                newFactor.Labels = listOfAllLabels[i - 1];
                    //                //newFactor.Levels = int.Parse((
                    //                //    (uie as FrameworkElement).FindVisualChildren<TextBox>().First(l => l.GetType() == typeof(TextBox))).Text);
                    //                newFactor.Levels = listOfAllLabels[i - 1].Count;
                    //            }
                    //        }

                    //        _controller.addFactor(newFactor);

                    //    }

                    //    Close();



                    //});
                }

                return _saveFactors;
            }
        }


    }
}
