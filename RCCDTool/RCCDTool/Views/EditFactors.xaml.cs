using System;
using System.Collections.Generic;
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
        //private IController _controller;
        private EditFactorsViewModel _efViewModel;
        private List<List<string>> listOfAllLabels;
        private List<ResearchFactor> researchFactors;

        //public EditFactorsViewer(int numFactors, IController controller)
        //{
            
        //    InitializeComponent();
        //    _controller = controller;
            
        //    _numFactors = numFactors;
        //    researchFactors = new List<ResearchFactor>();

        //    if (_controller.ModelHasData)
        //    {
        //        _numFactors = _controller.NumFactors < _numFactors ? numFactors : _controller.NumFactors;

        //    }
        //    listOfAllLabels = new List<List<string>>();
        //    for (int i = 0; i < _numFactors; i++)
        //        listOfAllLabels.Add(new List<string>());
            
        //    CreateGrid(_numFactors); 
        //    buildTable(_numFactors);
    
        //}

        public EditFactorsViewer(EditFactorsViewModel efViewModel)
        {

            InitializeComponent();
            _efViewModel = efViewModel;

            _numFactors = efViewModel.ResearchFactors.Count;
            researchFactors = new List<ResearchFactor>();

            //if (_controller.ModelHasData)
            //{
            //    _numFactors = _controller.NumFactors < _numFactors ? _numFactors : _controller.NumFactors;

            //}

            listOfAllLabels = new List<List<string>>();
            for (int i = 0; i < _numFactors; i++)
                listOfAllLabels.Add(new List<string>());

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
                    listOfAllLabels[i] = _efViewModel.ResearchFactors[i].Labels;   //(List<string>)_controller.FactorSet.Rows[i]["Labels"];

                }
                
            }


        }

        private void AddRow(int rowNum)
        {
            ResearchFactor rf = new ResearchFactor();
            _efViewModel.ResearchFactors.Add(rf);

            //Binding binding = new Binding();
            //binding.Source = rf;
            //binding.Path = new PropertyPath("Value");
            //binding.Mode = BindingMode.TwoWay;

            //box for label
            var tb = new TextBox
            {
                Height = 20,
                Width = 170,
                Name = "factorName",

                
            };

            //tb.DataContext = _efViewModel;
            Binding binding = new Binding("Name") {Source = _efViewModel.ResearchFactors[rowNum]};
            tb.SetBinding(TextBox.TextProperty, binding);
            
            Grid.SetRow(tb, rowNum + 1);
            Grid.SetColumn(tb, 0);

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
            var labelList = new ListBox { MinHeight = 45, MaxHeight = 45 };
            //set data binding 
            binding = new Binding("Labels") { Source = _efViewModel.ResearchFactors[rowNum] };
            labelList.SetBinding(ItemsControl.ItemsSourceProperty, binding);

            sp.Children.Add(labelList);
            sp.Children.Add(createlevels);

            //checkbox for isRandomized
            var checkbox = new CheckBox
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            binding = new Binding("IsRandomized") { Source = _efViewModel.ResearchFactors[rowNum] };
            checkbox.SetBinding(ItemsControl.ItemsSourceProperty, binding);

            Grid.SetRow(checkbox, rowNum + 1);
            Grid.SetColumn(checkbox, 2);



            //combobox for within/between subjects selection
            var cb = new ComboBox
            {
                Width = 150,
                Height = 20
            };

            cb.Items.Add("Within Subjects Factor");
            cb.Items.Add("Between Subjects Factor");

            binding = new Binding("IsWithinSubjects") { Source = _efViewModel.ResearchFactors[rowNum] };
            cb.SetBinding(ItemsControl.ItemsSourceProperty, binding);

            Grid.SetRow(cb, rowNum + 1); // I guess this means that "whatever grid you put me in, I'll be in this row and this column. 
            Grid.SetColumn(cb, 3);

            factorGrid.Children.Add(tb);
            factorGrid.Children.Add(sp);
            factorGrid.Children.Add(checkbox);
            factorGrid.Children.Add(cb);

            listOfAllLabels.Add(new List<string>());
            labelList.ItemsSource = listOfAllLabels[listOfAllLabels.Count-1];

            
        }

        private void createlevels_OnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            //get the row number and label of the button we clicked
            var rowNum = Grid.GetRow((sender as FrameworkElement).Parent as UIElement);
          
            //now get the listbox so we can reset the labels later
            var label = (sender as FrameworkElement).Parent.FindVisualChildren<ListBox>().First(l => l.GetType() == typeof(ListBox));

            List<string> newLabels = new List<string>();
            if((listOfAllLabels.Count >= rowNum-1) && (listOfAllLabels[rowNum-1] != null))
                newLabels = listOfAllLabels[rowNum-1];

            AddFactorLabels afl = new AddFactorLabels(newLabels);
            afl.LabelReturn += (fromChild) => newLabels = fromChild;
            afl.ShowDialog();
            if (label != null)
            {
                label.ItemsSource = null;
                label.ItemsSource = newLabels;
            }

            listOfAllLabels[rowNum-1] = newLabels;

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

        private void saveAndClose_Click(object sender, RoutedEventArgs e)
        {
            //if (_controller.ModelHasData)
            //    _controller.ClearFactors(); //resets the research factors, needed for updates

            ////add data from the grid to the model
            //for (int i = 1; i <= _numFactors; i++)
            //{
            //    var itemsInFirstRow = factorGrid.Children.Cast<UIElement>().Where(a => Grid.GetRow(a) == i);
 
            //    ResearchFactor newFactor = new ResearchFactor();
            //    foreach (UIElement uie in itemsInFirstRow)
            //    {
            //        if (uie is TextBox)
            //        {
            //            if ((uie as TextBox).Name == "factorName")
            //            {
            //                newFactor.Name = (uie as TextBox).Text;

            //                if (newFactor.Name == "" || newFactor.Name == null)
            //                {
            //                    throw new ArgumentNullException("Please enter a name for all factors listed!");
            //                }
            //            }
                        
            //        }
            //        else if (uie is ComboBox)
            //        {
            //            if ((uie as ComboBox).Text == "Within Subjects Factor")
            //                newFactor.IsWithinSubjects = true;
            //        }
            //        else if (uie is CheckBox)
            //        {
            //            newFactor.IsRandomized = (bool)(uie as CheckBox).IsChecked;
            //        }
            //        else if (uie is StackPanel)
            //        {
            //            newFactor.Labels = listOfAllLabels[i - 1];
            //            //newFactor.Levels = int.Parse((
            //            //    (uie as FrameworkElement).FindVisualChildren<TextBox>().First(l => l.GetType() == typeof(TextBox))).Text);
            //            newFactor.Levels = listOfAllLabels[i - 1].Count;
            //        }
            //    }

            //    _controller.addFactor(newFactor);
                
            //}
            
            //Close();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                factorGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                AddRow(factorGrid.RowDefinitions.Count-2);
                listOfAllLabels.Add(new List<string>());
                _numFactors++;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding blank row: " + ex.Message);
            }

        }
    }


}
