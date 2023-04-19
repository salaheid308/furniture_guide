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
    public class cartsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: carts
        [Authorize]
        public ActionResult Index()
        {
            var userid = User.Identity.GetUserId();

            var cartpro = from y in db.products join p in db.carts on y.id equals p.productid
                              where p.userid == userid select y;

            ViewBag.listpro = cartpro.ToList();
            var cartcontent = db.carts.Where(x => x.userid == userid).ToList();
            return View(cartcontent);
        }

        // GET: carts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cart cart = db.carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }
        //public ActionResult addtocart(int proid)
        //{
        //    return RedirectToAction("productdetails", "Home", new { id = proid });
        //}
        [Authorize]
        
        public ActionResult addtocart(int ? id)
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
            cart npro = new cart();
            npro.productid = Convert.ToInt32(id);
            npro.userid = User.Identity.GetUserId();
            npro.quantity = 1;

            db.carts.Add(npro);
            db.SaveChanges();
            return RedirectToAction("index", "Home");
        }

        // GET: carts/Create
        public ActionResult Create(int proid)
        {
            return RedirectToAction("productdetails", "Home", new { id = proid });
        }

        // POST: carts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int proid, int quntity)
        {
            cart npro = new cart();
            npro.productid =  proid;
            npro.userid =  User.Identity.GetUserId();
            npro.quantity = quntity;

            db.carts.Add(npro);
            db.SaveChanges();
            return RedirectToAction("productdetails", "Home", new { id = proid });
        }

       

        // POST: carts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id , int qty)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cart cart = db.carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            cart.quantity = qty;
            db.Entry(cart).State = EntityState.Modified;
             db.SaveChanges();
             return RedirectToAction("Index");
            
           
        }

        // GET: carts/Delete/5
        
        // POST: carts/Delete/5
        
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cart cart = db.carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            
            db.carts.Remove(cart);
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
