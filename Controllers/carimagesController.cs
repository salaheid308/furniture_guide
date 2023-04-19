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
    public class carimagesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: carimages
        public ActionResult Index()
        {
            var carimages = db.carimages.Include(c => c.driver);
            return View(carimages.ToList());
        }

        // GET: carimages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            carimages carimages = db.carimages.Find(id);
            if (carimages == null)
            {
                return HttpNotFound();
            }
            return View(carimages);
        }

        // GET: carimages/Create
        public ActionResult Create()
        {
            ViewBag.driverid = new SelectList(db.drivers, "id", "name");
            return View();
        }

        // POST: carimages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( int id , HttpPostedFileBase carphoto )
        {

            carimages carimages = new carimages();

                if (carimages != null)
                {
                    var  name = Guid.NewGuid() + Path.GetExtension(carphoto.FileName);
                    string bath = Path.Combine(Server.MapPath("~/carimages"), name);
                    carphoto.SaveAs(bath);

                     carimages.driverid = id;
                    carimages.imagename = name;
                    db.carimages.Add(carimages);
                    db.SaveChanges();
                    return RedirectToAction("driverdetails", "Home", new { id = id });
                }
                return RedirectToAction("driverdetails", "Home", new { id = id });
           
        }

        // GET: carimages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            carimages carimages = db.carimages.Find(id);
            if (carimages == null)
            {
                return HttpNotFound();
            }
            ViewBag.driverid = new SelectList(db.drivers, "id", "name", carimages.driverid);
            return View(carimages);
        }

        // POST: carimages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,imagename,driverid")] carimages carimages)
        {
            if (ModelState.IsValid)
            {
                db.Entry(carimages).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.driverid = new SelectList(db.drivers, "id", "name", carimages.driverid);
            return View(carimages);
        }

        // GET: carimages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            carimages carimages = db.carimages.Find(id);
            if (carimages == null)
            {
                return HttpNotFound();
            }
            return View(carimages);
        }

        // POST: carimages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            carimages carimages = db.carimages.Find(id);

            string old_bath = Path.Combine(Server.MapPath("~/carimages"), carimages.imagename);
            System.IO.File.Delete(old_bath);

            db.carimages.Remove(carimages);
            db.SaveChanges();
            return RedirectToAction("driverdetails", "Home", new { id = carimages.driverid });
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
