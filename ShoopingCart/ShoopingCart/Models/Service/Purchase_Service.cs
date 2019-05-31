using ShoopingCart.Models.Entity;
using ShoopingCart.Models.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ShoopingCart.Models.Service
{
    public class Purchase_Service
    {
        private Purchase_Repository purchase_Repository;

        public Purchase_Service()
        {
            purchase_Repository = new Purchase_Repository(ConfigurationManager.ConnectionStrings["ShoopingCartConnection"].ToString());
        }

        public bool Insert_Purchase_Products_To_Db(List<ProductModel> productlist, int user_id)
        {
            bool insert_succesful = purchase_Repository.Insert_Purchase_Products_To_Db(productlist, user_id);

            return insert_succesful;
        }

        public List<PurchaseProductModel> GetPurchasedProductbyID(int userId)
        {
            List<PurchaseProductModel> product = new List<PurchaseProductModel>();
            product = this.purchase_Repository.GetAllPurchaseProductbyUserID(userId);
            return product;
        }
    }
}