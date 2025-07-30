# 📊 Dashboard-Builder (.NET MAUI)

**Dynamic KPI Dashboard** is a cross-platform application built with **.NET MAUI** that enables business executives to create, personalize, and visualize performance dashboards in real-time. It allows users to dynamically customize layout, configure chart widgets, and monitor key metrics across tablet and desktop platforms — without developer involvement.

---

## 🚀 Use Case

"As a **Business Executive**, I want a cross-platform dashboard app for my tablet and desktop where I can personally customize the layout and content so that I can track the KPIs that matter most to me, without needing a developer to make changes."

---

## 🤦 Problem It Solves

Executives previously relied on:

* Static PDF reports emailed weekly
* Rigid dashboards requiring dev team intervention for any update
* A long cycle for BI/Dev team requests

This solution offers:

* ⏳ **Real-time customization**
* ✅ **Drag-and-drop widgets**
* 🔄 **Live API-powered KPIs**
* 📝 **User-specific layouts**

---

## 🔧 Architecture Overview

```
.NET MAUI UI <--> ViewModel (MVVM) <--> JSON Config Parser
                                     |
                                     |--> REST API for Layout & KPI Data
                                     |--> CoinGecko API for Live Market Data
```

---

## 📊 Key Features

* ⚖️ **Drag-and-drop Dashboard Builder**
* 🔄 **Dynamic layout rendering from JSON**
* 🔄 **Resizable and configurable widgets**
* 🔹 Charts: line, bar, pie
* 🔹 Data grids: sortable, pageable
* 📈 Live data updates via HTTP APIs
* 🌐 Adaptive layout for tablets & desktops
* 📂 Configurable widget data sources
* 🔢 Built with **MVVM**, **DataTemplateSelector**, **XAML Converters**

---

## 💡 Technologies Used

| Layer       | Technology                     |
| ----------- | ------------------------------ |
| UI          | .NET MAUI, XAML, MVVM          |
| Data        | JSON Layout Configuration      |
| Networking  | .NET HttpClient, CoinGecko API |
| Backend     | ASP.NET Core Web API           |
| Charts/Grid | Syncfusion Charts & DataGrid   |

---

## 🛠️ Project Structure

```
DynamicDashboard/
├── ViewModels/
│   └── DashboardViewModel.cs
├── Views/
│   └── DashboardPage.xaml
├── Models/
│   └── WidgetConfig.cs
├── Services/
│   └── ApiService.cs
├── Controls/
│   └── ChartWidget.xaml
│   └── DataGridWidget.xaml
├── API/
│   └── DashboardController.cs
├── Data/
│   └── UserLayoutStore.cs
```

---

## 🚄 Getting Started

### Backend (ASP.NET Core Web API)

```bash
git clone https://github.com/your-org/dashboard-api.git
cd dashboard-api
dotnet run
```

* Ensure it serves layout configs and KPI data on `/api/layout/{userId}` and `/api/kpi?symbol=btc`

---

### MAUI Frontend Setup

```bash
git clone https://github.com/your-org/dynamic-kpi-dashboard.git
cd dynamic-kpi-dashboard
dotnet build

dotnet maui run -t:windows # or -t:android / -t:macos
```

---

## 🤖 JSON Layout Example

```json
[
  {
    "type": "bar_chart",
    "position": { "row": 0, "column": 0 },
    "size": { "rows": 2, "columns": 3 },
    "dataSource": "https://api.coingecko.com/api/v3/coins/bitcoin/market_chart?vs_currency=usd&days=30"
  },
  {
    "type": "data_grid",
    "position": { "row": 2, "column": 0 },
    "size": { "rows": 2, "columns": 3 },
    "dataSource": "/api/kpi/sales"
  }
]
```

---

## 🌐 External API Integration

* **CoinGecko API** — Cryptocurrency price trends, market data
* **Open endpoints**: no API key required for most requests

---

## 🙋 User Experience

* Dashboard loads per user from cloud-based config
* Widgets fetch live data as per selected type
* Edit Mode enables: move, resize, swap, and reconfigure
* Changes are stored via backend `POST /api/layout/{userId}`

---

## 📚 License

MIT License — Free to use and extend

---

## 💬 Contact

* Created by: \[Narayana Chittala]
* Email: \[[narayanachittala007@gmail.com](mailto:narayanachittala007@gmail.com)]

