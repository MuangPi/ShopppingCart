using ShoopingCart.Models.Entity;
using ShoopingCart.Models.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ShoopingCart.Models.Service
{

    public class Produt_Service
    {
        private Product_Repository product_Repository;

        public Produt_Service()
        {
            product_Repository = new Product_Repository(ConfigurationManager.ConnectionStrings["ShoopingCartConnection"].ToString());
        }


        public List<ProductModel> GetAllProduct()
        {
            List<ProductModel> products = new List<ProductModel>();
            products = this.product_Repository.GetAllProduct();
            return products;
        }

        public ProductModel GetProductbyID(int product_id)
        {
            ProductModel product = new ProductModel();
            product = this.product_Repository.GetProductbyID(product_id);
            return product;
        }
    }
}