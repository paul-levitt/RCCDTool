using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

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

        public MainWindow()
        {
            InitializeComponent();
        }
        public MainWindow(string designSelection, IModel model, IController controller)
        {
            InitializeComponent();
            _factors = new List<ResearchFactor>();
            _designSelection = designSelection;
            designLabel.Content += designSelection;
            _controller = controller;
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
            MessageBox.Show("Num Factors: " + _factors.Count);
            
            for (int i = 0; i < 4; i++)
            {
                factorGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int i = 0; i < _factors.Count; i++)
            {
                factorGrid.RowDefinitions.Add(new RowDefinition());
            }
            

            for (int i = 0; i < _factors.Count; i++)
            {
                factorGrid.RowDefinitions.Add(new RowDefinition());
                factorLabel = new Label();
                factorLabel.Content = _factors[i].ToString();
                Grid.SetRow(factorLabel, i);
                Grid.SetColumn(factorLabel, 0);
                factorGrid.Children.Add(factorLabel);
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
    }
}
