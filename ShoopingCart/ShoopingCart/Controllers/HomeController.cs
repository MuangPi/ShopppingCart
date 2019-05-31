using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using ShoopingCart.Models;
using ShoopingCart.Models.Service;
using ShoopingCart.Models.Entity;
using System.IO;
using System.Data;


namespace ShoopingCart.Controllers
{
    public class HomeController : Controller
    {
        int qty;
        static ProductList myCart;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        string con = "Server=DESKTOP-KC6R8N3;" + "Database=Test;" +
                                        "Integrated Security=true";
        public ActionResult FileUpload(ProductModel productModel, HttpPostedFileBase file)
        {
            if (file != null)
            {
                string pic = System.IO.Path.GetFileName(file.FileName);
                string no_extenstion = Path.GetFileNameWithoutExtension(pic);
                string date = DateTime.Now.ToString("yyyyMMddHHmmss");
                string image = pic.Replace(no_extenstion, date);

                string path = System.IO.Path.Combine(
                                       Server.MapPath("~/app_Data"), image);
                // file is uploaded
                file.SaveAs(path);


                using (SqlConnection sqlcon = new SqlConnection(con))
                {
                    try
                    {
                        sqlcon.Open();
                        string q = @"INSERT INTO Product VALUES(@product_name,@product_Description,
                        @price,@image)";
                        SqlCommand cmd = new SqlCommand(q, sqlcon);
                        cmd.Parameters.AddWithValue("@product_name", productModel.ProductName);
                        cmd.Parameters.AddWithValue("@product_description", productModel.ProductDescription);
                        cmd.Parameters.AddWithValue("@price", productModel.Price);
                        cmd.Parameters.AddWithValue("@image", date);
                        cmd.ExecuteNonQuery();

                    }
                    catch (SqlException e)
                    {
                        Debug.WriteLine(e.ToString());
                    }
                }
            }
            return View();
        }

        public ActionResult BindData()

        {
            myCart = (ProductList)Session["cart"];
            using (SqlConnection sqlcon = new SqlConnection(con))
                try
                {
                    sqlcon.Open();
                    string query = "SELECT * from Product";
                    SqlCommand cmd = new SqlCommand(query, sqlcon);
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<ProductModel> products = new List<ProductModel>();
                    while (reader.Read())
                    {
                        

                        ProductModel productModel = new ProductModel()
                        {
                            ProductId = (int)reader["id"],
                            ProductName = (string)reader["product_name"],
                            ProductDescription = (string)reader["product_Description"],
                            Price = (int)reader["price"],
                            Qty = 1,
                            Image = "~/App_Data/ "+ (string)reader["image"] + ".jpg"
                           
                        };
                        // var img = Server.MapPath("/App_Data/"+ productModel.Image + ".jpg");
                        string temp = myCart.AddToCart(productModel);

                        Session["cart"] = myCart;
                     //   products.Add(productModel);
                      
                       
                        
                    }                 
                   
                    ViewData["sendData"] = myCart.GetCartItems();

                   
                    ViewData["total_price"] = this.Get_Total_Price();
                }

                catch (SqlException e)
                {
                    Debug.WriteLine(e.ToString());
                }
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
                    product.Qty = p.Qty + 1;

                    myCart.UpdateCart(product);
                }
            }

            ViewData["sendData"] = myCart.GetCartItems();
            ViewData["total_price"] = this.Get_Total_Price();

            return View("BindData");
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

            return View("BindData");
        }
        public ActionResult Bo()
        {
            using (SqlConnection sqlcon = new SqlConnection(con))
                try
                {
                    sqlcon.Open();
                    string query = @"SELECT * from Product";
                    SqlCommand cmd = new SqlCommand(query, sqlcon);
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<ProductModel> products = new List<ProductModel>();
                    while (reader.Read())
                    {
                        //ProductModel productModel = new ProductModel()
                        //{
                        //    ProductId = (int)reader["id"],
                        //    ProductName = (string)reader["product_name"],
                        //    ProductDescription = (string)reader["product_Description"],
                        //    Price = (float)reader["price"],
                        //    Image = (string)reader["image"]
                        //};
                        //products.Add(productModel);
                    }
                }

                catch (SqlException e)
                {
                    Debug.WriteLine(e.ToString());
                }
            return View();
        }
    }
}