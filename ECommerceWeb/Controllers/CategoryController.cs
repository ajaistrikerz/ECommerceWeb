using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ECommerceWeb.Controllers
{
    public class CategoryController : Controller
    {
        dbEcommerceEntities _context;
        public CategoryController()
        {
            _context = new dbEcommerceEntities();
        }
        public ActionResult CategoryList()
        {
            var categories = _context.Tble_Category.ToList();
            return View(categories);
        }
        public ActionResult Create()
        {
            return View(new Tble_Category { CategoryId = 0 });
        }
        [HttpPost]
        public ActionResult Create(Tble_Category category)
        {
            if (ModelState.IsValid)
            {

                if (category.CategoryId > 0)
                    _context.Entry(category).State = System.Data.Entity.EntityState.Modified;

                else
                    _context.Tble_Category.Add(category);


                _context.SaveChanges();
                return RedirectToAction("CategoryList");
            }

            return RedirectToAction("Create", category);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var category = _context.Tble_Category.Find(id);

            if (category == null)
                return HttpNotFound();

            return View("Create", category);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Tble_Category category = _context.Tble_Category.Find(id);

            if (category == null)
                return HttpNotFound();

            _context.Tble_Category.Remove(category);
            _context.SaveChanges();

            return RedirectToAction("CategoryList");
        }

        public ActionResult Search(string search)
        {
            var matchingCategory = _context.Tble_Category.Where(p => p.CategoryName.Contains(search));
            return View("CategoryList", matchingCategory);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _context.Dispose();
            base.Dispose(disposing);
        }
    }
}