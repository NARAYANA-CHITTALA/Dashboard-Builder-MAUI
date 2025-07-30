using GamanaDashboard.Models;
using GamanaDashBoardApp.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace GamanaDashBoardApp.Viewmodels
{
    public class DashboardViewModel : BaseViewModel
    {
        private readonly DashboardService _dashboardService;
        private string _userId = "executive1";
        private int _gridRows = 2;
        private int _gridCols = 2;

        public ObservableCollection<WidgetViewModel> Widgets { get; } = new();

        public int GridRows
        {
            get => _gridRows;
            set => SetProperty(ref _gridRows, value);
        }

        public int GridCols
        {
            get => _gridCols;
            set => SetProperty(ref _gridCols, value);
        }

        public ICommand LoadDashboardCommand { get; }
        public ICommand SaveDashboardCommand { get; }
        public ICommand AddWidgetCommand { get; }

        public DashboardViewModel(DashboardService dashboardService)
        {
            _dashboardService = dashboardService;

            LoadDashboardCommand = new Command(async () => await LoadDashboard());
            SaveDashboardCommand = new Command(async () => await SaveDashboard());
            AddWidgetCommand = new Command<WidgetConfig>(AddWidget);

            LoadDashboardCommand.Execute(null);
        }

        private async Task LoadDashboard()
        {
            IsBusy = true;
            try
            {
                var config = await _dashboardService.GetDashboardConfig(_userId);
                GridRows = config.GridRows;
                GridCols = config.GridCols;

                Widgets.Clear();
                foreach (var widgetConfig in config.Widgets)
                {
                    AddWidget(widgetConfig);
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void AddWidget(WidgetConfig config)
        {
            if (string.IsNullOrWhiteSpace(config.Type))
            {
                throw new ArgumentException("Widget type cannot be null or empty");
            }

            WidgetViewModel vm = config.Type.ToLower() switch
            {
                "linechart" => new LineChartViewModel(config, _dashboardService),
                "barchart" => new BarChartViewModel(config, _dashboardService),
                "datagrid" => new DataGridViewModel(config, _dashboardService),
                _ => throw new ArgumentException($"Unknown widget type: {config.Type}")
            };

            Widgets.Add(vm);
        }

        public async Task SaveDashboard()
        {
            var config = new DashboardConfig
            {
                UserId = _userId,
                GridRows = GridRows,
                GridCols = GridCols,
                Widgets = Widgets.Select(x => x.Config).ToList()
            };

            await _dashboardService.SaveDashboardConfig(_userId, config);
        }
    }
}