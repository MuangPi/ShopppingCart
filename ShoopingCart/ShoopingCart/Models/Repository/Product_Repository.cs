using ShoopingCart.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace ShoopingCart.Models.Repository
{
    public class Product_Repository
    {
        //string con = "Server=(local);" + "Database=ShopCart;" +
        //                       "Integrated Security=true";

        string DBConnectionString;
        public Product_Repository(string ConnectionString)
        {

            DBConnectionString = ConnectionString;
        }

        public List<ProductModel> GetAllProduct()
        {
            List<ProductModel> products = new List<ProductModel>();

            using (SqlConnection sqlcon = new SqlConnection(DBConnectionString))
                try
                {

                    string query = "SELECT * from Product";
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = sqlcon;
                    cmd.CommandText = query;

                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, DBConnectionString);

                    sqlcon.Open();

                    cmd.ExecuteNonQuery();

                    SqlDataReader reader = cmd.ExecuteReader();


                    // I am Muang PI.I am stupid.

                    while (reader.Read())
                    {


                        ProductModel productModel = new ProductModel()
                        {
                            ProductId = Convert.ToInt32(reader["ProductID"]),
                            ProductName = (string)reader["ProductName"],
                            ProductDescription = reader["ProductDescription"].ToString(),
                            Price = Convert.ToInt32(reader["ProductPrice"]),
                            Qty = 1,
                            Image = "../Content/Images/" + reader["ImageID"].ToString()

                        };

                        products.Add(productModel);

                    }

                }

                catch (SqlException e)
                {
                    Debug.WriteLine(e.ToString());
                }
            return products;
        }


        public ProductModel GetProductbyID(int product_id)
        {
            ProductModel product = new ProductModel();

            using (SqlConnection sqlcon = new SqlConnection(DBConnectionString))
                try
                {
                    string q = @"select * from product where ProductID = @product_id;";
                    SqlCommand cmd = new SqlCommand(q, sqlcon);
                    cmd.Parameters.AddWithValue("@product_id", product_id);
                    sqlcon.Open();
                    cmd.ExecuteNonQuery();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {


                        product.ProductId = Convert.ToInt32(reader["ProductID"]);
                        // ProductName = (string)reader["product_name"],
                        product.ProductDescription = reader["ProductDescription"].ToString();
                        product.Price = Convert.ToInt32(reader["ProductPrice"]);
                        product.Qty = 1;
                        product.Image = "../Content/Images/" + reader["ImageID"].ToString();



                    }

                }

                catch (SqlException e)
                {
                    Debug.WriteLine(e.ToString());
                }
            return product;
        }
    }
}