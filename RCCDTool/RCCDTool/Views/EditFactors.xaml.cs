using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interop;
using System.Windows.Threading;
using RCCDTool.ViewModels;
using Xceed.Wpf.AvalonDock.Controls;

namespace RCCDTool
{
    /// <summary>
    /// Interaction logic for EditFactors.xaml
    /// </summary>
    partial class EditFactorsViewer : Window
    {
        private int _numFactors;
        private EditFactorsViewModel _efViewModel;
        private List<ResearchFactor> researchFactors;
        

        public EditFactorsViewer(EditFactorsViewModel efViewModel)
        {

            InitializeComponent();
            _efViewModel = efViewModel;

            _numFactors = efViewModel.ResearchFactors.Count;
            researchFactors = new List<ResearchFactor>();

            CreateGrid(_numFactors);
            buildTable(_numFactors);
        }


        private void CreateGrid(int numFactors)
        {
            factorGrid.ShowGridLines = false;
            this.MinWidth = 850;
            this.MinHeight = 500;
            
            //create columns            
            ColumnDefinition col1 = new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }; 
            ColumnDefinition col2 = new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) };
            ColumnDefinition col3 = new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) };
            ColumnDefinition col4 = new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) };
            //ColumnDefinition col5 = new ColumnDefinition();

            factorGrid.ColumnDefinitions.Add(col1);
            factorGrid.ColumnDefinitions.Add(col2);
            factorGrid.ColumnDefinitions.Add(col3);
            factorGrid.ColumnDefinitions.Add(col4);
            //factorGrid.ColumnDefinitions.Add(col5);

            //create rows based on number of factors
            for (int i = 0; i <= numFactors; i++)
                factorGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto } );

            //Create Column Headers
            TextBlock col = new TextBlock();
            setHeaderProperties(col, "Factor Name:", 0, 0);

            col = new TextBlock();
            setHeaderProperties(col, "Factor Levels:", 0, 1);

            col = new TextBlock();
            setHeaderProperties(col, "Randomize factor:", 0, 2);

            col = new TextBlock();
            setHeaderProperties(col, "Within/Between Subjects:", 0, 3);

            //col = new TextBlock();
            //setHeaderProperties(col, "Factor Labels:", 0, 4);

        }

        public void buildTable(int numFactors)
        {
            for (int i = 0; i < numFactors; i++)
            {
                
                AddRow(i);
                
                //if we already have data, put it in the grid for editing.
                if (_efViewModel.ResearchFactors.Count > i)
                {
                    var nameBox = (TextBox)factorGrid.FindVisualChildren<TextBox>().First(control => Grid.GetRow(control) == i+1 && Grid.GetColumn(control) == 0);
                    //var numLevels = (TextBox)factorGrid.FindVisualChildren<TextBox>().First(control => Grid.GetRow(control) == i+1 && Grid.GetColumn(control) == 1);
                    var checkbox = (CheckBox)factorGrid.FindVisualChildren<CheckBox>().First(control => Grid.GetRow(control) == i+1 && Grid.GetColumn(control) == 2);
                    var cb = (ComboBox)factorGrid.FindVisualChildren<ComboBox>().First(control => Grid.GetRow(control) == i+1 && Grid.GetColumn(control) == 3);
                    var piece1 =
                        factorGrid.FindVisualChildren<StackPanel>()
                            .First(control => Grid.GetRow(control) == i + 1 && Grid.GetColumn(control) == 1);
                    var labelList = (ListBox)piece1.Children[0];//(ListBox)piece1.FindVisualChildren<ListBox>();

                    nameBox.Text = _efViewModel.ResearchFactors[i].Name;//_controller.FactorSet.Rows[i]["Name"].ToString();
                    //numLevels.Text = _controller.FactorSet.Rows[i]["Levels"].ToString();
                    checkbox.IsChecked = _efViewModel.ResearchFactors[i].IsRandomized;//_controller.FactorSet.Rows[i].Field<bool>("IsRandomized");
                    cb.SelectedItem = _efViewModel.ResearchFactors[i].IsWithinSubjects; // _controller.FactorSet.Rows[i].Field<bool>("IsWithinSubjects") ? "Within Subjects Factor" : "Between Subjects Factor";
                    labelList.ItemsSource = _efViewModel.ResearchFactors[i].Labels;//(List<string>)_controller.FactorSet.Rows[i]["Labels"];
                    //listOfAllLabels[i] = _efViewModel.ResearchFactors[i].Labels;   //(List<string>)_controller.FactorSet.Rows[i]["Labels"];

                }
                
            }


        }

        /// <summary>
        /// Adds a row and associated bindings for each UIElement to the grid.
        /// </summary>
        /// <param name="rowNum"></param>
        private void AddRow(int rowNum)
        {
            ResearchFactor rf = new ResearchFactor();

            //Create a new research factor if there isn't one already, otherwise select that research factor
            if(_efViewModel.ResearchFactors.Count <= rowNum)
                _efViewModel.ResearchFactors.Add(rf);
            else
                rf = _efViewModel.ResearchFactors[rowNum];
            

            //box for label
            var tb = new TextBox
            {
                Height = 20,
                Width = 170,
                Name = "factorName",

                
            };

            Binding binding = new Binding("Name") { Source = rf };
            binding.Mode = BindingMode.TwoWay;
            tb.SetBinding(TextBox.TextProperty, binding);
            
            Grid.SetRow(tb, rowNum + 1);
            Grid.SetColumn(tb, 0);

            //Button for creating levels
            var createlevels = new Button
            {
                Content = "Create levels",
                Width = 80,
                Height = 20,
                HorizontalAlignment = HorizontalAlignment.Left
            };

            createlevels.Click += createlevels_OnClick;
            StackPanel sp = new StackPanel();
            sp.Margin = new Thickness(10);
            Grid.SetRow(sp, rowNum + 1);
            Grid.SetColumn(sp, 1);


            //Listbox for the labels to be displayed in
            var labelList = new ListBox { MinHeight = 45, MaxHeight = 45 };
                        
            binding = new Binding("Labels") { Source = rf };
            binding.Mode = BindingMode.TwoWay;
            labelList.SetBinding(ListBox.ItemsSourceProperty, binding);

            sp.Children.Add(labelList);
            sp.Children.Add(createlevels);


            //checkbox for isRandomized
            var checkbox = new CheckBox
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };


            binding = new Binding("IsRandomized") { Source = rf };
            binding.Mode = BindingMode.TwoWay;
            checkbox.SetBinding(CheckBox.IsCheckedProperty, binding);

            Grid.SetRow(checkbox, rowNum + 1);
            Grid.SetColumn(checkbox, 2);
            

            //combobox for within/between subjects selection
            var cb = new ComboBox
            {
                Width = 150,
                Height = 20
            };

            string ComboBoxTrueValue = _efViewModel.DesignTypes[0]; //"Within Subjects Factor";
            string ComboBoxFalseValue = _efViewModel.DesignTypes[1]; //"Between Subjects Factor";


            Binding binding2 = new Binding("DesignTypes") { Source = _efViewModel };
            binding2.Mode = BindingMode.TwoWay;
            cb.SetBinding(ComboBox.ItemsSourceProperty, binding2);

            binding = new Binding("IsWithinSubjects") { Source = rf, Converter = new StringToBoolConverter { TrueValue = ComboBoxTrueValue, FalseValue = ComboBoxFalseValue } };
            binding.Mode = BindingMode.TwoWay;
            cb.SetBinding(ComboBox.SelectedValueProperty, binding);


            Grid.SetRow(cb, rowNum + 1); // I guess this means that "whatever grid you put me in, I'll be in this row and this column. 
            Grid.SetColumn(cb, 3);

            factorGrid.Children.Add(tb);
            factorGrid.Children.Add(sp);
            factorGrid.Children.Add(checkbox);
            factorGrid.Children.Add(cb);
            
        }

        /// <summary>
        /// Pops up the dialog box for the user to add levels.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="routedEventArgs"></param>
        private void createlevels_OnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            
            //get the row number and label of the button we clicked
            var rowNum = Grid.GetRow((sender as FrameworkElement).Parent as UIElement) -1; //-1: offset for the header row

            AddFactorLabels afl = new AddFactorLabels(_efViewModel, rowNum);
            
            afl.ShowDialog();
            
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

        //TODO: Validation on saving.
        private void saveAndClose_Click(object sender, RoutedEventArgs e)
        {
 
            var thing = _efViewModel.ResearchFactors;
            
            Close();
        }

        /// <summary>
        /// Begins the process of adding a row. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                factorGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                AddRow(factorGrid.RowDefinitions.Count-2);
                _numFactors++;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding blank row: " + ex.Message);
            }

        }
    }


}
