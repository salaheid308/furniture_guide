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
    public class storeownersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: storeowners
        public ActionResult Index()
        {
            return View(db.storeowners.ToList());
        }

        // GET: storeowners/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            storeowner storeowner = db.storeowners.Find(id);
            if (storeowner == null)
            {
                return HttpNotFound();
            }
            return View(storeowner);
        }

        // GET: storeowners/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: storeowners/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,PhoneNumber,Password,ConfirmPassword,city,adress,galleryname,galleryphone")] storeowner storeowner)
        {
            if (ModelState.IsValid)
            {
                db.storeowners.Add(storeowner);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(storeowner);
        }

        // GET: storeowners/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            storeowner storeowner = db.storeowners.Find(id);
            if (storeowner == null)
            {
                return HttpNotFound();
            }
            return View(storeowner);
        }

        // POST: storeowners/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,PhoneNumber,Password,ConfirmPassword,city,adress,galleryname,galleryphone")] storeowner storeowner)
        {
            if (ModelState.IsValid)
            {
                db.Entry(storeowner).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(storeowner);
        }

        // GET: storeowners/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            storeowner storeowner = db.storeowners.Find(id);
            if (storeowner == null)
            {
                return HttpNotFound();
            }
            return View(storeowner);
        }

        // POST: storeowners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            storeowner storeowner = db.storeowners.Find(id);
            db.storeowners.Remove(storeowner);
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
