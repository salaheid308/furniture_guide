using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dalelk.Models;

namespace Dalelk.Controllers
{
    public class driverpricesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: driverprices
        public ActionResult Index()
        {
            var driverprices = db.driverprices.Include(d => d.city);
            return View(driverprices.ToList());
        }

        // GET: driverprices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            driverprice driverprice = db.driverprices.Find(id);
            if (driverprice == null)
            {
                return HttpNotFound();
            }
            return View(driverprice);
        }

        // GET: driverprices/Create
        public ActionResult Create()
        {
            ViewBag.cityid = new SelectList(db.cities, "id", "cityname");
            return View();
        }

        // POST: driverprices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( int id , int Segmentation ,int price)
        {

            if (Segmentation != 0 )
            {

                driverprice nprice = new driverprice();
                nprice.cityid = Segmentation;
                nprice.driverid = id;
                nprice.price = price;

                db.driverprices.Add(nprice);
                db.SaveChanges();
                return RedirectToAction("driverdetails", "Home", new { id = id });
            }
            return RedirectToAction("driverdetails", "Home", new { id = id });
        }

        // GET: driverprices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            driverprice driverprice = db.driverprices.Find(id);
            if (driverprice == null)
            {
                return HttpNotFound();
            }
           
            return View(driverprice);
        }

        // POST: driverprices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( driverprice driverprice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(driverprice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("driverdetails", "Home", new { id = driverprice.driverid });
            }
           var curdrip = db.driverprices.Find(driverprice.id);
            return View(curdrip);
        }

        // GET: driverprices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            driverprice driverprice = db.driverprices.Find(id);
            if (driverprice == null)
            {
                return HttpNotFound();
            }
            return View(driverprice);
        }

        // POST: driverprices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            driverprice driverprice = db.driverprices.Find(id);
            db.driverprices.Remove(driverprice);
            db.SaveChanges();
            return RedirectToAction("driverdetails", "Home", new { id = driverprice.driverid });
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
