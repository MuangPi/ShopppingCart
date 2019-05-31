using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShoopingCart.Models;
using ShoopingCart.Models.Entity;
using ShoopingCart.Models.Service;

namespace ShoopingCart.Controllers
{
    public class ProductController : Controller
    {
        static ProductList myCart;
        private Produt_Service product_Service;

        public ProductController()
        {
            product_Service = new Produt_Service();
        }
        // GET: Product
        public ActionResult Index(string productkeyword)
        {
            myCart = (ProductList)Session["cart"];

            Customer customer = new Customer();

            customer.UserName = "Megan";

            ViewData["Customer"] = customer;

            List<ProductModel> products = new List<ProductModel>();
            products = this.product_Service.GetAllProduct();

            if (productkeyword != null && productkeyword != "")
            {
                products = products.Where(p => p.ProductName.ToLower().Contains(productkeyword.ToLower()) || p.ProductDescription.ToLower().Contains(productkeyword.ToLower())).ToList();
                ViewData.Clear();
            }

            if (products.Count > 0)
            {
                ViewData["sendData"] = products;
                


                List<ProductModel> total_products = new List<ProductModel>();
                total_products = myCart.GetCartItems();

                int total_qty = 0;
                if (total_products != null)
                {
                    if (total_products.Count > 0)
                    {
                        foreach (ProductModel product in total_products)
                        {
                            total_qty += product.Qty;
                        }

                        ViewData["total_quantity"] = total_qty;
                    }
                }
               
                else
                {
                    ViewData["total_quantity"] = total_qty;
                }

                if (Request.IsAjaxRequest())
                {
                    return PartialView("_ProductList", products);
                }
            }
            return View();
        }


        public ActionResult AddToCart(int id)
        {
            ProductModel product = new ProductModel();
            product = this.product_Service.GetProductbyID(id);

            if (product != null)
            {
                //ViewData["sendData"] = product;
                myCart = (ProductList)Session["cart"];

                    string temp = myCart.AddToCart(product);
                    Session["cart"] = myCart;
                
            }
            return RedirectToAction("Index");
        }
    }
}