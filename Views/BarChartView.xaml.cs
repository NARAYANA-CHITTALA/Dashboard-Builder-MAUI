using GamanaDashBoardApp.Viewmodels;
using Syncfusion.Maui.Charts;

namespace GamanaDashBoardApp.Views
{
    public partial class BarChartView : ContentView
    {
        public BarChartView()
        {
            InitializeComponent();
            this.BindingContextChanged += OnBindingContextChanged;
        }

        private void OnBindingContextChanged(object sender, EventArgs e)
        {
            if (BindingContext is BarChartViewModel vm)
            {
                vm.PropertyChanged += OnViewModelPropertyChanged;
                UpdateChart();
            }
            else if (BindingContext is null && this.BindingContext is BarChartViewModel oldVm)
            {
                oldVm.PropertyChanged -= OnViewModelPropertyChanged;
            }
        }

        private void OnViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(BarChartViewModel.Data) ||
                e.PropertyName == nameof(BarChartViewModel.IsBusy))
            {
                UpdateChart();
            }
        }

        private void UpdateChart()
        {
            if (BindingContext is not BarChartViewModel vm || vm.IsBusy) return;

            // Clear existing series
            chartView.Series.Clear();

            // Create and configure the column series
            var columnSeries = new ColumnSeries()
            {
                ItemsSource = vm.Data,
                XBindingPath = "Name",
                YBindingPath = "CurrentPrice",
                ShowDataLabels = true
            };

            // Configure data labels
            columnSeries.DataLabelSettings = new CartesianDataLabelSettings()
            {
                LabelPlacement = DataLabelPlacement.Outer
            };

            // Add series to chart
            chartView.Series.Add(columnSeries);

            // Configure axes
            var xAxis = new CategoryAxis();
            xAxis.Title = new ChartAxisTitle() { Text = "Cryptocurrency" };

            var yAxis = new NumericalAxis();
            yAxis.Title = new ChartAxisTitle() { Text = "Price (USD)" };

            chartView.XAxes.Clear();
            chartView.YAxes.Clear();
            chartView.XAxes.Add(xAxis);
            chartView.YAxes.Add(yAxis);
        }
    }
}