using System.Windows;
using MotorDepotApp.Services.Implementations;
using MotorDepotApp.Services.Interfaces;
using MotorDepotApp.ViewModels;

namespace MotorDepotApp
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Реєстрація та складання залежностей (DIP)
            IVehicleSelectionStrategy vehicleStrategy = new OptimalVehicleSelectionStrategy();
            IDriverSelectionStrategy driverStrategy = new ExperienceDriverSelectionStrategy();
            IPaymentCalculator paymentCalculator = new StandardPaymentCalculator();
            IRequestGenerator requestGenerator = new RandomRequestGenerator();

            IDispatcherService dispatcherService = new DispatcherService(vehicleStrategy, driverStrategy);
            ITripSimulator tripSimulator = new TripSimulator(paymentCalculator);

            MainViewModel mainViewModel = new MainViewModel(requestGenerator, dispatcherService, tripSimulator);

            MainWindow mainWindow = new MainWindow(mainViewModel);
            mainWindow.Show();
        }
    }
}
