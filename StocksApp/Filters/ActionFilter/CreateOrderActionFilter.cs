using Microsoft.AspNetCore.Mvc.Filters;
using ServiceContracts.DTO;
using StocksApp.Controllers;
using StocksApp.Models;

namespace StocksApp.Filters.ActionFilter
{
    public class CreateOrderActionFilter : IAsyncActionFilter
    {
        public CreateOrderActionFilter() { }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.Controller is TradeController)
            {
                IOrderRequest? orderRequest = context.ActionArguments["orderRequest"] as IOrderRequest;
                if (orderRequest != null)
                {
                    TradeController tradeController = (TradeController)context.Controller;
                    tradeController.ModelState.Clear();
                    orderRequest.DateAndTimeOfOrder = DateTime.Now;
                    tradeController.TryValidateModel(orderRequest);

                    if (!tradeController.ModelState.IsValid)
                    {
                        tradeController.ViewBag.Errors = tradeController.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                        StockTrade stockTrade = new StockTrade() { Price = orderRequest.Price, Quantity = orderRequest.Quantity, StockName = orderRequest.StockName, StockSymbol = orderRequest.StockSymbol };
                        context.Result = tradeController.View("Index", stockTrade);
                    }
                }
                await next();
            }
        }
    }
}
