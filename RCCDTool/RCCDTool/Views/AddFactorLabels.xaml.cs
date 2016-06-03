using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using RCCDTool.ViewModels;

namespace RCCDTool
{
    /// <summary>
    /// Interaction logic for AddFactorLabels.xaml
    /// </summary>
    public partial class AddFactorLabels : Window
    {
        private ObservableCollection<string> _labels { get; set; }
        public event Action<ObservableCollection<string>> LabelReturn;
        

        public AddFactorLabels(ObservableCollection<string> labels)
        {
            InitializeComponent();
            _labels = labels ?? new ObservableCollection<string>();

            foreach (string label in _labels)
            {
                labels_listView.Items.Add(label);
            }
        }

        public AddFactorLabels(EditFactorsViewModel _efViewModel, int index)
        {
            InitializeComponent();
            try
            {
                if(_efViewModel.ResearchFactors[index].Labels == null)
                    _efViewModel.ResearchFactors[index].Labels = new ObservableCollection<string>();

                _labels = _efViewModel.ResearchFactors[index].Labels;
                Binding binding = new Binding("Labels") {Source = _efViewModel.ResearchFactors[index]};
                binding.Mode = BindingMode.TwoWay;
                labels_listView.SetBinding(ListView.ItemsSourceProperty, binding);

            }
            catch (IndexOutOfRangeException ex)
            {
                MessageBox.Show("Index for binding was out of range! " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("General exception: " + ex.Message);
            }

        }

        private void addLabel_Click(object sender, RoutedEventArgs e)
        {
            OnAddLevel();
        }

        private void OnAddLevel()
        {
            //if we edit a label, remove the old one and replace the current list view item with the new label
            if ((newLabel_tb.Text != "") && (!_labels.Contains(newLabel_tb.Text)) && (labels_listView.SelectedItem != null))
            {
                int index = _labels.IndexOf(labels_listView.SelectedItem.ToString());
                _labels[index] = newLabel_tb.Text;
            }
            else if ((newLabel_tb.Text != "") && (!_labels.Contains(newLabel_tb.Text)) && (labels_listView.SelectedItem == null))
            {
                _labels.Add(newLabel_tb.Text);
            }
            newLabel_tb.Text = "";
            labels_listView.UnselectAll();
        }

        private void removeLabel_Click(object sender, RoutedEventArgs e)
        {
            if(labels_listView.SelectedItem != null) { 
                _labels.Remove(labels_listView.SelectedItem.ToString());
            }
        }

        private void labels_listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (labels_listView.SelectedItem != null)
                newLabel_tb.Text = labels_listView.SelectedItem.ToString();
            
        }

        //TODO: Remove this, no longer needed.
        private void buttonDone_Click(object sender, RoutedEventArgs e)
        {
            //pass back labels to the parent window
            LabelReturn?.Invoke(_labels);

            Close();
        }

    }
}
