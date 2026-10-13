using System.Windows;
using MotorDepotApp.ViewModels;

namespace MotorDepotApp
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
