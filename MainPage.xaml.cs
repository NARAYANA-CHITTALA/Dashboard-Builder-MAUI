using GamanaDashBoardApp.Models;
using GamanaDashBoardApp.Viewmodels;
using GamanaDashBoardApp.Views;
using System.Text.Json;

namespace GamanaDashBoardApp
{
    public partial class MainPage : ContentPage
    {
        private bool _isEditMode = false;
        private View _dragTarget;
        private double _dragStartX, _dragStartY;
        private Microsoft.Maui.Graphics.Rect _originalBounds;
        private View _selectedFrame;
        private string _desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);


        public MainPage()
        {
            InitializeComponent();
            var vm = new MainPageViewModel();
            BindingContext = vm;
            EditLayoutButton.Text = "Edit";

            EditLayoutButton.Clicked += (s, e) =>
            {
                _isEditMode = !_isEditMode;
                if (_isEditMode)
                {
                    EditLayoutButton.Text = "Done";
                    if (_selectedFrame != null)
                    {
                        StepperPanel.IsVisible = true;
                        var rect = AbsoluteLayout.GetLayoutBounds(_selectedFrame);
                        WidthStepper.Value = rect.Width;
                        HeightStepper.Value = rect.Height;
                    }
                }
                else
                {
                    EditLayoutButton.Text = "Edit";
                    StepperPanel.IsVisible = false;
                    if (_selectedFrame is Frame frame)
                    {
                        frame.BorderColor = Colors.Transparent;
                    }
                    _selectedFrame = null;
                }
            };

            SaveLayoutButton.Clicked += async (s, e) =>
            {


                var dashboard = Application.Current.MainPage.Handler.MauiContext.Services.GetRequiredService<Dashboard>();
                await Navigation.PushAsync(dashboard);
                //_isEditMode = false;
                //EditLayoutButton.Text = "Edit";
                //StepperPanel.IsVisible = false;
                //if (_selectedFrame is Frame frame)
                //{
                //    frame.BorderColor = Colors.Transparent;
                //}
                //_selectedFrame = null;

                //try
                //{
                //    string name = await DisplayPromptAsync("Save Layout", "Enter a name for your layout:", initialValue: "MyLayout");
                //    if (string.IsNullOrWhiteSpace(name))
                //        return;

                //    foreach (var c in Path.GetInvalidFileNameChars())
                //        name = name.Replace(c, '_');

                //    var layout = new LayoutState
                //    {
                //        BarChart = AbsoluteLayout.GetLayoutBounds(BarChartFrame),
                //        LineChart = AbsoluteLayout.GetLayoutBounds(LineChartFrame),
                //        DataGrid = AbsoluteLayout.GetLayoutBounds(DataGridFrame)
                //    };

                //    var json = System.Text.Json.JsonSerializer.Serialize(layout);
                //    var fileName = $"CryptoLayout_{name}.json";
                //    var filePath = Path.Combine(_desktopPath, fileName);
                //    File.WriteAllText(filePath, json);
                //    vm.LoadLayoutFiles(_desktopPath);
                //}
                //catch (Exception ex)
                //{
                //    await DisplayAlert("Error", $"Failed to save layout: {ex.Message}", "OK");
                //}
            };

            vm.PropertyChanged += async (s, e) =>
            {
                if (e.PropertyName == nameof(vm.SelectedLayoutFile))
                {
                    if (!string.IsNullOrEmpty(vm.SelectedLayoutFile))
                    {
                        await LoadLayoutFromFileAsync(Path.Combine(_desktopPath, vm.SelectedLayoutFile));
                    }
                }
            };


            vm.LoadLayoutFiles(_desktopPath);

            AttachGestures(BarChartFrame);
            AttachGestures(LineChartFrame);
            AttachGestures(DataGridFrame);
        }
        private async Task LoadLayoutFromFileAsync(string filePath)
        {
            try
            {
                // Read the file without blocking the UI
                var json = await File.ReadAllTextAsync(filePath);
                var layout = JsonSerializer.Deserialize<LayoutState>(json);

                if (layout != null)
                {
                    // Update UI on the main thread
                    AbsoluteLayout.SetLayoutBounds(BarChartFrame, layout.BarChart);
                    AbsoluteLayout.SetLayoutBounds(LineChartFrame, layout.LineChart);
                    AbsoluteLayout.SetLayoutBounds(DataGridFrame, layout.DataGrid);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load layout: {ex.Message}", "OK");
            }
        }

        private void AttachGestures(Frame frame)
        {
            var pan = new PanGestureRecognizer();
            pan.PanUpdated += (s, e) =>
            {
                if (!_isEditMode) return;

                switch (e.StatusType)
                {
                    case GestureStatus.Started:
                        _dragTarget = frame;
                        _dragStartX = e.TotalX;
                        _dragStartY = e.TotalY;
                        _originalBounds = AbsoluteLayout.GetLayoutBounds(frame);
                        break;
                    case GestureStatus.Running:
                        if (_dragTarget == frame)
                        {
                            double dx = e.TotalX - _dragStartX;
                            double dy = e.TotalY - _dragStartY;
                            var newRect = new Microsoft.Maui.Graphics.Rect(
                                _originalBounds.X + dx,
                                _originalBounds.Y + dy,
                                _originalBounds.Width,
                                _originalBounds.Height);
                            AbsoluteLayout.SetLayoutBounds(frame, newRect);
                        }
                        break;
                    case GestureStatus.Completed:
                    case GestureStatus.Canceled:
                        _dragTarget = null;
                        break;
                }
            };
            frame.GestureRecognizers.Add(pan);

            var tap = new TapGestureRecognizer();
            tap.Tapped += (s, e) =>
            {
                if (!_isEditMode) return;

                if (_selectedFrame is Frame oldFrame)
                {
                    oldFrame.BorderColor = Colors.Transparent;
                }

                _selectedFrame = frame;
                frame.BorderColor = Colors.Blue;

                var rect = AbsoluteLayout.GetLayoutBounds(frame);
                WidthStepper.Value = rect.Width;
                HeightStepper.Value = rect.Height;
                StepperPanel.IsVisible = true;
            };
            frame.GestureRecognizers.Add(tap);
        }

        private void OnWidthStepperChanged(object sender, ValueChangedEventArgs e)
        {
            if (_selectedFrame == null) return;
            var rect = AbsoluteLayout.GetLayoutBounds(_selectedFrame);
            AbsoluteLayout.SetLayoutBounds(_selectedFrame, new Microsoft.Maui.Graphics.Rect(rect.X, rect.Y, e.NewValue, rect.Height));
        }

        private void OnHeightStepperChanged(object sender, ValueChangedEventArgs e)
        {
            if (_selectedFrame == null) return;
            var rect = AbsoluteLayout.GetLayoutBounds(_selectedFrame);
            AbsoluteLayout.SetLayoutBounds(_selectedFrame, new Microsoft.Maui.Graphics.Rect(rect.X, rect.Y, rect.Width, e.NewValue));
        }

        private void LoadLayoutFromFile(string filePath)
        {
            try
            {
                var json = File.ReadAllText(filePath);
                var layout = System.Text.Json.JsonSerializer.Deserialize<LayoutState>(json);
                if (layout != null)
                {
                    AbsoluteLayout.SetLayoutBounds(BarChartFrame, layout.BarChart);
                    AbsoluteLayout.SetLayoutBounds(LineChartFrame, layout.LineChart);
                    AbsoluteLayout.SetLayoutBounds(DataGridFrame, layout.DataGrid);
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Failed to load layout: {ex.Message}", "OK");
            }
        }
    }
}
