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
using Xceed.Wpf.DataGrid;

namespace RCCDTool
{
    /// <summary>
    /// Interaction logic for DesignDisplay.xaml
    /// </summary>
    public partial class DesignDisplay : Window
    {
        private int NumSubjects;
        private IController _controller;

        public DesignDisplay()
        {
            InitializeComponent();
            updateDesign();
        }
        public DesignDisplay(IController controller)
        {
            InitializeComponent();
            this._controller = controller;
            //NumSubjects = numSubjects;
            updateDesign();
        }

        public void updateDesign()
        {
            tableSelect.ItemsSource = _controller.Tables;
            tableSelect.SelectedItem = _controller.Tables[0];
            DataGridCollectionView dcg = new DataGridCollectionView(_controller.ResearchDesignOutput.Tables[0].DefaultView);
            DesignTable.ItemsSource = dcg;
        }

        private void tableSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = (sender as ComboBox).SelectedIndex;

            string table = (string)(sender as ComboBox).Items[index];

            DataGridCollectionView dcg = new DataGridCollectionView(_controller.ResearchDesignOutput.Tables[table].DefaultView);
            DesignTable.ItemsSource = dcg;

        }
    }
}
