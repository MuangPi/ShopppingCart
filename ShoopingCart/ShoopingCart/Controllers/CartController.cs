using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShoopingCart.Models.Entity;
using ShoopingCart.Models.Service;

namespace ShoopingCart.Controllers
{
    public class CartController : Controller
    {
        static ProductList myCart;
        // GET: Cart
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewCart()
        {
            myCart = (ProductList)Session["cart"];
            ViewData["sendData"] = myCart.GetCartItems();
            ViewData["total_price"] = this.Get_Total_Price();

            return View();
        }

        public int Get_Total_Price()
        {
            List<ProductModel> total_products = myCart.GetCartItems();

            int total_price = 0;
            foreach (ProductModel product in total_products)
            {
                total_price += product.Price * product.Qty;
            }

            return total_price;
        }

        public ActionResult IncreaseQuantity(int id)
        {

            List<ProductModel> prod = myCart.GetCartItems();

            foreach (ProductModel p in prod)
            {
                if (p.ProductId == id)
                {
                    ProductModel product = new ProductModel();
                    product.ProductId = p.ProductId;
                    product.ProductName = p.ProductName;
                    product.ProductDescription = p.ProductDescription;
                    product.Price = p.Price;
                    product.Image = p.Image;
                    product.Qty = p.Qty + 1;

                    myCart.UpdateCart(product);
                }
            }

            ViewData["sendData"] = myCart.GetCartItems();
            ViewData["total_price"] = this.Get_Total_Price();

            return RedirectToAction("ViewCart");
        }

        public ActionResult DecreaseQuantity(int id)
        {
            List<ProductModel> prod = myCart.GetCartItems();

            foreach (ProductModel p in prod)
            {
                if (p.ProductId == id)
                {
                    ProductModel product = new ProductModel();
                    product.ProductId = p.ProductId;
                    product.ProductName = p.ProductName;
                    product.ProductDescription = p.ProductDescription;
                    product.Price = p.Price;

                    if (p.Qty > 0)
                    {
                        product.Qty = p.Qty - 1;
                    }


                    myCart.UpdateCart(product);
                }
            }

            ViewData["sendData"] = myCart.GetCartItems();
            ViewData["total_price"] = this.Get_Total_Price();

            return RedirectToAction("ViewCart");
        }
    }
}