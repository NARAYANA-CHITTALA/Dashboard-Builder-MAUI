using GamanaDashboard.Models;
using GamanaDashBoardApp.Viewmodels;
using Microsoft.Maui.Controls;

namespace GamanaDashBoardApp.Views;

public partial class Dashboard : ContentPage
{
    private readonly DashboardViewModel _viewModel;

    public Dashboard(DashboardViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
        LayoutPicker.SelectedIndex = 0;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        BuildGrid(_viewModel.GridRows, _viewModel.GridCols);
    }

    private void BuildGrid(int rows, int cols)
    {
        DynamicGrid.Children.Clear();
        DynamicGrid.RowDefinitions.Clear();
        DynamicGrid.ColumnDefinitions.Clear();

        for (int i = 0; i < rows; i++)
            DynamicGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });

        for (int j = 0; j < cols; j++)
            DynamicGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

        // Place existing widgets
        foreach (var widget in _viewModel.Widgets)
        {
            var view = widget.CreateView();
            DynamicGrid.Add(view, widget.Config.Column, widget.Config.Row);
        }

        // Create drop targets for empty cells
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                if (_viewModel.Widgets.All(w => w.Config.Row != row || w.Config.Column != col))
                {
                    var dropCell = CreateDropContainer(row, col);
                    DynamicGrid.Add(dropCell, col, row);
                }
            }
        }
    }

    private ContentView CreateDropContainer(int row, int col)
    {
        var dropContainer = new ContentView
        {
            BackgroundColor = Colors.Transparent,
            HorizontalOptions = LayoutOptions.FillAndExpand,
            VerticalOptions = LayoutOptions.FillAndExpand
        };

        var dropGesture = new DropGestureRecognizer { AllowDrop = true };
        dropGesture.Drop += async (s, e) =>
        {
            try
            {
                if (e.Data.Properties["WidgetType"] is string widgetType)
                {
                    var config = new WidgetConfig
                    {
                        Type = widgetType switch
                        {
                            "Line Chart" => "LineChart",
                            "Bar Chart" => "BarChart",
                            "Data Grid" => "DataGrid",
                            _ => throw new ArgumentException("Unknown widget type from drag")
                        },
                        Row = row,
                        Column = col,
                        CoinId = "bitcoin" // Default
                    };

                    _viewModel.AddWidget(config);
                    await _viewModel.SaveDashboard();
                    BuildGrid(_viewModel.GridRows, _viewModel.GridCols);
                }
            }
            catch (ArgumentException ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        };

        dropContainer.GestureRecognizers.Add(dropGesture);
        return dropContainer;
    }
    private void OnPickerSelectedIndexChanged(object sender, EventArgs e)
    {
        string selectedLayout = LayoutPicker.SelectedItem?.ToString()!;
        (int rows, int cols) = selectedLayout switch
        {
            "2x2" => (2, 2),
            "3x3" => (3, 3),
            "4x4" => (4, 4),
            "4x3" => (4, 3),
            _ => (2, 2)
        };

        _viewModel.GridRows = rows;
        _viewModel.GridCols = cols;
        BuildGrid(rows, cols);
    }

    // Add this method to handle drag starting from sidebar widgets
    private void OnDragStarting(object sender, DragStartingEventArgs e)
    {
        if (sender is DragGestureRecognizer recognizer &&
            recognizer.Parent is Frame frame &&
            frame.Content is Label label)
        {
            e.Data.Properties["WidgetType"] = label.Text;
        }
    }

    private View CreatePreview(string widgetType) => new Frame
    {
        BackgroundColor = Color.FromRgba(255, 255, 255, 150),
        BorderColor = Colors.Red,
        CornerRadius = 8,
        Margin = 2,
        Content = new Label
        {
            Text = widgetType,
            TextColor = Colors.Black,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        }
    };

    private View CreateWidget(string label, Color borderColor) => new Frame
    {
        BackgroundColor = Colors.White,
        BorderColor = borderColor,
        CornerRadius = 8,
        Margin = 2,
        Content = new Label
        {
            Text = label,
            TextColor = Colors.Black,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        }
    };
}