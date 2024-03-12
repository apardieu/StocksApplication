using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceContracts;

namespace StocksApp.ViewComponents
{
    public class SelectedStockViewComponent : ViewComponent
    {
        private readonly IOptions<TradingOptions> _options;
        private readonly IStocksService _stocksService;
        private readonly IFinnhubService _finnhubService;
        private readonly IConfiguration _configuration;

        public SelectedStockViewComponent(IOptions<TradingOptions> options, IStocksService stocksService, IFinnhubService finnhubService, IConfiguration configuration)
        {
            _options = options;
            _stocksService = stocksService;
            _finnhubService = finnhubService;
            _configuration = configuration;
        }

        public async Task<IViewComponentResult> InvokeAsync(string? stockSymbol)
        {
            Dictionary<string, object>? companyProfileDictionary = null;
            if (stockSymbol != null)
            {
                companyProfileDictionary = await _finnhubService.GetCompanyProfile(stockSymbol);
                var stockPriceDict = await _finnhubService.GetStockPriceQuote(stockSymbol);

                if (stockPriceDict != null && companyProfileDictionary != null)
                {
                    companyProfileDictionary.Add("price", stockPriceDict["c"]);
                }
            }
            if (companyProfileDictionary != null && companyProfileDictionary.ContainsKey("logo"))
                return View("Default",companyProfileDictionary);
            else
                return Content("");
        }
    }
}
