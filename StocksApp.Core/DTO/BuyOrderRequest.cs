using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// DTO for buying a stock
    /// </summary>
    public class BuyOrderRequest : IValidatableObject, IOrderRequest
    {
        [Required(ErrorMessage = "StockSymbol can't be blank")]
        public string? StockSymbol { get; set; }

        [Required(ErrorMessage = "StockName can't be blank")]
        public string? StockName { get; set; }

        public DateTime DateAndTimeOfOrder { get; set; }

        [Range(1, 100000)]
        public uint Quantity { get; set; }
        [Range(1,10000)]
        public double Price { get; set; }

        public override bool Equals(object? obj)
        {
            BuyOrderRequest? other = obj as BuyOrderRequest;
            if (other == null) return false;

            return
                string.Equals(StockSymbol, other.StockSymbol, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(StockName, other.StockName, StringComparison.OrdinalIgnoreCase) &&
                DateTime.Equals(DateAndTimeOfOrder,other.DateAndTimeOfOrder) &&
                Quantity == other.Quantity &&
                Price == other.Price;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public BuyOrder ToBuyOrder()
        {
            return new BuyOrder()
            {
                StockSymbol = StockSymbol,
                StockName = StockName,
                Price = Price,
                Quantity = Quantity,
                DateAndTimeOfOrder = DateAndTimeOfOrder
            };

        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            //Date of order should be less than Jan 01, 2000
            if (DateAndTimeOfOrder < Convert.ToDateTime("2000-01-01"))
            {
                results.Add(new ValidationResult("Date of the order should not be older than Jan 01, 2000."));
            }

            return results;
        }




    }
}
