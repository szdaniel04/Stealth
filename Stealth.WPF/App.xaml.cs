using Stealth.ViewModel;
using System.Configuration;
using System.Data;
using System.Windows;

namespace Stealth
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private MainViewModel? _mainViewModel;
        private MainWindow? _mainWindow;

        public App()
        {
            Startup += OnStartup;
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            _mainViewModel = new MainViewModel();
            _mainWindow = new MainWindow
            {
                DataContext = _mainViewModel
            };
            _mainWindow.KeyDown += _mainViewModel.KeyPressed;
            _mainWindow.Show();
        }
    }

}
