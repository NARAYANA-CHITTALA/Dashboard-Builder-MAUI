using GamanaDashBoardApp.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;

public class MainPageViewModel : INotifyPropertyChanged
{
    public ObservableCollection<CoinMarket> GridData { get; set; } = new();
    public ObservableCollection<CoinMarket> BarChartData { get; set; } = new();
    public ObservableCollection<PricePoint> LineChartData { get; set; } = new();
    private bool _isLoading;
    public bool IsLoading { get => _isLoading; set { _isLoading = value; OnPropertyChanged(); } }
    private string _errorMessage;
    public string ErrorMessage { get => _errorMessage; set { _errorMessage = value; OnPropertyChanged(); } }
    public ObservableCollection<string> LayoutFiles { get; set; } = new();
    private string _selectedLayoutFile;
    public string SelectedLayoutFile
    {
        get => _selectedLayoutFile;
        set
        {
            if (_selectedLayoutFile != value)
            {
                _selectedLayoutFile = value;
                OnPropertyChanged();
            }
        }
    }
    public MainPageViewModel()
    {
        LoadDataAsync();
    }
    public async void LoadDataAsync()
    {
        IsLoading = true;
        try
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("x-cg-demo-api-key", "CG-TQ15VokK6D3ApEVrzpZJdu7t");
            var url = "https://api.coingecko.com/api/v3/coins/markets?vs_currency=usd&order=market_cap_desc&per_page=10&page=1&sparkline=false";
            var coins = await client.GetFromJsonAsync<List<CoinMarket>>(url);

            GridData.Clear();
            BarChartData.Clear();
            if (coins != null)
            {
                foreach (var coin in coins)
                {
                    GridData.Add(coin);
                    BarChartData.Add(coin);
                }

                if (coins.Count > 0)
                {
                    var coinId = coins[0].Id;
                    var chartUrl = $"https://api.coingecko.com/api/v3/coins/{coinId}/market_chart?vs_currency=usd&days=7";
                    var chartResp = await client.GetFromJsonAsync<MarketChartResponse>(chartUrl);

                    LineChartData.Clear();
                    if (chartResp?.prices != null)
                    {
                        foreach (var pt in chartResp.prices)
                        {
                            LineChartData.Add(new PricePoint { Time = DateTimeOffset.FromUnixTimeMilliseconds((long)pt[0]).DateTime.ToString("MM-dd"), Price = pt[1] });
                        }
                    }
                }
            }
            ErrorMessage = string.Empty;
        }
        catch (Exception ex)
        {
            ErrorMessage = "Failed to load data: " + ex.Message;
        }
        finally
        {
            IsLoading = false;
        }
    }
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    public void LoadLayoutFiles(string desktopPath)
    {
        try
        {
            var files = Directory.GetFiles(desktopPath, "CryptoLayout_*.json");
            LayoutFiles.Clear();
            foreach (var file in files)
            {
                LayoutFiles.Add(Path.GetFileName(file));
            }
        }
        catch (Exception ex)
        {
            // Optionally show error
        }
    }
}