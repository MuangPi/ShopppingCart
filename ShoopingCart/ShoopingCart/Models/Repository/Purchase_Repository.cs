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
    public class Purchase_Repository
    {
        string DBConnectionString;

        public Purchase_Repository(string ConnectionString)
        {
            DBConnectionString = ConnectionString;
        }


        #region insert purchased product to DB
        public bool Insert_Purchase_Products_To_Db(List<ProductModel> productlist, int user_id)
        {
            bool inserted = false;

            int purchase_id = GeneratePurchaseID();

            //create order to purchase table
            using (SqlConnection sqlcon = new SqlConnection(DBConnectionString))
            {
                try
                {
                    sqlcon.Open();
                    string q = @"INSERT INTO Purchase VALUES(@purchase_id,@customer_id,
                        @purchase_date)";
                    SqlCommand cmd = new SqlCommand(q, sqlcon);
                    cmd.Parameters.AddWithValue("@purchase_id", purchase_id);
                    cmd.Parameters.AddWithValue("@customer_id", user_id);
                    cmd.Parameters.AddWithValue("@purchase_date", DateTime.Now);
                    cmd.ExecuteNonQuery();


                    inserted = true;
                    sqlcon.Close();
                }
                catch (SqlException e)
                {
                    sqlcon.Close();
                    Debug.WriteLine(e.ToString());
                    inserted = false;
                }


                if (inserted)
                {
                    foreach (ProductModel p in productlist)
                    {
                        //to insert product based on purchased quantity
                        for (int i = 0; i < p.Qty; i++)
                        {

                            //generate ActivationKey
                            string Activationkey = "";
                            Activationkey = CreateActivationKey();

                            int purchaseitemId = GeneratePurchaseItemID();
                            try
                            {

                                string q = @"INSERT INTO PurchaseItem VALUES(@purchaseitem_id,@product_id,@quantity,
                        @purchaseid,@activation_code)";
                                SqlCommand cmd = new SqlCommand(q, sqlcon);
                                sqlcon.Open();

                                cmd.Parameters.AddWithValue("@purchaseitem_id", purchaseitemId);
                                cmd.Parameters.AddWithValue("@product_id", p.ProductId);
                                cmd.Parameters.AddWithValue("@quantity", 1);
                                cmd.Parameters.AddWithValue("@purchaseid", purchase_id);
                                cmd.Parameters.AddWithValue("@activation_code", Activationkey);
                                cmd.ExecuteNonQuery();

                              
                                inserted = true;
                                sqlcon.Close();
                            }
                            catch (SqlException e)
                            {
                                sqlcon.Close();
                                Debug.WriteLine(e.ToString());
                                inserted = false;
                            }

                        }


                    }
                }


            }
            return inserted;
        }


        private string CreateActivationKey()
        {
            var activationKey = Guid.NewGuid().ToString();

            var activationKeyAlreadyExists = GetAllActivationKeyfromDB().AsEnumerable().Any(key => key.Field<string>("ActivationCode") == activationKey);

            if (activationKeyAlreadyExists)
            {
                activationKey = CreateActivationKey();
            }

            return activationKey;
        }


        private DataTable GetAllActivationKeyfromDB()
        {
            DataTable ActivationKeys = new DataTable();

            using (SqlConnection sqlcon = new SqlConnection(DBConnectionString))
            {
                try
                {

                    string q = @"select ActivationCode from PurchaseItem";
                    SqlCommand cmd = new SqlCommand(q, sqlcon);
                    sqlcon.Open();

                    // create data adapter
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    // this will query your database and return the result to your datatable
                    da.Fill(ActivationKeys);
                    sqlcon.Close();
                    da.Dispose();
                }

                catch (SqlException e)
                {
                    Debug.WriteLine(e.ToString());
                }
            }

            return ActivationKeys;
        }

        public int GeneratePurchaseID()
        {
            int purchase_id = 0;

            using (SqlConnection sqlcon = new SqlConnection(DBConnectionString))
                try
                {

                    string query = "select Max(PurchaseID) As PurchaseID from Purchase;";
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = sqlcon;
                    cmd.CommandText = query;

                    sqlcon.Open();

                    cmd.ExecuteNonQuery();

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        if (reader["PurchaseID"].Equals(DBNull.Value))
                        {
                            purchase_id = 1;
                        }
                        else
                        {
                            purchase_id = Convert.ToInt32(reader["PurchaseID"]) + 1;
                        }
                    }

                }

                catch (SqlException e)
                {
                    Debug.WriteLine(e.ToString());
                }
            return purchase_id;
        }


        public int GeneratePurchaseItemID()
        {
            int purchaseitem_id = 0;

            using (SqlConnection sqlcon = new SqlConnection(DBConnectionString))
                try
                {

                    string query = "select Max(PurchaseItemID) As PurchaseItemID from PurchaseItem;";
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = sqlcon;
                    cmd.CommandText = query;

                    sqlcon.Open();

                    cmd.ExecuteNonQuery();

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        if (reader["PurchaseItemID"].Equals(DBNull.Value))
                        {
                            purchaseitem_id = 1;
                        }
                        else
                        {
                            purchaseitem_id = Convert.ToInt32(reader["PurchaseItemID"]) + 1;
                        }
                    }

                }

                catch (SqlException e)
                {
                    Debug.WriteLine(e.ToString());
                }
            return purchaseitem_id;
        }
        #endregion

        #region Get Purchase Products

        public List<PurchaseProductModel> GetAllPurchaseProductbyUserID(int user_Id)
        {
            List<PurchaseProductModel> products = new List<PurchaseProductModel>();

            using (SqlConnection sqlcon = new SqlConnection(DBConnectionString))
                try
                {

                    string query = @"Select p.PurchaseID,p.PurchaseDate, prod.ProductID,prod.ProductDescription,prod.ProductPrice,prod.ImageID,pit.Quantity,pit.ActivationCode from Purchase p, PurchaseItem pit, Product prod
where p.PurchaseID = pit.PurchaseID and pit.ProductID = prod.ProductID and p.CustomerID = @user_id";
                    SqlCommand cmd = new SqlCommand();
                    cmd.Parameters.AddWithValue("@user_id", user_Id);
                    cmd.Connection = sqlcon;
                    cmd.CommandText = query;


                    sqlcon.Open();

                    cmd.ExecuteNonQuery();

                    SqlDataReader reader = cmd.ExecuteReader();




                    while (reader.Read())
                    {


                        PurchaseProductModel productModel = new PurchaseProductModel()
                        {
                            PurchaseId = Convert.ToInt32(reader["PurchaseID"]),
                            PurchaseDate = DateTime.Parse(reader["PurchaseDate"].ToString()),
                            ProductId = Convert.ToInt32(reader["ProductID"]),
                            // ProductName = (string)reader["product_name"],
                            ProductDescription = reader["ProductDescription"].ToString(),
                            Price = Convert.ToInt32(reader["ProductPrice"]),
                            Qty = 1,
                            Image = "../Content/Images/" + reader["ImageID"].ToString(),
                            ActivationCode = reader["ActivationCode"].ToString()

                        };

                        products.Add(productModel);

                    }
                    sqlcon.Close();
                }

                catch (SqlException e)
                {
                    Debug.WriteLine(e.ToString());
                }
            return products;
        }

        #endregion

    }
}