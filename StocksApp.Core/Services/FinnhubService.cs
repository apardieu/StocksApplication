using Microsoft.Extensions.Configuration;
using RepositoryContracts;
using ServiceContracts;
using System.Text.Json;

namespace Service
{
    public class FinnhubService : IFinnhubService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IFinnhubRepository _finnhubRepository;
        public FinnhubService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IFinnhubRepository finnhubRepository )
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _finnhubRepository = finnhubRepository;
        }
        public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
        {
            Dictionary<string, object>? responseDictionary = await _finnhubRepository.GetCompanyProfile(stockSymbol);
            return responseDictionary;
        }

        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
        {
            Dictionary<string,object>? responseDictionary = await _finnhubRepository.GetStockPriceQuote(stockSymbol);
            return responseDictionary;
        }

        public async Task<List<Dictionary<string, string>>?> GetStocks()
        {
            List<Dictionary<string,string>>? responseDictionary = await _finnhubRepository.GetStocks();
            return responseDictionary;
        }

        public async Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch)
        {
            Dictionary<string, object>? responseDictionary = await _finnhubRepository.SearchStocks(stockSymbolToSearch);
            return responseDictionary;
        }
    }
}
