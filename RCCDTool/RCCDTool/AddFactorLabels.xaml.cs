using System;
using System.Collections.Generic;
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

namespace RCCDTool
{
    /// <summary>
    /// Interaction logic for AddFactorLabels.xaml
    /// </summary>
    public partial class AddFactorLabels : Window
    {
        private List<string> _labels { get; set; }
        public event Action<List<string>> LabelReturn;
        public AddFactorLabels(List<string> labels)
        {
            InitializeComponent();
            _labels = labels ?? new List<string>();

            foreach (string label in _labels)
            {
                labels_listView.Items.Add(label);
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
                _labels.Remove(labels_listView.SelectedItem.ToString());
                _labels.Add(newLabel_tb.Text);
                labels_listView.Items[labels_listView.SelectedIndex] = newLabel_tb.Text;
            }
            else if ((newLabel_tb.Text != "") && (!_labels.Contains(newLabel_tb.Text)) && (labels_listView.SelectedItem == null))
            {
                _labels.Add(newLabel_tb.Text);
                labels_listView.Items.Add(newLabel_tb.Text);
            }
            newLabel_tb.Text = "";
            labels_listView.UnselectAll();
        }

        private void removeLabel_Click(object sender, RoutedEventArgs e)
        {
            if(labels_listView.SelectedItem != null) { 
                _labels.Remove(labels_listView.SelectedItem.ToString());
                labels_listView.Items.Remove(labels_listView.SelectedItem);
            }
        }

        private void labels_listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (labels_listView.SelectedItem != null)
                newLabel_tb.Text = labels_listView.SelectedItem.ToString();
            else
                newLabel_tb.Text = "";
            
        }

        private void buttonDone_Click(object sender, RoutedEventArgs e)
        {
            //pass back labels to the parent window
            LabelReturn?.Invoke(_labels);

            Close();
        }

        private void newLabel_tb_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
                OnAddLevel();
        }
    }
}
