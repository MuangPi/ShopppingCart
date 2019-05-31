using ShoopingCart.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoopingCart.Models.Service
{
    public class ProductList
    {

        public List<ProductModel> Product_List { get; set; }

        // Messages
        public const string AddToCartNG = "Failed to add item to cart, please try again.";
        public const string AddToCartOK = "Added item to cart.";
        public ProductList()
        {
            Product_List = new List<ProductModel>();
        }

        // Service Methods
        public string AddToCart(ProductModel itm)
        {
            try
            {
                if (itm != null)
                {
                    bool IsExists = false;
                    foreach (var i in Product_List)
                    {
                        if (i.ProductId == itm.ProductId)
                        { i.Qty += itm.Qty; IsExists = true; break; }
                    }
                    if (!IsExists) Product_List.Add(itm);
                    return AddToCartOK;
                }
                else
                    return AddToCartNG;
            }
            catch (Exception e)
            {
                return AddToCartNG;
            }
        }
        public List<ProductModel> GetCartItems()
        {
            try
            {
                if (Product_List.Count != 0)
                {
                    return Product_List;
                }
                else
                    return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public void UpdateCart(ProductModel itm)
        {
            foreach (ProductModel i in Product_List)
            {
                if (i.ProductId == itm.ProductId)
                {
                    i.ProductId = itm.ProductId;
                    i.Qty = itm.Qty;
                    i.Price = itm.Price;
                    break;
                }
            }
        }
        public void RemoveFromCart(int ProductID)
        {
            foreach (ProductModel i in Product_List)
            {
                if (i.ProductId == ProductID)
                {
                    Product_List.Remove(i);
                    break;
                }
            }
        }
    }
}