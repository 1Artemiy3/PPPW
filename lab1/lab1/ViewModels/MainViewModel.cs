using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using MotorDepotApp.Models;
using MotorDepotApp.Services.Interfaces;

namespace MotorDepotApp.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IRequestGenerator _requestGenerator;
        private readonly IDispatcherService _dispatcherService;
        private readonly ITripSimulator _tripSimulator;

        public ObservableCollection<TripRequest> PendingRequests { get; } = new ObservableCollection<TripRequest>();
        public ObservableCollection<Driver> Drivers { get; } = new ObservableCollection<Driver>();
        public ObservableCollection<IVehicle> Vehicles { get; } = new ObservableCollection<IVehicle>();
        public ObservableCollection<Trip> ActiveTrips { get; } = new ObservableCollection<Trip>();

        private Trip _selectedTrip;
        public Trip SelectedTrip
        {
            get => _selectedTrip;
            set
            {
                _selectedTrip = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public ICommand GenerateRequestsCommand { get; }
        public ICommand DispatchAllCommand { get; }
        public ICommand SimulateStepCommand { get; }
        public ICommand RequestRepairCommand { get; }
        public ICommand RepairVehicleCommand { get; }

        public MainViewModel(
            IRequestGenerator requestGenerator,
            IDispatcherService dispatcherService,
            ITripSimulator tripSimulator)
        {
            _requestGenerator = requestGenerator;
            _dispatcherService = dispatcherService;
            _tripSimulator = tripSimulator;

            GenerateRequestsCommand = new RelayCommand(_ => GenerateDailyRequests());
            DispatchAllCommand = new RelayCommand(_ => DispatchRequests());
            SimulateStepCommand = new RelayCommand(_ => SimulateTrip(SelectedTrip), _ => CanSimulateTrip());
            RequestRepairCommand = new RelayCommand(_ => RequestRepair(SelectedTrip), _ => CanRequestRepair());
            RepairVehicleCommand = new RelayCommand(_ => RepairVehicle(SelectedTrip), _ => CanRepairVehicle());

            InitializeSeedData();
        }

        private void InitializeSeedData()
        {
            Vehicles.Add(new StandardTruck("Газель Next", "AA1122BB", 1500, 1));
            Vehicles.Add(new StandardTruck("MAN TGL", "AA3344BB", 5000, 2));
            Vehicles.Add(new HeavyTruck("Mercedes Actros", "AA5566BB", 10000, 3));
            Vehicles.Add(new HeavyTruck("Scania R500", "AA7788BB", 15000, 4));
            


            Drivers.Add(new Driver("Олександр Коваленко", 2));
            Drivers.Add(new Driver("Іван Сидоренко", 4));
            Drivers.Add(new Driver("Василь Мельник", 7));
            Drivers.Add(new Driver("Петро Шевченко", 10));
        }

        private void GenerateDailyRequests()
        {
            var newRequests = _requestGenerator.GenerateDailyRequests(3);
            int count = 0;
            foreach (var req in newRequests)
            {
                PendingRequests.Add(req);
                count++;
            }
            MessageBox.Show($"Згенеровано {count} нових заявок на перевезення!", "Нові заявки", MessageBoxButton.OK, MessageBoxImage.Information);
            RefreshCollections();
        }

        private void DispatchRequests()
        {
            try
            {
                var unassigned = PendingRequests.Where(r => !r.IsAssigned).ToList();
                int dispatchedCount = 0;

                foreach (var req in unassigned)
                {
                    var trip = _dispatcherService.CreateTrip(req, Drivers, Vehicles);
                    if (trip != null)
                    {
                        ActiveTrips.Add(trip);
                        dispatchedCount++;
                    }
                }

                // Видаліть розподілені заявки зі списку
                for (int i = PendingRequests.Count - 1; i >= 0; i--)
                {
                    if (PendingRequests[i].IsAssigned)
                    {
                        PendingRequests.RemoveAt(i);
                    }
                }

                RefreshCollections();

                if (dispatchedCount == 0 && unassigned.Any())
                {
                    MessageBox.Show("Не вдалося знайти вільних водіїв або підходящі авто для наявних заявок!", "Диспетчер", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else if (dispatchedCount > 0)
                {
                    MessageBox.Show($"Розподілено {dispatchedCount} рейсів!", "Диспетчер", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при розподіленні рейсів: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SimulateTrip(Trip trip)
        {
            if (trip == null) return;

            string originalStatus = trip.Status.ToString();
            _tripSimulator.ProgressTrip(trip);

            // Покажіть результат дії
            if (trip.Status == TripStatus.BrokenOnTrip)
            {
                MessageBox.Show("Автомобіль зламався під час руху! Подайте заявку на ремонт.", "Рейс", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (trip.Status == TripStatus.Completed)
            {
                MessageBox.Show($"Рейс успішно завершено!\n" +
                               $"Водій {trip.Driver.FullName} отримав: {trip.Driver.Balance:C0}", 
                               "Рейс завершено", MessageBoxButton.OK, MessageBoxImage.Information);
                // Видаліть завершений рейс зі списку активних
                ActiveTrips.Remove(trip);
                SelectedTrip = null;
            }

            RefreshCollections();
        }

        private void RequestRepair(Trip trip)
        {
            if (trip == null) return;

            if (trip.Status != TripStatus.BrokenOnTrip)
            {
                MessageBox.Show("Заявака на ремонт необхідна, тільки коли автомобіль зламався!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _tripSimulator.RequestRepair(trip);
            MessageBox.Show("Заявка на ремонт подана. Чекаємо фахівців СТО.", "Заявка подана", MessageBoxButton.OK, MessageBoxImage.Information);
            RefreshCollections();
        }

        private void RepairVehicle(Trip trip)
        {
            if (trip == null) return;

            if (trip.Vehicle.Status != VehicleStatus.UnderRepair)
            {
                MessageBox.Show("Автомобіль не знаходиться у статусі 'Під ремонтом'!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _tripSimulator.RepairVehicle(trip);
            MessageBox.Show("Ремонт завершено. Рейс продовжується!", "Ремонт готово", MessageBoxButton.OK, MessageBoxImage.Information);
            RefreshCollections();
        }

        private void RefreshCollections()
        {
            OnPropertyChanged(nameof(Drivers));
            OnPropertyChanged(nameof(Vehicles));
            OnPropertyChanged(nameof(ActiveTrips));
            OnPropertyChanged(nameof(PendingRequests));
            CommandManager.InvalidateRequerySuggested();
        }

        private bool CanSimulateTrip()
        {
            return SelectedTrip != null && 
                   (SelectedTrip.Status == TripStatus.InProgress || SelectedTrip.Status == TripStatus.Planned);
        }

        private bool CanRequestRepair()
        {
            return SelectedTrip != null && SelectedTrip.Status == TripStatus.BrokenOnTrip;
        }

        private bool CanRepairVehicle()
        {
            return SelectedTrip != null && SelectedTrip.Vehicle.Status == VehicleStatus.UnderRepair;
        }
    }
}
