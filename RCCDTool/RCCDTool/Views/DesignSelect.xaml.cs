using System.Linq;
using System.Windows;
using System.Windows.Controls;

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
            
            Model model = new Model();
            MainWindowViewModel mainWindowViewModel = new MainWindowViewModel(model);
            Close();
            //MainWindow mw = new MainWindow(checkedButton.Content.ToString(), model, controller);
            //mw.Show();

        }
        
    }
}
