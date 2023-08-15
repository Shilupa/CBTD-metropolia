using System;
using DataAccess.Models;
using System.ComponentModel.DataAnnotations;

namespace CBTD.ViewModel
{
    public class ShoppingCartVM
    {
        public Product Product { get; set; }
        [Range(1, 1000, ErrorMessage = "Must be between 1 and 1000")]
        public int Count { get; set; }
        public IEnumerable<ShoppingCart> cartItems { get; set; }

        public double CartTotal { get; set; }

        public OrderHeader OrderHeader { get; set; }

    }
}


