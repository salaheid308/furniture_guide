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
    public class favoritesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: favorites
        [Authorize]
        public ActionResult Index()
        {
            var userid = User.Identity.GetUserId();

            var cartpro = from y in db.products
                          join p in db.favorites on y.id equals p.productid
                          where p.userid == userid
                          select y;

            ViewBag.listpro = cartpro.ToList();
            var favcontent = db.favorites.Where(x => x.userid == userid).ToList();
            return View(favcontent);
        }

        // GET: favorites/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            favorite favorite = db.favorites.Find(id);
            if (favorite == null)
            {
                return HttpNotFound();
            }
            return View(favorite);
        }
        [Authorize]
        public ActionResult addnfav(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product pro = db.products.Find(id);
            if (pro == null)
            {
                return HttpNotFound();
            }
            favorite npro = new favorite();
            npro.productid = Convert.ToInt32(id);
            npro.userid = User.Identity.GetUserId();
           
            db.favorites.Add(npro);
            db.SaveChanges();
            return RedirectToAction("index", "Home");
        }
        // GET: favorites/Create
        public ActionResult Create(int proid)
        {
            return RedirectToAction("productdetails", "Home", new { id = proid });
        }

        // POST: favorites/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult addtofav(int proid)
        {
            favorite npro = new favorite();
            npro.productid = proid;
            npro.userid = User.Identity.GetUserId();
            
            db.favorites.Add(npro);
            db.SaveChanges();
            return RedirectToAction("productdetails", "Home", new { id = proid });
        }

        // GET: favorites/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            favorite favorite = db.favorites.Find(id);
            if (favorite == null)
            {
                return HttpNotFound();
            }
            ViewBag.productid = new SelectList(db.products, "id", "productname", favorite.productid);
           
            return View(favorite);
        }

        // POST: favorites/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,userid,productid")] favorite favorite)
        {
            if (ModelState.IsValid)
            {
                db.Entry(favorite).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.productid = new SelectList(db.products, "id", "productname", favorite.productid);
            
            return View(favorite);
        }

        // GET: favorites/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            favorite fav = db.favorites.Find(id);
            if (fav == null)
            {
                return HttpNotFound();
            }

            db.favorites.Remove(fav);
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
