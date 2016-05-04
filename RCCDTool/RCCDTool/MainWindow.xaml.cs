using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using Xceed.Wpf.Toolkit;
using Xceed.Wpf.DataGrid;
using Xceed.Wpf.DataGrid.Converters;
using Xceed.Wpf.DataGrid.ValidationRules;
using Xceed.Wpf.DataGrid.Views;
using DataRow = System.Data.DataRow;
using MessageBox = System.Windows.MessageBox;

//using Xceed.Wpf.DataGrid.Settings;
//using MessageBox = Xceed.Wpf.Toolkit.MessageBox;

namespace RCCDTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _designSelection;
        private IController _controller;

        public MainWindow()
        {
            //InitializeComponent();
            
        }
        public MainWindow(string designSelection, IController controller)
        {
            
            InitializeComponent();
            
            _designSelection = designSelection;
            designLabel.Content += designSelection;
            _controller = controller;
            
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            int nPerGroupCalc = (Int32.Parse(totalN.Text) / Int32.Parse(numFactors.Text));
            nPerGroup.Text = nPerGroupCalc.ToString();
        }

        private void editFactors_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int nf = int.Parse(numFactors.Text);
                EditFactorsViewer ef = new EditFactorsViewer(nf, _controller);
               
                ef.Show();
                FactorsGrid.Items.Refresh();
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

        private void generateDesign_Click(object sender, RoutedEventArgs e)
        {

            int numSubjects;
            try
            {
                numSubjects = int.Parse(totalN.Text);
                if (numSubjects <= 0)
                {
                    throw new ArgumentException("Number of subjects must be greater than 0.");
                }

                _controller.GenerateDesign(numSubjects);
                
            }
            catch (FormatException)
            {
                    MessageBox.Show("Invalid entry for number of people. Please enter an integer greater than 1.");              

            }
            catch (ArgumentException exception)
            {
                MessageBox.Show(exception.Message);
            }
         
        }

        private void loadFactorSet_Click(object sender, RoutedEventArgs e)
        {
            
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "FactorSet files (.fac)|*.fac",
                DefaultExt = ".fac"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                _controller.LoadFactorSet(openFileDialog.FileName);

                numFactors.Text = _controller.FactorSet?.Rows.Count.ToString();
            }
        }

        private void saveFactorSet_Click(object sender, RoutedEventArgs e)
        {
            if (_controller.FactorSet.Rows.Count == 0)
            {
                MessageBox.Show("Please input factors into the model before saving.", "Please enter data");
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                FileName = "FactorSet",
                DefaultExt = ".fac",
                Filter = "FactorSet files (.fac)|*.fac"
            };
            
            if (saveFileDialog.ShowDialog() == true)
            {
                 _controller.SaveFactorSet(saveFileDialog.FileName);   
            }
        }
    }
}
