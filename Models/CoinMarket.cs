using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GamanaDashBoardApp.Models
{
    public class CoinMarket
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }
        [JsonPropertyName("image")]
        public string Image { get; set; }
        [JsonPropertyName("current_price")]
        public double CurrentPrice { get; set; }
        [JsonPropertyName("market_cap")]
        public double MarketCap { get; set; }
        [JsonPropertyName("price_change_percentage_24h")]
        public double PriceChangePercentage24h { get; set; }
    }
    public class MarketChartResponse
    {
        [JsonPropertyName("prices")]
        public List<List<double>> prices { get; set; }
    }
    public class PricePoint
    {
        public string Time { get; set; }
        public double Price { get; set; }
    }

    public class LayoutState
    {
        [JsonConverter(typeof(RectJsonConverter))]
        public Rect BarChart { get; set; }

        [JsonConverter(typeof(RectJsonConverter))]
        public Rect LineChart { get; set; }

        [JsonConverter(typeof(RectJsonConverter))]
        public Rect DataGrid { get; set; }
    }
}
