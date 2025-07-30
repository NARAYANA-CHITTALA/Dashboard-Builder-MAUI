using GamanaDashboard.Models;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http.Json;

namespace GamanaDashBoardApp.Services
{
    public class DashboardService
    {
        private readonly HttpClient _client;
        private readonly ILogger<DashboardService> _logger;

        public DashboardService(HttpClient client, ILogger<DashboardService> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<DashboardConfig> GetDashboardConfig(string userId)
        {
            try
            {
                var response = await _client.GetAsync($"api/dashboard/{userId}");

                if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    _logger.LogWarning("Access forbidden to dashboard API");
                    return CreateDefaultDashboard(userId);
                }

                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<DashboardConfig>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching dashboard config");
                return CreateDefaultDashboard(userId);
            }
        }

        public async Task SaveDashboardConfig(string userId, DashboardConfig config)
        {
            try
            {
                var response = await _client.PostAsJsonAsync($"api/dashboard/{userId}", config);

                if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    _logger.LogWarning("Access forbidden when saving dashboard");
                    return;
                }

                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving dashboard config");
            }
        }

        public async Task<List<HistoricalPriceData>> GetHistoricalData(string coinId, string currency, int days)
        {
            try
            {
                var response = await _client.GetAsync(
                    $"api/market/historical?coinId={coinId}&currency={currency}&days={days}");

                if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    _logger.LogWarning("Access forbidden to historical data API");
                    return new List<HistoricalPriceData>();
                }

                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<HistoricalPriceData>>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching historical data");
                return new List<HistoricalPriceData>();
            }
        }

        public async Task<List<CoinMarketData>> GetMarketData(string[] coinIds, string currency)
        {
            try
            {
                var idsParam = string.Join("&ids=", coinIds);
                var response = await _client.GetAsync(
                    $"api/market/coins?ids={idsParam}&currency={currency}");

                if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    _logger.LogWarning("Access forbidden to market data API");
                    return new List<CoinMarketData>();
                }

                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<CoinMarketData>>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching market data");
                return new List<CoinMarketData>();
            }
        }

        private DashboardConfig CreateDefaultDashboard(string userId)
        {
            return new DashboardConfig
            {
                UserId = userId,
                GridRows = 2,
                GridCols = 2,
                Widgets = new List<WidgetConfig>()
            };
        }
    }
} 