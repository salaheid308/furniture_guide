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
    public class driversController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: drivers
        public ActionResult Index()
        {
            var drivers = db.drivers.Include(d => d.city);
            return View(drivers.ToList());
        }

        // GET: drivers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            drivers drivers = db.drivers.Find(id);
            if (drivers == null)
            {
                return HttpNotFound();
            }
            return View(drivers);
        }

        // GET: drivers/Create
        public ActionResult Create()
        {
            ViewBag.cityid = new SelectList(db.cities, "id", "cityname");
            return View();
        }

        // POST: drivers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,PhoneNumber,Password,ConfirmPassword,userimg,cityid")] drivers drivers)
        {
            if (ModelState.IsValid)
            {
                db.drivers.Add(drivers);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.cityid = new SelectList(db.cities, "id", "cityname", drivers.cityid);
            return View(drivers);
        }

        // GET: drivers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            drivers drivers = db.drivers.Find(id);
            if (drivers == null)
            {
                return HttpNotFound();
            }
            ViewBag.cityid = new SelectList(db.cities, "id", "cityname", drivers.cityid);
            return View(drivers);
        }

        // POST: drivers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(drivers drivers , HttpPostedFileBase newphoto)
        {
            if (ModelState.IsValid)
            {
                var name = "splash_image.png";
                if (newphoto != null)
                {
                    string old_bath = Path.Combine(Server.MapPath("~/driverimgs"), drivers.userimg);
                    System.IO.File.Delete(old_bath);

                    name = Guid.NewGuid() + Path.GetExtension(newphoto.FileName);
                    string bath = Path.Combine(Server.MapPath("~/driverimgs"), name);
                    newphoto.SaveAs(bath);
                }
                else
                {
                    return RedirectToAction("driverdetails", "Home", new { myprofile = drivers.userid });
                }
                drivers.userimg = name;
                db.Entry(drivers).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("driverdetails", "Home", new { myprofile = drivers.userid });
            }

            return RedirectToAction("driverdetails", "Home", new { myprofile = drivers.userid });
        }

        // GET: drivers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            drivers drivers = db.drivers.Find(id);
            if (drivers == null)
            {
                return HttpNotFound();
            }
            return View(drivers);
        }

        // POST: drivers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            drivers drivers = db.drivers.Find(id);
            db.drivers.Remove(drivers);
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
