using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// DTO class used as return type for sells in StockService
    /// </summary>
    public class SellOrderResponse : IOrderResponse
    {
        public Guid SellOrderID { get; set; }

        public string StockSymbol { get; set; }

        public string StockName { get; set; }

        public DateTime DateAndTimeOfOrder { get; set; }

        public uint Quantity { get; set; }

        public double Price { get; set; }

        public double TradeAmount { get; set; }

        public OrderType TypeOfOrder => OrderType.SellOrder;

        public override bool Equals(object? obj)
        {
            SellOrderResponse? sellOrderResponse = obj as SellOrderResponse;
            if (sellOrderResponse == null)
                return false;

            return
                Guid.Equals(sellOrderResponse.SellOrderID, SellOrderID) &&
                string.Equals(sellOrderResponse.StockName, StockName, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(sellOrderResponse.StockSymbol, StockSymbol, StringComparison.OrdinalIgnoreCase) &&
                DateTime.Equals(sellOrderResponse.DateAndTimeOfOrder, DateAndTimeOfOrder) &&
                sellOrderResponse.Quantity == Quantity &&
                sellOrderResponse.Price == Price &&
                sellOrderResponse.TradeAmount == TradeAmount;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public static class SellOrderExtensions
    {

        /// <summary>
        /// An extension method to convert an object of SellOrder class to SellOrderResponse
        /// </summary>
        /// <param name="sellOrder">SellOrder object to be returned as SellOrderResposne</param>
        /// <returns>Returns converted SellOrderResponse</returns>
        public static SellOrderResponse ToSellOrderResponse(this SellOrder sellOrder)
        {
            return new SellOrderResponse
            {
                SellOrderID = sellOrder.SellOrderID,
                StockSymbol = sellOrder.StockSymbol,
                StockName = sellOrder.StockName,
                Price = sellOrder.Price,
                Quantity = sellOrder.Quantity,
                DateAndTimeOfOrder = sellOrder.DateAndTimeOfOrder,
                TradeAmount = sellOrder.Price * sellOrder.Quantity

            };
        }
    }
}
