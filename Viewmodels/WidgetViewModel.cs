using GamanaDashboard.Models;
using GamanaDashBoardApp.Services;
using GamanaDashBoardApp.Viewmodels;

namespace GamanaDashBoardApp.Viewmodels
{
    public abstract class WidgetViewModel : BaseViewModel
    {
        protected readonly DashboardService _dashboardService;

        public WidgetConfig Config { get; }

        protected WidgetViewModel(WidgetConfig config, DashboardService dashboardService)
        {
            Config = config;
            _dashboardService = dashboardService;
        }

        public abstract View CreateView();
    }
}