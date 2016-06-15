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
            InitializeComponent();
            
        }


        private void button_Click(object sender, RoutedEventArgs e)
        {
            //int nPerGroupCalc = (Int32.Parse(totalN.Text) / Int32.Parse(numFactors.Text));
            //nPerGroup.Text = nPerGroupCalc.ToString();
            var dc = FactorsGrid.DataContext;

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

    }
}
