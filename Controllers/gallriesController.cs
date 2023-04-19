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
    public class gallriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: gallries
        public ActionResult Index()
        {
            ViewBag.time = DateTime.Now.Minute;
            var gallries = db.gallries.Include(g => g.category).Include(p => p.city);
            return View(gallries.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gallries galry = db.gallries.Find(id);
            if (galry == null)
            {
                return HttpNotFound();
            }
            return View(galry);
        }

        public ActionResult userdetails(int? id, string searshbox)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gallries galry = db.gallries.Find(id);
            if (searshbox != null)
            {
                var result3 = db.products.Where(l => l.gallryid == id);
                var result4 = result3.Where(a => a.productname.Contains(searshbox)
                || a.productcolor.Contains(searshbox)
                || a.productcolor2.Contains(searshbox)
                || a.productsize.Contains(searshbox)).ToList();
                ViewBag.mov = result4;
                
            }
            if (galry == null)
            {
                return HttpNotFound();
            }
            return View(galry);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editsocial(gallries gallries)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gallries).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("userdetails", "gallries", new { id = gallries.id });
            }

            return RedirectToAction("userdetails", "gallries", new { id = gallries.id });
        }
        // GET: gallries/Details/5
        //public ActionResult Details(int? id)
        //{

        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    string eid = id.ToString();

        //    int i = DateTime.Now.Hour;
        //    string o = i.ToString();
        //    if (o.Length == 1)
        //    {
        //        string dat = eid.Remove(1);
        //        int dt = int.Parse(dat);
        //        if (dt == DateTime.Now.Hour)
        //        {
        //            string ids = id.ToString();
        //            string nid = ids.Remove(0, 1);
        //            id = int.Parse(nid);
        //            gallries gallries = db.gallries.Find(id);

        //            return View(gallries);
        //        }
        //        else
        //        {
        //            return HttpNotFound();
        //        }
        //    }
        //    else
        //    {
        //        string dat = eid.Remove(2);
        //        int dt = int.Parse(dat);
        //        if (dt == DateTime.Now.Hour)
        //        {
        //            string ids = id.ToString();
        //            string nid = ids.Remove(0, 2);
        //            id = int.Parse(nid);
        //            gallries gallries = db.gallries.Find(id);

        //            return View(gallries);
        //        }
        //        else
        //        {
        //            return HttpNotFound();
        //        }
        //    }


        //}

        // GET: gallries/Create
        public ActionResult Create(int ctyid)
        {
            ViewBag.catlist = db.categories.Where(u=>u.cityid==ctyid).ToList();
            ViewBag.cityid = ctyid;
            return View();
        }

        // POST: gallries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( gallries gallries, int ctyid, int catid)
        {
            if (ModelState.IsValid)
            {
                var userid = User.Identity.GetUserId();
                var currentuser = db.Users.Where(a => a.Id == userid).SingleOrDefault();
                if (currentuser.usertype == "sales")
                {

                    var laststor = db.storeowners.FirstOrDefault(i=>i.userid==currentuser.Id);
                    int gname = laststor.id;
                    gallries.gallryname = "معرض" + db.Users.Count();
                    gallries.userid = currentuser.Id;
                    gallries.categoryid = catid;
                    gallries.cityid = ctyid;
                    db.gallries.Add(gallries);
                    db.SaveChanges();
                    return RedirectToAction("userdetails", "gallries", new { id = gallries.id });
                }
                db.gallries.Add(gallries);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.catlist = db.categories.Where(u => u.cityid == ctyid).ToList();
            ViewBag.cityid = ctyid;
            return View(gallries);
        }

        // GET: gallries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gallries gallries = db.gallries.Find(id);
            if (gallries == null)
            {
                return HttpNotFound();
            }
            ViewBag.cityid = new SelectList(db.cities, "id", "cityname", gallries.cityid);
            ViewBag.categoryid = new SelectList(db.categories, "id", "categoryname", gallries.categoryid);
            return View(gallries);
        }

        // POST: gallries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( gallries gallries)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gallries).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.cityid = new SelectList(db.cities, "id", "cityname", gallries.cityid);
            ViewBag.categoryid = new SelectList(db.categories, "id", "categoryname", gallries.categoryid);
            return View(gallries);
        }

        // GET: gallries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gallries gallries = db.gallries.Find(id);
            if (gallries == null)
            {
                return HttpNotFound();
            }
            return View(gallries);
        }

        // POST: gallries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            gallries gallries = db.gallries.Find(id);
            db.gallries.Remove(gallries);
            var relatedproduct = db.products.Where(a => a.gallryid == id).ToList();
            db.products.RemoveRange(relatedproduct);
            var relproductcolor = db.productcolor.Where(a => a.gallryid == id).ToList();
            db.productcolor.RemoveRange(relproductcolor);
            var relproductcolor2 = db.productcolor2.Where(a => a.gallryid == id).ToList();
            db.productcolor2.RemoveRange(relproductcolor2);
            var relproductprice = db.productprice.Where(a => a.gallryid == id).ToList();
            db.productprice.RemoveRange(relproductprice);
            
            var relproductsize = db.productsize.Where(a => a.gallryid == id).ToList();
            db.productsize.RemoveRange(relproductsize);
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
