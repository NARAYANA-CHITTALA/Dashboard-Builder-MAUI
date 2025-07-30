using GamanaDashboard.Models;
using GamanaDashBoardApp.Services;
using GamanaDashBoardApp.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace GamanaDashBoardApp.Viewmodels
{
    public class BarChartViewModel : WidgetViewModel
    {
        private ObservableCollection<CoinMarketData> _data = new();

        public ObservableCollection<CoinMarketData> Data
        {
            get => _data;
            set => SetProperty(ref _data, value);
        }

        public BarChartViewModel(WidgetConfig config, DashboardService dashboardService)
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
                var newData = await _dashboardService.GetMarketData(
                    Config.CompareCoinIds ?? new[] { Config.CoinId },
                    Config.Currency);

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
            return new BarChartView { BindingContext = this };
        }
    }
}