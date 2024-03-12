using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
    public interface IStocksService
    {
        /// <summary>
        /// Create a new buy order
        /// </summary>
        /// <param name="buyRequest">Buy details</param>
        /// <returns>Return the same buy request details along a generated ID</returns>
        Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest buyRequest);

        /// <summary>
        /// Get all processed buy orders
        /// </summary>
        /// <returns>All processed buy orders</returns>
        Task<List<BuyOrderResponse>> GetBuyOrders();

        /// <summary>
        /// Create a new sell order
        /// </summary>
        /// <param name="sellOrderRequest">Sell details</param>
        /// <returns>Return the same sell request details along a generated ID</returns>
        Task<SellOrderResponse> CreateSellOrder(SellOrderRequest sellOrderRequest);
    
        /// <summary>
        /// Get all processed sell orders
        /// </summary>
        /// <returns>All processed sell orders</returns>
        Task<List<SellOrderResponse>> GetSellOrders();
    
    
    }
}
