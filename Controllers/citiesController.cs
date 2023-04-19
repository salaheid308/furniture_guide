using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dalelk.Models;

namespace Dalelk.Controllers
{
    public class citiesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: cities
        public ActionResult Index()
        {
            return View(db.cities.ToList());
        }

        // GET: cities/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            city city = db.cities.Find(id);
            if (city == null)
            {
                return HttpNotFound();
            }
            return View(city);
        }

        // GET: cities/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: cities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( city city, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid && upload != null)
            {
                string name = Guid.NewGuid() + Path.GetExtension(upload.FileName);
                string bath = Path.Combine(Server.MapPath("~/uploads"), name);
                upload.SaveAs(bath);
                city.cityimg = name;
                db.cities.Add(city);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                db.cities.Add(city);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(city);
        }

        // GET: cities/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            city city = db.cities.Find(id);
            if (city == null)
            {
                return HttpNotFound();
            }
            return View(city);
        }

        // POST: cities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(city city, HttpPostedFileBase files)
        {
            if (ModelState.IsValid)
            {
                string old_bath = Path.Combine(Server.MapPath("~/uploads"), city.cityimg);
                if (files != null)
                {

                    System.IO.File.Delete(old_bath);

                    string name = Guid.NewGuid() + Path.GetExtension(files.FileName);
                    string bath = Path.Combine(Server.MapPath("~/uploads"), name);
                    files.SaveAs(bath);
                    city.cityimg = name;

                }
                db.Entry(city).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(city);
        }

        // GET: cities/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            city city = db.cities.Find(id);
            if (city == null)
            {
                return HttpNotFound();
            }
            return View(city);
        }

        // POST: cities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id,city citedel)
        {
            if (citedel.cityimg != null )
            {
                string old_bath = Path.Combine(Server.MapPath("~/uploads"), citedel.cityimg);
                System.IO.File.Delete(old_bath);
            }
           

            city city = db.cities.Find(id);
            db.cities.Remove(city);
            var relatedproduct = db.products.Where(a => a.gallry.city.id == id).ToList();
            
            foreach (var item in relatedproduct)
            {
                var allpic = db.productimages.Where(p => p.productid ==item.id ).ToList();
                foreach (var item1 in allpic)
                {
                    string old_bath1 = Path.Combine(Server.MapPath("~/uploads"), item1.imagename);
                    System.IO.File.Delete(old_bath1);
                }
                string old_bath2 = Path.Combine(Server.MapPath("~/uploads"), item.productimg);
                System.IO.File.Delete(old_bath2);
            }

            db.products.RemoveRange(relatedproduct);
            var relproductcolor = db.productcolor.Where(a => a.gallry.city.id == id).ToList();
            db.productcolor.RemoveRange(relproductcolor);
            var relproductcolor2 = db.productcolor2.Where(a => a.gallry.city.id == id).ToList();
            db.productcolor2.RemoveRange(relproductcolor2);
            var relproductprice = db.productprice.Where(a => a.gallry.city.id == id).ToList();
            db.productprice.RemoveRange(relproductprice);
            
            var relproductsize = db.productsize.Where(a => a.gallry.city.id == id).ToList();
            db.productsize.RemoveRange(relproductsize);
            var relatedctegory = db.categories.Where(a => a.cityid == id).ToList();
            var relatedgallry = db.gallries.Where(a => a.cityid == id).ToList();
            db.categories.RemoveRange(relatedctegory);
            db.gallries.RemoveRange(relatedgallry);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
