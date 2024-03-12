using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Rotativa.AspNetCore;
using ServiceContracts;
using ServiceContracts.DTO;
using StocksApp.Filters.ActionFilter;
using StocksApp.Models;

namespace StocksApp.Controllers
{
    [Route("[controller]")]
    public class TradeController : Controller
    {
        private readonly IFinnhubService _finnhubService;
        private readonly IStocksService _stocksService;
        private readonly IOptions<TradingOptions> _tradingOptions;
        private readonly IConfiguration _configuration;

        public TradeController(IFinnhubService finnhubService, IOptions<TradingOptions> tradingOptions, IConfiguration configuration, IStocksService stocksService)
        {
            _finnhubService = finnhubService;
            _tradingOptions = tradingOptions;
            _configuration = configuration;
            _stocksService = stocksService;
        }


        [Route("[action]")]
        [Route("~/[controller]")]
        public async Task<IActionResult> Index()
        {
            if(_tradingOptions.Value.DefaultStockSymbol == null)
            {
                _tradingOptions.Value.DefaultStockSymbol = "MSFT";
            }


            Dictionary<string,object>? stockPriceDictionnary = await _finnhubService.GetStockPriceQuote(_tradingOptions.Value.DefaultStockSymbol);
            Dictionary<string, object>? companyProfileDictionnary = await _finnhubService.GetCompanyProfile(_tradingOptions.Value.DefaultStockSymbol);

            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            StockTrade stockTrade = new StockTrade
            {
                StockSymbol = Convert.ToString(companyProfileDictionnary["ticker"]),
                Price = Convert.ToDouble(stockPriceDictionnary["c"].ToString()),
                StockName = Convert.ToString(companyProfileDictionnary["name"])
            };

            ViewBag.FinnhubToken = _configuration["FinnhubToken"];

            return View(stockTrade);
        }


        [Route("[action]/{stockSymbol}")]
        [Route("~/[controller]/{stockSymbol}")]
        public async Task<IActionResult> Index(string stockSymbol)        
        {
            if (string.IsNullOrEmpty(stockSymbol))
                stockSymbol = "MSFT";

            Dictionary<string, object>? stockPriceDictionnary = await _finnhubService.GetStockPriceQuote(stockSymbol);
            Dictionary<string, object>? companyProfileDictionnary = await _finnhubService.GetCompanyProfile(stockSymbol);

            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            StockTrade stockTrade = new StockTrade
            {
                StockSymbol = Convert.ToString(companyProfileDictionnary["ticker"]),
                Price = Convert.ToDouble(stockPriceDictionnary["c"].ToString()),
                StockName = Convert.ToString(companyProfileDictionnary["name"])
            };

            ViewBag.FinnhubToken = _configuration["FinnhubToken"];

            return View(stockTrade);
        }

        [Route("[action]")]
        public async Task<IActionResult> Orders()
        {
            List<BuyOrderResponse> buyOrders = await _stocksService.GetBuyOrders();
            List<SellOrderResponse> sellOrders = await _stocksService.GetSellOrders();
            
            Orders orders = new Orders() { BuyOrders = buyOrders, SellOrders = sellOrders };

            ViewBag.TradingOptions = _tradingOptions;

            return View(orders);

        }

        [Route("[action]")]
        [HttpPost]
        [TypeFilter(typeof(CreateOrderActionFilter))]
        public async Task<IActionResult> BuyOrder(BuyOrderRequest orderRequest)
        {
                BuyOrderResponse sellOrderResponse = await _stocksService.CreateBuyOrder(orderRequest);
                return RedirectToAction("Orders", "Trade");
        }

        [Route("[action]")]
        [HttpPost]
        [TypeFilter(typeof(CreateOrderActionFilter))]
        public async Task<IActionResult> SellOrder(SellOrderRequest orderRequest)
        {
                SellOrderResponse sellOrderResponse = await _stocksService.CreateSellOrder(orderRequest);
                return RedirectToAction("Orders", "Trade");
        }

        [Route("[action]")]
        public async Task<IActionResult> StockMarketPDF()
        {

            List<IOrderResponse> orders = new List<IOrderResponse>();
            orders.AddRange(await _stocksService.GetBuyOrders());
            orders.AddRange(await _stocksService.GetSellOrders());

            orders = orders.OrderByDescending(temp => temp.DateAndTimeOfOrder).ToList();

            return new ViewAsPdf("StockMarketPDF", orders, ViewData)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins() { Top = 20, Bottom = 20, Left = 20, Right = 20 },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };
        }
    }
}
