using System.Windows;

namespace RCCDTool
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static App()
        {

        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Model model = new Model();
            MainWindowViewModel mainWindowViewModel = new MainWindowViewModel(model);

            MainWindow mw = new MainWindow();
            mw.DataContext = mainWindowViewModel;

            //mainWindowViewModel.StartApp();
            mw.Show();
        }
    }

}
