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
    public class productimagesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: productimages
        public ActionResult Index(int? pid)
        {
            if (pid != null)
            {
                var relatedimg = db.productimages.Where(d => d.productid == pid).Include(p => p.product).ToList();
                return View(relatedimg);
            }
            else
            {
                var productimages = db.productimages.Include(p => p.product);
                return View(productimages.ToList());
            }
        }

        // GET: productimages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            productimages productimages = db.productimages.Find(id);
            if (productimages == null)
            {
                return HttpNotFound();
            }
            return View(productimages);
        }

        // GET: productimages/Create
        public ActionResult Create()
        {
            ViewBag.productid = new SelectList(db.products, "id", "datetime");
            return View();
        }

        // POST: productimages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( productimages productimages , imgfiles obgct  )
        {
            if (ModelState.IsValid)
            {
                foreach (var file in obgct.files )
                {
                    if (file !=null && file.ContentLength >0 )
                    {
                        string name = Guid.NewGuid() + Path.GetExtension(file.FileName);
                        file.SaveAs(Path.Combine(Server.MapPath("~/uploads"), name));
                        productimages.imagename = name ;
                        productimages.datetime = DateTime.Now;
                        db.productimages.Add(productimages);
                        db.SaveChanges();
                        
                    }

                }
                return RedirectToAction("Index");
            }

            ViewBag.productid = new SelectList(db.products, "id", "productname", productimages.productid);
            return View(productimages);
        }

        public class imgfiles
        {
            public List<HttpPostedFileBase> files { get; set; }
        }
       

        // GET: productimages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            productimages productimages = db.productimages.Find(id);
            if (productimages == null)
            {
                return HttpNotFound();
            }
            //ViewBag.productid = new SelectList(db.products, "id", "productname", productimages.productid);
            return View(productimages);
        }

        // POST: productimages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( productimages productimages , HttpPostedFileBase files )
        {
            if (ModelState.IsValid)
            {

                string old_bath = Path.Combine(Server.MapPath("~/uploads"), productimages.imagename );
                if (files != null)
                {

                    System.IO.File.Delete(old_bath);

                    string name = Guid.NewGuid() + Path.GetExtension(files.FileName);
                    string bath = Path.Combine(Server.MapPath("~/uploads"), name);
                    files.SaveAs(bath);
                    productimages.imagename = name;

                }

                
                db.Entry(productimages).State = EntityState.Modified;
                    db.SaveChanges();
                return RedirectToAction("Index", "products");


            }
            //ViewBag.productid = new SelectList(db.products, "id", "productname", productimages.productid);
            return View(productimages);
        }

        // GET: productimages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            productimages productimages = db.productimages.Find(id);
            if (productimages == null)
            {
                return HttpNotFound();
            }
            return View(productimages);
        }

        // POST: productimages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id ,productimages productimgdel)
        {
            string old_bath = Path.Combine(Server.MapPath("~/uploads"), productimgdel.imagename);
            System.IO.File.Delete(old_bath);
            productimages productimages = db.productimages.Find(id);
            db.productimages.Remove(productimages);
            db.SaveChanges();
            return RedirectToAction("Index","products");
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
