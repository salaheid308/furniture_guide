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
using Microsoft.AspNet.Identity;

namespace Dalelk.Controllers
{
    public class productsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: products1
        public ActionResult Index()
        {
            var products = db.products.ToList();
            return View(products);
        }

        public ActionResult productdetailss()
        {

            return View();
        }

        // GET: products1/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product product = db.products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: products1/Create
        public ActionResult Create()
        {
            ViewBag.categoryid = new SelectList(db.categories, "id", "categoryname");
            ViewBag.gallryid = new SelectList(db.gallries, "id", "gallryname");
            return View();


        }

        // POST: products1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(product product, HttpPostedFileBase upload, imgfiless obgct , int? id , string favcolor , string favcolor2)
        {
            if (ModelState.IsValid)
            {
                if (Request.IsAuthenticated)
                {

                
                var userid = User.Identity.GetUserId();
                var currentuser = db.Users.Where(a => a.Id == userid).SingleOrDefault();

                    if (currentuser.usertype == "sales")
                    {
                        var gallriesdata = db.gallries.First(a => a.userid == currentuser.Id && a.id == id);
                        product.categoryid = gallriesdata.categoryid;
                        product.gallryid = gallriesdata.id;

                        string name = Guid.NewGuid() + Path.GetExtension(upload.FileName);
                        string bath = Path.Combine(Server.MapPath("~/uploads"), name);
                        upload.SaveAs(bath);


                        var color = db.productcolor.FirstOrDefault(x => x.colorname == favcolor && x.categoryid == product.categoryid && x.gallryid == product.gallryid);
                        if (color != null)
                        {
                            ViewBag.colorid = color.id;
                        }
                        else
                        {
                            productcolor ncolor = new productcolor();
                            ncolor.colorname = favcolor;
                            ncolor.categoryid = product.categoryid;
                            ncolor.gallryid = product.gallryid;
                            db.productcolor.Add(ncolor);
                            db.SaveChanges();
                            ViewBag.colorid = ncolor.id;
                        }

                        var color2 = db.productcolor2.FirstOrDefault(x => x.color2name == favcolor2 && x.categoryid == product.categoryid && x.gallryid == product.gallryid);
                        if (color2 != null)
                        {
                            ViewBag.color2id = color2.id;
                        }
                        else
                        {
                            productcolor2 ncolor2 = new productcolor2();
                            ncolor2.color2name = favcolor2;
                            ncolor2.categoryid = product.categoryid;
                            ncolor2.gallryid = product.gallryid;
                            db.productcolor2.Add(ncolor2);
                            db.SaveChanges();
                            ViewBag.color2id = ncolor2.id;
                        }

                        var product_price = db.productprice.FirstOrDefault(x => x.price == product.productprice && x.categoryid == product.categoryid && x.gallryid == product.gallryid);
                        if (product_price != null)
                        {
                            ViewBag.productpriceid = product_price.id;
                        }
                        else
                        {
                            productprice product__price = new productprice();
                            product__price.price = product.productprice;
                            product__price.categoryid = product.categoryid;
                            product__price.gallryid = product.gallryid;
                            db.productprice.Add(product__price);
                            db.SaveChanges();
                            ViewBag.productpriceid = product__price.id;
                        }

                       

                        var product_size = db.productsize.FirstOrDefault(x => x.size == product.productsize && x.categoryid == product.categoryid && x.gallryid == product.gallryid);
                        if (product_size != null)
                        {
                            ViewBag.productsizeid = product_size.id;
                        }
                        else
                        {
                            productsize product__size = new productsize();
                            product__size.size = product.productsize;
                            product__size.categoryid = product.categoryid;
                            product__size.gallryid = product.gallryid;
                            db.productsize.Add(product__size);
                            db.SaveChanges();
                            ViewBag.productsizeid = product__size.id;
                        }
                        product.colorid = ViewBag.colorid;
                        product.color2id = ViewBag.color2id;
                        product.priceid = ViewBag.productpriceid;
                        product.sizeid = ViewBag.productsizeid;
                        
                        product.productcolor2 = favcolor2; 
                        product.productimg = name;
                        product.productcolor = favcolor;
                        product.datetime = DateTime.Now;
                        db.products.Add(product);
                        db.SaveChanges();
                        productimages productimages = new productimages();

                        foreach (var file in obgct.files)
                        {
                            if (file != null && file.ContentLength > 0)
                            {
                                string name1 = Guid.NewGuid() + Path.GetExtension(file.FileName);
                                file.SaveAs(Path.Combine(Server.MapPath("~/uploads"), name1));
                                productimages.imagename = name1;
                                productimages.productid = product.id;
                                productimages.datetime = DateTime.Now;
                                db.productimages.Add(productimages);
                                db.SaveChanges();

                            }

                        }
                        return RedirectToAction("userdetails", "gallries", new { id = product.gallryid });
                     }
                    }
                else
                {

                    string name = Guid.NewGuid() + Path.GetExtension(upload.FileName);
                    string bath = Path.Combine(Server.MapPath("~/uploads"), name);
                    upload.SaveAs(bath);


                    var color = db.productcolor.FirstOrDefault(x => x.colorname == favcolor && x.categoryid == product.categoryid && x.gallryid == product.gallryid);
                    if (color != null)
                    {
                        ViewBag.colorid = color.id;
                    }
                    else
                    {
                        productcolor ncolor = new productcolor();
                        ncolor.colorname = favcolor;
                        ncolor.categoryid = product.categoryid;
                        ncolor.gallryid = product.gallryid;
                        db.productcolor.Add(ncolor);
                        db.SaveChanges();
                        ViewBag.colorid = ncolor.id;
                    }

                    var color2 = db.productcolor2.FirstOrDefault(x => x.color2name == favcolor2 && x.categoryid == product.categoryid && x.gallryid == product.gallryid);
                    if (color2 != null)
                    {
                        ViewBag.color2id = color2.id;
                    }
                    else
                    {
                        productcolor2 ncolor2 = new productcolor2();
                        ncolor2.color2name = favcolor2;
                        ncolor2.categoryid = product.categoryid;
                        ncolor2.gallryid = product.gallryid;
                        db.productcolor2.Add(ncolor2);
                        db.SaveChanges();
                        ViewBag.color2id = ncolor2.id;
                    }

                    var product_price = db.productprice.FirstOrDefault(x => x.price == product.productprice && x.categoryid == product.categoryid && x.gallryid == product.gallryid);
                    if (product_price != null)
                    {
                        ViewBag.productpriceid = product_price.id;
                    }
                    else
                    {
                        productprice product__price = new productprice();
                        product__price.price = product.productprice;
                        product__price.categoryid = product.categoryid;
                        product__price.gallryid = product.gallryid;
                        db.productprice.Add(product__price);
                        db.SaveChanges();
                        ViewBag.productpriceid = product__price.id;
                    }

                    

                    var product_size = db.productsize.FirstOrDefault(x => x.size == product.productsize && x.categoryid == product.categoryid && x.gallryid == product.gallryid);
                    if (product_size != null)
                    {
                        ViewBag.productsizeid = product_size.id;
                    }
                    else
                    {
                        productsize product__size = new productsize();
                        product__size.size = product.productsize;
                        product__size.categoryid = product.categoryid;
                        product__size.gallryid = product.gallryid;
                        db.productsize.Add(product__size);
                        db.SaveChanges();
                        ViewBag.productsizeid = product__size.id;
                    }
                    product.colorid = ViewBag.colorid;
                    product.color2id = ViewBag.color2id;
                    product.priceid = ViewBag.productpriceid;
                    
                    product.sizeid = ViewBag.productsizeid;
                    product.productimg = name;
                    product.productcolor = favcolor;
                    
                    product.productcolor2 = favcolor2;
                    product.datetime = DateTime.Now;
                    db.products.Add(product);
                    db.SaveChanges();

                    productimages productimages = new productimages();
                    foreach (var file in obgct.files)
                    {
                        if (file != null && file.ContentLength > 0)
                        {
                            string name1 = Guid.NewGuid() + Path.GetExtension(file.FileName);
                            file.SaveAs(Path.Combine(Server.MapPath("~/uploads"), name1));
                            productimages.imagename = name1;
                            productimages.productid = product.id;
                            productimages.datetime = DateTime.Now;
                            db.productimages.Add(productimages);
                            db.SaveChanges();

                        }

                    }
                    return View("index");
                }
            }

            ViewBag.categoryid = new SelectList(db.categories, "id", "categoryname", product.categoryid);
            ViewBag.gallryid = new SelectList(db.gallries, "id", "gallryname", product.gallryid);
            
            return View(product);
        }

        public class imgfiless
        {
            public List<HttpPostedFileBase> files { get; set; }
        }

        
        // GET: products1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product product = db.products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.categoryid = new SelectList(db.categories, "id", "categoryname", product.categoryid);
            ViewBag.gallryid = new SelectList(db.gallries, "id", "gallryname", product.gallryid);
            return View(product);
        }

        // POST: products1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(product product, HttpPostedFileBase files)
        {
            if (ModelState.IsValid)
            {
                string old_bath = Path.Combine(Server.MapPath("~/uploads"), product.productimg);
                if (files != null)
                {

                    System.IO.File.Delete(old_bath);

                    string name = Guid.NewGuid() + Path.GetExtension(files.FileName);
                    string bath = Path.Combine(Server.MapPath("~/uploads"), name);
                    files.SaveAs(bath);
                    product.productimg = name;

                }
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.categoryid = new SelectList(db.categories, "id", "categoryname", product.categoryid);
            ViewBag.gallryid = new SelectList(db.gallries, "id", "gallryname", product.gallryid);
            return View(product);
        }

        // GET: products1/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product product = db.products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: products1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, product productdel)
        {
            var a = db.productimages.Where(p => p.productid == id).ToList();
            foreach (var item in a)
            {
                string old_bath1 = Path.Combine(Server.MapPath("~/uploads"), item.imagename);
                System.IO.File.Delete(old_bath1);
            }
            string old_bath = Path.Combine(Server.MapPath("~/uploads"), productdel.productimg);
            System.IO.File.Delete(old_bath);

            product product = db.products.Find(id);
            db.products.Remove(product);
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
