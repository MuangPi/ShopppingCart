using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoopingCart.Models.Entity
{
    public class ProductModel
    {
        //public ProductModel(int productId, string productName, string productDescription, int price, string image)
        //{
        //    ProductId = productId;
        //    ProductName = productName;
        //    ProductDescription = productDescription;
        //    Price = price;
        //    Image = image;
        //}

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int Price { get; set; }
        public int Qty { get; set; }
        public string Image { get; set; }
    }
}