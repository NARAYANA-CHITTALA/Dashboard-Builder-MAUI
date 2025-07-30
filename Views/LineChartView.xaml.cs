using GamanaDashBoardApp.Viewmodels;
using Syncfusion.Maui.Charts;

namespace GamanaDashBoardApp.Views;

public partial class LineChartView : ContentView
{
    public LineChartView()
    {
        InitializeComponent();
        this.BindingContextChanged += OnBindingContextChanged;
    }

    private void OnBindingContextChanged(object sender, EventArgs e)
    {
        if (BindingContext is LineChartViewModel vm)
        {
            vm.PropertyChanged += OnViewModelPropertyChanged;
            UpdateChart();
        }
        else if (BindingContext is null && this.BindingContext is LineChartViewModel oldVm)
        {
            oldVm.PropertyChanged -= OnViewModelPropertyChanged;
        }
    }

    private void OnViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(LineChartViewModel.Data) ||
            e.PropertyName == nameof(LineChartViewModel.IsBusy))
        {
            UpdateChart();
        }
    }

    private void UpdateChart()
    {
        if (BindingContext is not LineChartViewModel vm || vm.IsBusy) return;

       
    }
}