using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ServiceContracts.DTO.IOrderResponse;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// DTO class used as return type for buy in StockService
    /// </summary>
    public class BuyOrderResponse : IOrderResponse
    {
        public Guid BuyOrderID { get; set; }

        public string StockSymbol { get; set; }

        public string StockName { get; set; }

        public DateTime DateAndTimeOfOrder { get; set; }

        public uint Quantity { get; set; }

        public double Price { get; set; }

        public OrderType TypeOfOrder => OrderType.BuyOrder;
        public double TradeAmount { get; set; }


        public override bool Equals(object? obj)
        {
            BuyOrderResponse? buyOrderResponse = obj as BuyOrderResponse;
            if(buyOrderResponse == null)
                return false;

            return
                Guid.Equals(buyOrderResponse.BuyOrderID, BuyOrderID) &&
                string.Equals(buyOrderResponse.StockName, StockName, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(buyOrderResponse.StockSymbol, StockSymbol, StringComparison.OrdinalIgnoreCase) &&
                DateTime.Equals(buyOrderResponse.DateAndTimeOfOrder, DateAndTimeOfOrder) &&
                buyOrderResponse.Quantity == Quantity &&
                buyOrderResponse.Price == Price &&
                buyOrderResponse.TradeAmount == TradeAmount;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }


    public static class BuyOrderExtensions
    {

        /// <summary>
        /// An extension method to convert an object of BuyOrder class to BuyOrderResponse
        /// </summary>
        /// <param name="buyOrder">BuyOrder object to be returned as BuyOrderResposne</param>
        /// <returns>Returns converted BuyOrderResponse</returns>
        public static BuyOrderResponse ToBuyOrderResponse(this BuyOrder buyOrder)
        {
            return new BuyOrderResponse
            {
                BuyOrderID = buyOrder.BuyOrderID,
                StockSymbol = buyOrder.StockSymbol,
                StockName = buyOrder.StockName,
                Price = buyOrder.Price,
                Quantity = buyOrder.Quantity,
                DateAndTimeOfOrder = buyOrder.DateAndTimeOfOrder,
                TradeAmount = buyOrder.Price * buyOrder.Quantity

            };
        }

    }
}
