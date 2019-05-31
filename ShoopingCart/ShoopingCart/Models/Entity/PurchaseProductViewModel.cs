using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoopingCart.Models.Entity
{
    public class PurchaseProductViewModel
    {
        public int PurchaseId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int ProductId { get; set; }
        public string ProductDescription { get; set; }
        public int Price { get; set; }
        public int Qty { get; set; }
        public string Image { get; set; }

        public List<string> ActivationCode { get; set; }
    }
}