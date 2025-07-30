using GamanaDashboard.Models;
using GamanaDashBoardApp.Services;
using GamanaDashBoardApp.Viewmodels;
using GamanaDashBoardApp.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace GamanaDashBoardApp.Viewmodels
{
    public class LineChartViewModel : WidgetViewModel
    {
        private bool _isBusy;
        private ObservableCollection<HistoricalPriceData> _data = new();

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public bool IsNotBusy => !IsBusy;

        public ObservableCollection<HistoricalPriceData> Data
        {
            get => _data;
            set => SetProperty(ref _data, value);
        }

        public LineChartViewModel(WidgetConfig config, DashboardService dashboardService)
            : base(config, dashboardService)
        {
            LoadDataCommand = new Command(async () => await LoadData());
            LoadDataCommand.Execute(null);
        }

        public ICommand LoadDataCommand { get; }

        private async Task LoadData()
        {
            IsBusy = true;
            try
            {
                var newData = await _dashboardService.GetHistoricalData(
                    Config.CoinId, Config.Currency, Config.Days);

                Data.Clear();
                foreach (var item in newData)
                {
                    Data.Add(item);
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        public override View CreateView()
        {
            var chart = new LineChartView { BindingContext = this };
            return new Frame
            {
                Content = chart,
                BorderColor = Colors.Blue,
                CornerRadius = 8,
                Padding = 5
            };
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}