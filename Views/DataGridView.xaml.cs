using GamanaDashBoardApp.Viewmodels;
using Syncfusion.Maui.DataGrid;

namespace GamanaDashBoardApp.Views
{
    public partial class DataGridView : ContentView
    {
        public DataGridView()
        {
            InitializeComponent();
            this.BindingContextChanged += OnBindingContextChanged;
        }

        private void OnBindingContextChanged(object sender, EventArgs e)
        {
            if (BindingContext is DataGridViewModel vm)
            {
                vm.PropertyChanged += OnViewModelPropertyChanged;
                UpdateGrid();
            }
            else if (BindingContext is null && this.BindingContext is DataGridViewModel oldVm)
            {
                oldVm.PropertyChanged -= OnViewModelPropertyChanged;
            }
        }

        private void OnViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(DataGridViewModel.Data) ||
                e.PropertyName == nameof(DataGridViewModel.IsBusy))
            {
                UpdateGrid();
            }
        }

        private void UpdateGrid()
        {
            if (BindingContext is not DataGridViewModel vm || vm.IsBusy) return;

            // Configure grid columns if not already set
            if (dataGrid.Columns.Count == 0)
            {
                dataGrid.Columns.Add(new DataGridTextColumn()
                {
                    MappingName = "Name",
                    HeaderText = "Coin",
                    Width = 120
                });

                dataGrid.Columns.Add(new DataGridTextColumn()
                {
                    MappingName = "CurrentPrice",
                    HeaderText = "Price",
                    Format = "C2",
                    Width = 100
                });

                dataGrid.Columns.Add(new DataGridTextColumn()
                {
                    MappingName = "PriceChangePercentage24H",
                    HeaderText = "24H Change",
                    Format = "P2",
                    Width = 100
                });

                dataGrid.Columns.Add(new DataGridTextColumn()
                {
                    MappingName = "MarketCap",
                    HeaderText = "Market Cap",
                    Format = "C0",
                    Width = 120
                });
            }

            // Set the items source
            dataGrid.ItemsSource = vm.Data;
        }
    }
}