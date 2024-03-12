using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    /// <summary>
    /// Domain Model for SellOrder
    /// </summary>
    public class SellOrder
    {
        [Key]
        public Guid SellOrderID { get; set; }

        [Required(ErrorMessage = "StockSymbol can't be blank")]
        [StringLength(10)]
        public string StockSymbol { get; set; }
        [Required(ErrorMessage = "StockName can't be blank")]
        [StringLength(50)]
        public string StockName { get; set; }
        public DateTime DateAndTimeOfOrder { get; set; }
        [Range(minimum: 1, maximum: 100000, ErrorMessage = "Quantity must be between 1 and 100000")]
        public uint Quantity { get; set; }
        [Range(minimum: 1, maximum: 10000, ErrorMessage = "Price must be between 1 and 10000")]
        public double Price { get; set; }
    }
}
