using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Xceed.Wpf.Toolkit;
using Xceed.Wpf.DataGrid;
using Xceed.Wpf.DataGrid.Converters;
using Xceed.Wpf.DataGrid.ValidationRules;
using Xceed.Wpf.DataGrid.Views;
using DataRow = System.Data.DataRow;
//using Xceed.Wpf.DataGrid.Settings;
using MessageBox = Xceed.Wpf.Toolkit.MessageBox;

namespace RCCDTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IObserver<ResearchFactor>
    {
        private string _designSelection;
        //private IModel _model;
        private IController _controller;
        List<ResearchFactor> _factors;
        private DataTable factorSet;
        private DataGridControl dgControl;

        public MainWindow()
        {
            //InitializeComponent();
            
        }
        public MainWindow(string designSelection, IModel model, IController controller)
        {
            Type entityType = typeof(ResearchFactor);
            factorSet = new DataTable(entityType.Name);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

            foreach (PropertyDescriptor prop in properties)
            {
                factorSet.Columns.Add(prop.Name, prop.PropertyType);
            }

            _factors = new List<ResearchFactor>();
            //factorSet = UpdateDataTable();
            InitializeComponent();
            
            _designSelection = designSelection;
            designLabel.Content += designSelection;
            _controller = controller;
            
            dgControl = new DataGridControl
            {
                ItemsSource = new DataGridCollectionView(FactorSet.DefaultView)
            };
            
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            int nPerGroupCalc = (Int32.Parse(totalN.Text) / Int32.Parse(numFactors.Text));
            nPerGroup.Text = nPerGroupCalc.ToString();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int nf = int.Parse(numFactors.Text);
                EditFactors ef = new EditFactors(nf, _controller);
                _controller.AddSubscriber(ef);
                ef.Show();
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
        }

 

        public void UpdateDataTable()
        {

            ///this part sets up the structure of our datatable
            //http://stackoverflow.com/questions/701223/net-convert-generic-collection-to-datatable
            
            Type entityType = typeof(ResearchFactor);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

            //now we need to add the data to the table.
            foreach (ResearchFactor factor in _factors)
            {
                DataRow row = factorSet.NewRow();

                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(factor);
                }

                factorSet.Rows.Add(row);
               
            }

        }



        public void OnNext(ResearchFactor value)
        {
            //TODO: Fix factor updates. Currently, factors are NOT getting updated correctly in the main window. 
            //They are not getting deleted as they should be.
            if ((value != null) && (_factors.Contains(value)))
            {
                _factors.Remove(value);
            }
            else if((value != null) && (!_factors.Contains(value))){
                _factors.Add(value);
            }

            UpdateDataTable();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }


        public DataTable FactorSet => factorSet;
    }
}
