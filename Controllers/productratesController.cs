using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dalelk.Models;
using Microsoft.AspNet.Identity;

namespace Dalelk.Controllers
{
    public class productratesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: productrates
        public ActionResult Index()
        {
            var productrates = db.productrates.Include(p => p.product);
            return View(productrates.ToList());
        }

        // GET: productrates/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            productrate productrate = db.productrates.Find(id);
            if (productrate == null)
            {
                return HttpNotFound();
            }
            return View(productrate);
        }

        // GET: productrates/Create
        public ActionResult Create(int id)
        {
            return RedirectToAction("productdetails", "Home", new { id = id });
        }

        // POST: productrates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int id, int rate)
        {
            var userid = User.Identity.GetUserId();
            var currentuser = db.Users.Where(a => a.Id == userid).SingleOrDefault();
            var fondrate = db.productrates.FirstOrDefault(r => r.userid == currentuser.Id && r.productid == id);
            if (fondrate == null)
            {
            productrate nrate = new productrate();
            nrate.productid = id;
            nrate.rate = rate;
            nrate.userid = currentuser.Id;
            db.productrates.Add(nrate);
            db.SaveChanges();
            return RedirectToAction("productdetails", "Home", new { id = id });
             }
            else
            {

                fondrate.rate = rate;
                db.Entry(fondrate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("productdetails", "Home", new { id = id });
            }
        }

        // GET: productrates/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            productrate productrate = db.productrates.Find(id);
            if (productrate == null)
            {
                return HttpNotFound();
            }
            ViewBag.productid = new SelectList(db.products, "id", "productname", productrate.productid);
            return View(productrate);
        }

        // POST: productrates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,rate,userid,productid")] productrate productrate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productrate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.productid = new SelectList(db.products, "id", "productname", productrate.productid);
            return View(productrate);
        }

        // GET: productrates/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            productrate productrate = db.productrates.Find(id);
            if (productrate == null)
            {
                return HttpNotFound();
            }
            return View(productrate);
        }

        // POST: productrates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            productrate productrate = db.productrates.Find(id);
            db.productrates.Remove(productrate);
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
