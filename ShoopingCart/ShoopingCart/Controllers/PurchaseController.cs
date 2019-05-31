using ShoopingCart.Models.Entity;
using ShoopingCart.Models.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoopingCart.Controllers
{
    public class PurchaseController : Controller
    {
        static ProductList myCart;

        private Purchase_Service purchase_Service;

        public PurchaseController()
        {
            purchase_Service = new Purchase_Service();
        }
        // GET: CheckOut
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MyPurchase()
        {

            int user_id = 1; //hard coded+++++++++++++++++

            myCart = (ProductList)Session["cart"];

            List<ProductModel> productstoinsert = new List<ProductModel>();

            if (myCart != null)
            {
                productstoinsert = myCart.GetCartItems();
            }


            //to insert products from session to database
            if (productstoinsert != null)
            {
                if (productstoinsert.Count > 0)
                {
                    bool inserted = purchase_Service.Insert_Purchase_Products_To_Db(productstoinsert, user_id);

                    //clear product list from session after inserted
                    Session.Remove("cart");
                }
            }


            //Display all purchased products to view

            List<PurchaseProductModel> purchasedproductsfromdb = new List<PurchaseProductModel>();

            purchasedproductsfromdb = purchase_Service.GetPurchasedProductbyID(user_id);

            if (purchasedproductsfromdb.Count > 0)
            {
                List<PurchaseProductViewModel> purchasedproductstodisplay = new List<PurchaseProductViewModel>();


                #region test
                foreach (PurchaseProductModel p in purchasedproductsfromdb)
                {
                   
                    if (purchasedproductstodisplay.Count > 0)
                    {
                        if (purchasedproductstodisplay.Where(x => x.PurchaseId == p.PurchaseId && x.ProductId == p.ProductId).ToList().Count > 0)
                        {
                            purchasedproductstodisplay.Where(x => x.PurchaseId == p.PurchaseId && x.ProductId == p.ProductId)
                           .Select(y => { y.Qty = y.Qty + p.Qty; y.ActivationCode.Add(p.ActivationCode); return y; })
                           .ToList();
                        }
                        else
                        {
                            PurchaseProductViewModel purchaseprod = new PurchaseProductViewModel();

                            purchaseprod.PurchaseId = p.PurchaseId;
                            purchaseprod.PurchaseDate = p.PurchaseDate;
                            purchaseprod.ProductId = p.ProductId;
                            purchaseprod.ProductDescription = p.ProductDescription;
                            purchaseprod.Qty = p.Qty;
                            purchaseprod.Image = p.Image;

                            List<string> ActivationKeys = new List<string>();
                            ActivationKeys.Add(p.ActivationCode);

                            purchaseprod.ActivationCode = ActivationKeys;

                            purchasedproductstodisplay.Add(purchaseprod);
                        }
                             
                    }
                    else
                    {
                        PurchaseProductViewModel purchaseprod = new PurchaseProductViewModel();

                        purchaseprod.PurchaseId = p.PurchaseId;
                        purchaseprod.PurchaseDate = p.PurchaseDate;
                        purchaseprod.ProductId = p.ProductId;
                        purchaseprod.ProductDescription = p.ProductDescription;
                        purchaseprod.Qty = p.Qty;
                        purchaseprod.Image = p.Image;

                        List<string> ActivationKeys = new List<string>();
                        ActivationKeys.Add(p.ActivationCode);

                        purchaseprod.ActivationCode = ActivationKeys;

                        purchasedproductstodisplay.Add(purchaseprod);
                    }

                }
                #endregion

                ViewData["purchasedProducts"] = purchasedproductstodisplay;
            }
            else
            {
                ViewData["purchasedProducts"] = null;
            }



            return View();
        }
    }
}