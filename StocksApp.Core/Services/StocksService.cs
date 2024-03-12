using Entities;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class StocksService : IStocksService
    {

        private readonly IStocksRepository _stocksRepository;

        public StocksService(IStocksRepository stocksRepository)
        {
            _stocksRepository = stocksRepository;
        }
        public async Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest buyRequest)
        {
            if(buyRequest == null) throw new ArgumentNullException(nameof(buyRequest));

            ValidationHelper.ModelValidation(buyRequest);

            BuyOrder buyOrder = buyRequest.ToBuyOrder();
            buyOrder.BuyOrderID = Guid.NewGuid();

            BuyOrder buyOrder_from_db = await _stocksRepository.CreateBuyOrder(buyOrder);

            return buyOrder_from_db.ToBuyOrderResponse();

        }

        public async Task<SellOrderResponse> CreateSellOrder(SellOrderRequest sellOrderRequest)
        {
            if (sellOrderRequest == null) throw new ArgumentNullException(nameof(sellOrderRequest));

            ValidationHelper.ModelValidation(sellOrderRequest);

            SellOrder sellOrder = sellOrderRequest.ToSellOrder();
            sellOrder.SellOrderID = Guid.NewGuid();
            //_sellOrders.Add(sellOrder);
            SellOrder sellOrder_from_db = await _stocksRepository.CreateSellOrder(sellOrder);

            return sellOrder_from_db.ToSellOrderResponse();
        }

        public async Task<List<BuyOrderResponse>> GetBuyOrders()
        {

            List<BuyOrder> buyOrders = await _stocksRepository.GetBuyOrders();
            return buyOrders.Select(temp=>temp.ToBuyOrderResponse()).ToList();
        }

        public async Task<List<SellOrderResponse>> GetSellOrders()
        {
            List<SellOrder> sellOrders = await _stocksRepository.GetSellOrders();
            return sellOrders.Select(temp => temp.ToSellOrderResponse()).ToList();
        }
    }
}
