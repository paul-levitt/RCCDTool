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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RCCDTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _designSelection;
        public MainWindow()
        {
            InitializeComponent();
        }
        public MainWindow(string designSelection)
        {
            InitializeComponent();
            this._designSelection = designSelection;
            designLabel.Content += designSelection;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            int nPerGroupCalc = (Int32.Parse(totalN.Text.ToString()) / Int32.Parse(numFactors.Text.ToString()));
            nPerGroup.Text = nPerGroupCalc.ToString();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int nf = int.Parse(numFactors.Text.ToString());
                EditFactors ef = new EditFactors(nf, this);
                ef.Show();
                List<Factor> factors = ef.Factors;
                //updateFactorList(factors);
            }
            catch (FormatException exception)
            {
                MessageBox.Show("Invalid entry for number of factors! Please enter an integer greater than 1.");
                MessageBox.Show("Exception was:" + exception.ToString());
            }
            catch (Exception exception)
            {
                MessageBox.Show("Exception was:" + exception.ToString());
            }
        }

        internal void updateFactorList(List<Factor> factors)
        {
            int nf = int.Parse(numFactors.Text.ToString());
            Label factorLabel;
            for (int i = 0; i < nf; i++)
            {
                factorGrid.RowDefinitions.Add(new RowDefinition());
                factorLabel = new Label();
                factorLabel.Content = factors[i].ToString();
                Grid.SetRow(factorLabel, i);
                Grid.SetColumn(factorLabel, 0);
                factorGrid.Children.Add(factorLabel);
            }
            
        }
    }
}
