using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace RCCDTool
{
    /// <summary>
    /// Interaction logic for EditFactors.xaml
    /// </summary>
    partial class EditFactors : Window
    {
        private int _numFactors;
        private IController _controller;
        
        public EditFactors(int numFactors, IController controller)
        {
            
            InitializeComponent();
            _controller = controller;
            
            _numFactors = numFactors;

            if (_controller.ModelHasData)
            {
                _numFactors = _controller.NumFactors < _numFactors ? numFactors : _controller.NumFactors;
                
            }
        
            CreateGrid(_numFactors);
            buildTable(_numFactors);
    
        }

      
        private void CreateGrid(int numFactors)
        {
            factorGrid.ShowGridLines = true;

            //create columns            
            ColumnDefinition col1 = new ColumnDefinition();
            ColumnDefinition col2 = new ColumnDefinition();
            ColumnDefinition col3 = new ColumnDefinition();
            ColumnDefinition col4 = new ColumnDefinition();
            factorGrid.ColumnDefinitions.Add(col1);
            factorGrid.ColumnDefinitions.Add(col2);
            factorGrid.ColumnDefinitions.Add(col3);
            factorGrid.ColumnDefinitions.Add(col4);

            //create rows based on number of factors
            for (int i = 0; i <= numFactors; i++)
                factorGrid.RowDefinitions.Add(new RowDefinition());

            //Create Column Headers
            TextBlock col = new TextBlock();
            setHeaderProperties(col, "Factor Name:", 0, 0);

            col = new TextBlock();
            setHeaderProperties(col, "Number of Levels:", 0, 1);

            col = new TextBlock();
            setHeaderProperties(col, "Randomize factor:", 0, 2);

            col = new TextBlock();
            setHeaderProperties(col, "Within/Between Subjects:", 0, 3);

        }

        public void buildTable(int numFactors)
        {
            for (int i = 0; i < numFactors; i++)
            {
                //box for label
                var tb = new TextBox
                {
                    Height = 20,
                    Width = 170,
                    Name = "factorName"
                };


                Grid.SetRow(tb, i + 1);
                Grid.SetColumn(tb, 0);
                

                //box for number of levels
                var tb2 = new TextBox
                {
                    Height = 20,
                    Width = 170,
                    Name = "numLevels"
                };

                Grid.SetRow(tb2, i + 1);
                Grid.SetColumn(tb2, 1);
                

                //checkbox for isRandomized
                var checkbox = new CheckBox
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };

                Grid.SetRow(checkbox, i + 1);
                Grid.SetColumn(checkbox, 2);
                


                //combobox for within/between subjects selection
                var cb = new ComboBox
                {
                    Width = 170,
                    Height = 20
                };

                cb.Items.Add("Within Subjects Factor");
                cb.Items.Add("Between Subjects Factor");
                Grid.SetRow(cb, i+1); // I guess this means that "whatever grid you put me in, I'll be in this row and this column. 
                Grid.SetColumn(cb, 3);

                if (_controller.NumFactors > i)
                {
                    tb.Text = _controller.FactorSet.Rows[i]["Label"].ToString();
                    tb2.Text = _controller.FactorSet.Rows[i]["Levels"].ToString();
                    checkbox.IsChecked = _controller.FactorSet.Rows[i].Field<bool>("IsRandomized");
                    cb.SelectedItem = _controller.FactorSet.Rows[i].Field<bool>("IsWithinSubjects") ? "Within Subjects Factor" : "Between Subjects Factor";
                }

                factorGrid.Children.Add(tb);
                factorGrid.Children.Add(tb2);
                factorGrid.Children.Add(checkbox);
                factorGrid.Children.Add(cb);

            }


        }
        
        private void setHeaderProperties(TextBlock element, string name, int row, int column)
        {
            element.Text = name;
            element.FontSize = 14;
            element.FontWeight = FontWeights.Bold;
            element.VerticalAlignment = VerticalAlignment.Center;
            element.HorizontalAlignment = HorizontalAlignment.Center;
            Grid.SetColumn(element, column);
            Grid.SetRow(element, row);
            factorGrid.Children.Add(element);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (_controller.ModelHasData)
                _controller.ClearFactors(); //resets the research factors, needed for updates

            for (int i = 1; i <= _numFactors; i++)
            {
                var itemsInFirstRow = factorGrid.Children.Cast<UIElement>().Where(a => Grid.GetRow(a) == i);
 
                ResearchFactor newFactor = new ResearchFactor();
                foreach (UIElement uie in itemsInFirstRow)
                {
                    if (uie is TextBox)
                    {
                        if ((uie as TextBox).Name == "factorName")
                            newFactor.Label = (uie as TextBox).Text;
                        else
                            newFactor.Levels = int.Parse((uie as TextBox).Text);
                    }
                    else if (uie is ComboBox)
                    {
                        if ((uie as ComboBox).Text == "Within Subjects Factor")
                            newFactor.isWithinSubjects = true;
                    }
                    else if (uie is CheckBox)
                    {
                        newFactor.IsRandomized = (bool)(uie as CheckBox).IsChecked;
                    }
                }

                _controller.addFactor(newFactor);
                
            }
            
            Close();
        }

    }


}
