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

namespace RCCDTool
{
    /// <summary>
    /// Interaction logic for EditFactors.xaml
    /// </summary>
    partial class EditFactors : Window
    {
        private List<ResearchFactor> _factors;
        private int _numFactors;
        private IController _controller;
        
        public EditFactors(int numFactors, IController controller)
        {
            
            InitializeComponent();
            createGrid(numFactors);
            buildTable(numFactors);
            _factors = new List<ResearchFactor>();
            this._numFactors = numFactors;
            this._controller = controller;
        }

        private void createGrid(int numFactors)
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

            

        }

        public void buildTable(int numFactors)
        {
            //Dictionary<int, string> boxSettings = new Dictionary<int, string>();
            //boxSettings.Add(1, "Within Subjects Factor");
            //boxSettings.Add(2, "Between Subjects Factor");

            //Create Column Headers
            TextBlock col = new TextBlock();
            setHeaderProperties(col, "Factor Name:", 0, 0);

            col = new TextBlock();
            setHeaderProperties(col, "Number of Levels:", 0, 1);

            col = new TextBlock();
            setHeaderProperties(col, "Randomize factor:", 0, 2);

            col = new TextBlock();
            setHeaderProperties(col, "Within/Between Subjects:", 0, 3);


            ComboBox cb;
            TextBox tb;
            CheckBox checkbox;
            
            for (int i = 0; i < numFactors; i++)
            {
                //box for label
                tb = new TextBox();
                tb.Height = 20;
                tb.Width = 170;
                tb.Name = "factorName";

                Grid.SetRow(tb, i + 1);
                Grid.SetColumn(tb, 0);
                factorGrid.Children.Add(tb);

                //box for number of levels
                tb = new TextBox();
                tb.Height = 20;
                tb.Width = 170;
                tb.Name = "numLevels";
                
                Grid.SetRow(tb, i + 1);
                Grid.SetColumn(tb, 1);
                factorGrid.Children.Add(tb);

                //checkbox for isRandomized
                checkbox = new CheckBox();
                checkbox.HorizontalAlignment = HorizontalAlignment.Center;
                checkbox.VerticalAlignment = VerticalAlignment.Center;
                
                Grid.SetRow(checkbox, i + 1);
                Grid.SetColumn(checkbox, 2);
                factorGrid.Children.Add(checkbox);


                //combobox for within/between subjects selection
                cb = new ComboBox();
                cb.Width = 170;
                cb.Height = 20;
                
                cb.Items.Add("Within Subjects Factor");
                cb.Items.Add("Between Subjects Factor");
                Grid.SetRow(cb, i+1); // I guess this means that "whatever grid you put me in, I'll be in this row and this column. 
                Grid.SetColumn(cb, 3);
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

            for(int i = 1; i <= _numFactors; i++)
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
                this._controller.addFactor(newFactor);
                _factors.Add(newFactor);
                
            }
           
            this.Close();
        }


        internal List<ResearchFactor> Factors
        {
            get
            {
                return _factors;
            }
        }
    }


}
