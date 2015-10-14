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
    /// Interaction logic for DesignSelect.xaml
    /// </summary>
    public partial class DesignSelect : Window
    {
        public DesignSelect()
        {
            InitializeComponent();
        }

        private void Continue_Click(object sender, RoutedEventArgs e)
        {
            var checkedButton = designSelection.Children.OfType<RadioButton>().Where(r => r.IsChecked == true).FirstOrDefault();
            
            MainWindow mw = new MainWindow(checkedButton.Content.ToString());
            mw.Show();
            this.Close();
        }
        
    }
}
