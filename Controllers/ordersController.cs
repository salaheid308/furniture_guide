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
    public class ordersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: orders
        [Authorize]
        public ActionResult Index()
        {
            var userid = User.Identity.GetUserId();

            var cartpro = from y in db.products
                          join p in db.orders on y.id equals p.productid
                          where p.userid == userid
                          select y;
            var cartcontent = db.orders.Where(x => x.userid == userid).ToList();
            ViewBag.listpro = cartpro.ToList();
            
            return View(cartcontent);
        }
        public ActionResult orderlist(int? id)
        {
            return View(db.orders.Where(i=>i.orderstatus== "yes").ToList());
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            order order = db.orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: orders/Details/5
        [Authorize]
        public ActionResult payment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var userid = User.Identity.GetUserId();
            
            var order = db.orders.FirstOrDefault(r=>r.userid == userid && r.productid == id);
            if (order == null)
            {
                return HttpNotFound();
            }
            //sb-intr66632430@personal.example.com
            //System Generated Password:
            //)qFC/d.3
            //            sb-dsnpx6631862@business.example.com
            //  System Generated Password:
            //ZD5y/}y<

            var pro = db.products.Find(id);
            var total = order.total;
            var totalprice = pro.productprice * total;
            return Redirect("https://www.sandbox.paypal.com/cgi-bin/webscr?cmd=_xclick&amount=" + (totalprice / 15) + "&business=sb-dsnpx6631862@business.example.com&item_name=" + (pro.productname) + "&return=http://http://furnituregid-001-site1.dtempurl.com//orders/Edit/" + (pro.id)+"");
        }

        // GET: orders/Create
        [Authorize]
        public ActionResult Create()
        {
            //var userid = User.Identity.GetUserId();
            //var carcont = db.carts.Where(t => t.userid == userid).ToList();

            //var cartpro = from y in db.products
            //              join p in db.carts on y.id equals p.productid
            //              where p.userid == userid
            //              select y;
            //var proli = cartpro.ToList();
            //int total = 0;
            //foreach (var item in carcont)
            //{
            //    total += item.quantity*item.product.productprice;
            //}

            //ViewBag.totl = total;
            //ViewBag.listpro = cartpro.ToList();
            return View();
        }

        // POST: orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(order order)
        {
            var userid = User.Identity.GetUserId();
            var carcont = db.carts.Where(t => t.userid == userid).ToList();
            var cartpro = from y in db.products
                          join p in db.carts on y.id equals p.productid
                          where p.userid == userid
                          select y;
            var pro = cartpro.ToList();
            if (ModelState.IsValid)
            {
               
                foreach (var item in carcont)
                {
                    order.datetime = DateTime.Now;
                    order.orderstatus = "no";
                    order.productid = item.product.id;
                    order.userid = userid;
                    order.total = item.quantity;
                    db.orders.Add(order);
                    db.SaveChanges();
                }
                
                    db.carts.RemoveRange(carcont);
                    db.SaveChanges();
                
                return RedirectToAction("Index");
            }
            
            int total = 0;
            foreach (var item in carcont)
            {
                total += item.quantity * item.product.productprice;
            }
            ViewBag.totl = total;
            ViewBag.listpro = cartpro.ToList();
            return View(order);
        }

        // GET: orders/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var userid = User.Identity.GetUserId();

            var order = db.orders.FirstOrDefault(r => r.userid == userid && r.productid == id);

            if (order == null)
            {
                return HttpNotFound();
            }
            order.datetime = DateTime.Now;
            order.orderstatus = "yes";
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
           
        }

        // POST: orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,userid,productid,total,datetime,First_Name,last_Name,Country,Address,City,Phone1,Phone2,Order_notes,orderstatus")] order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.productid = new SelectList(db.products, "id", "productname", order.productid);
           
            return View(order);
        }

        // GET: orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            order order = db.orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            if (order.orderstatus == "no")
            {
                db.orders.Remove(order);
                db.SaveChanges();
               
            }
            return RedirectToAction("Index"); 

        }
        public ActionResult Deleteorder(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            order order = db.orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        

        // POST: orders/Delete/5
        [HttpPost, ActionName("Deleteorder")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            order order = db.orders.Find(id);
            db.orders.Remove(order);
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
