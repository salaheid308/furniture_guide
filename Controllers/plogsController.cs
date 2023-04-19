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
    public class plogsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: plogs
        public ApplicationUser getuser()
        {
           
            var userid = User.Identity.GetUserId();
            return db.Users.First(a => a.Id == userid);
        }
        public ActionResult Index(string myplogs ,string searshbox)
        {

            if (myplogs != null)
            {

                var founded = db.Users.Find(myplogs);
                if (founded == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    var userplogs = db.plogs.Where(a => a.userid == myplogs).ToList();
                    return View(userplogs);
                }
            }
            if (searshbox !=null)
            {
                var cuurentid = getuser().Id;
                var userplo = db.plogs.Where(q => q.userid == cuurentid).ToList();
                var userplogs = userplo.Where(a => a.content.Contains(searshbox)||a.title.Contains(searshbox)).ToList();
                return View(userplogs);
            }
            var cuurentid2 = getuser().Id;
            var userplo2 = db.plogs.Where(q => q.userid == cuurentid2).ToList();
            return View(userplo2);
        }

        // GET: plogs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            plogs plogs = db.plogs.Find(id);
            if (plogs == null)
            {
                return HttpNotFound();
            }
            return View(plogs);
        }

        // GET: plogs/Create
        public ActionResult Create()
        {
            
            return View();
        }

        // POST: plogs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(plogs plogs , HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                var cuurentid = getuser().Id;
                string name = "splash_image.png";
                if (upload !=null)
                {


                    name = Guid.NewGuid() + Path.GetExtension(upload.FileName);
                    string bath = Path.Combine(Server.MapPath("~/plogsimages"), name);
                    upload.SaveAs(bath);
                    
                }
                
                plogs.userid = cuurentid;
                plogs.image = name;
                plogs.datetime = DateTime.Now;
                db.plogs.Add(plogs);
                db.SaveChanges();
                return RedirectToAction("Index",new { myplogs = cuurentid });
            }

            
            return View(plogs);
        }

        // GET: plogs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            plogs plogs = db.plogs.Find(id);
            if (plogs == null)
            {
                return HttpNotFound();
            }
           
            return View(plogs);
        }

        // POST: plogs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(plogs plogs)
        {
            if (ModelState.IsValid)
            {
                db.Entry(plogs).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            return View(plogs);
        }

        // GET: plogs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            plogs plogs = db.plogs.Find(id);
            if (plogs == null)
            {
                return HttpNotFound();
            }
            return View(plogs);
        }

        // POST: plogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            
            plogs plogs = db.plogs.Find(id);
            if (plogs !=null)
            {
                var cuurentid = getuser().Id;
                if (plogs.image != null && plogs.image != "splash_image.png")
                {
                    string old_bath = Path.Combine(Server.MapPath("~/plogsimages"), plogs.image);
                    System.IO.File.Delete(old_bath);
                }
                var ui = db.notifications.Where(t => t.plogid == id);
                db.notifications.RemoveRange(ui);
              
                db.plogs.Remove(plogs);
                db.SaveChanges();
                return RedirectToAction("Index", new { myplogs = cuurentid });
            }
            return HttpNotFound();
           
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
