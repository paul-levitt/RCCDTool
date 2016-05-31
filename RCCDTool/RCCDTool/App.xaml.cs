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
            MainWindow mw = new MainWindow();

            MainWindowViewModel mainWindowViewModel = new MainWindowViewModel(model);
            mw.DataContext = mainWindowViewModel;
            //mainWindowViewModel.StartApp();
            mw.Show();
        }
    }

}
