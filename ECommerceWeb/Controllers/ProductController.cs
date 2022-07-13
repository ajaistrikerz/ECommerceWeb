using ECommerceWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace ECommerceWeb.Controllers
{
    public class ProductController : Controller
    {
        dbEcommerceEntities _context;
        public ProductController()
        {
            _context = new dbEcommerceEntities();
        }
        public ActionResult ProductList()
        {
            var products = _context.Tble_Product.Include(c => c.Tble_Category).ToList();
            return View(products);
        }
        public ActionResult Create()
        {
            var _categories = _context.Tble_Category.ToList();

            var viewModel = new ProductFromViewModel
            {
                Product = new Tble_Product(),
                Categories = _categories
            };
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult Create(HttpPostedFileBase file, Tble_Product product)
        {
            string filename = Path.GetFileName(file.FileName);
            string _filename = DateTime.Now.ToString("yymmssfff") + filename;
            string extension = Path.GetExtension(file.FileName);
            string path = Path.Combine(Server.MapPath("~/ProductImg/"), _filename);
            product.ProductImage = "~/ProductImg/" + _filename;
            if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
            {
                if (file.ContentLength <= 1000000)
                {
                    _context.Tble_Product.Add(product);
                    if (_context.SaveChanges() > 0)
                    {
                        file.SaveAs(path);
                        ModelState.Clear();
                    }
                }
                else
                {
                    ViewBag.msg = "File size must be Equal or greater than 1MB";
                }
            }
            else
            {
                ViewBag.msg = "Invalid File Type";
            }
            return RedirectToAction("ProductList");


        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var product = _context.Tble_Product.Find(id);

            Session["imgPath"] = product.ProductImage;

            if (product == null)
                return HttpNotFound();

            var viewModel = new ProductFromViewModel
            {
                Product = product,
                Categories = _context.Tble_Category.ToList()
            };

            return View(viewModel);
        }
        [HttpPost]
        public ActionResult Edit(HttpPostedFileBase file, Tble_Product product)
        {
            if (ModelState.IsValid)
            {
                if (file != null && product.ProductId > 0)
                {
                    string filename = Path.GetFileName(file.FileName);
                    string _filename = DateTime.Now.ToString("yymmssfff") + filename;
                    string extension = Path.GetExtension(file.FileName);
                    string path = Path.Combine(Server.MapPath("~/ProductImg/"), _filename);
                    product.ProductImage = "~/ProductImg/" + _filename;
                    if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                    {
                        if (file.ContentLength <= 1000000)
                        {
                            _context.Entry(product).State = System.Data.Entity.EntityState.Modified;

                            if (_context.SaveChanges() > 0)
                            {
                                file.SaveAs(path);
                                TempData["msg"] = "Data Updated";
                                return RedirectToAction("ProductList");
                            }
                        }
                        else
                        {
                            ViewBag.msg = "File size must be Equal or greater than 1MB";
                        }
                    }
                    else
                    {
                        ViewBag.msg = "Invalid File Type";
                    }
                }
                else
                {
                    product.ProductImage = Session["imgPath"].ToString();
                    _context.Entry(product).State = System.Data.Entity.EntityState.Modified;
                    if (_context.SaveChanges() > 0)
                    {
                        TempData["msg"] = "Data Updated";
                        return RedirectToAction("ProductList");
                    }
                }
            }


            return RedirectToAction("ProductList");
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Tble_Product product = _context.Tble_Product.Find(id);

            if (product == null)
                return HttpNotFound();

            _context.Tble_Product.Remove(product);
            _context.SaveChanges();

            return RedirectToAction("ProductList");
        }
        public ActionResult Search(string search)
        {
            var matchingProduct = _context.Tble_Product.Where(p => p.ProductName.Contains(search));
            return View("ProductList", matchingProduct);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _context.Dispose();
            base.Dispose(disposing);
        }
    }
}