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
        private static DataTable factorSet;

        public MainWindow()
        {
            //InitializeComponent();
            
        }
        public MainWindow(string designSelection, IModel model, IController controller)
        {
            InitializeComponent();
            _factors = new List<ResearchFactor>();
            _designSelection = designSelection;
            designLabel.Content += designSelection;
            _controller = controller;
            updateFactorList();
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
                //List<ResearchFactor> factors = ef.Factors;
                //updateFactorList(factors);
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

        internal void updateFactorList()
        {
            //int nf = int.Parse(numFactors.Text.ToString());
            Label factorLabel;
            factorGrid.Children.Clear();
            //MessageBox.Show("Num Factors: " + _factors.Count);
            
            //for (int i = 0; i < 4; i++)
            //{
            //    factorGrid.ColumnDefinitions.Add(new ColumnDefinition());
            //}

            //for (int i = 0; i < _factors.Count; i++)
            //{
            //    factorGrid.RowDefinitions.Add(new RowDefinition());
            //}
            

            //for (int i = 0; i < _factors.Count; i++)
            //{
            //    factorGrid.RowDefinitions.Add(new RowDefinition());
            //    factorLabel = new Label();
            //    factorLabel.Content = _factors[i].ToString();
            //    Grid.SetRow(factorLabel, i);
            //    Grid.SetColumn(factorLabel, 0);
            //    factorGrid.Children.Add(factorLabel);
            //}


            factorSet = CreateDataTable();
            DataGridControl dgControl = new DataGridControl
            {
                ItemsSource = new DataGridCollectionView(FactorSet.DefaultView)
            };
        }

        public DataTable CreateDataTable()
        {

            ///this part sets up the structure of our datatable
            //http://stackoverflow.com/questions/701223/net-convert-generic-collection-to-datatable
            DataSet ds = new DataSet();
            Type entityType = typeof(ResearchFactor);
            DataTable dt = new DataTable(entityType.Name); // (typeof (ResearchFactor).ToString());
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

            foreach (PropertyDescriptor prop in properties)
            {
                dt.Columns.Add(prop.Name, prop.PropertyType);
            }

            //now we need to add the data to the table.
            foreach (ResearchFactor factor in _factors)
            {
                DataRow row = dt.NewRow();

                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(factor);
                }
            }
            ds.Tables.Add(dt);
            return dt;
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

            updateFactorList();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }


        public static DataTable FactorSet => factorSet;
    }
}
