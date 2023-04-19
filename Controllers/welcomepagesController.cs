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
    public class welcomepagesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: welcomepages
        public ActionResult Index()
        {
            return View(db.welcomepages.ToList());
        }

        // GET: welcomepages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            welcomepage welcomepage = db.welcomepages.Find(id);
            if (welcomepage == null)
            {
                return HttpNotFound();
            }
            return View(welcomepage);
        }

        // GET: welcomepages/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: welcomepages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HttpPostedFileBase upload, welcomepage welcomepage)
        {
            if (ModelState.IsValid)
            {
                string name = Guid.NewGuid() + Path.GetExtension(upload.FileName);
                string bath = Path.Combine(Server.MapPath("~/welcomeimgs"), name);
                upload.SaveAs(bath);
                welcomepage.backgroundtimg = name;
                db.welcomepages.Add(welcomepage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(welcomepage);
        }

        // GET: welcomepages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            welcomepage welcomepage = db.welcomepages.Find(id);
            if (welcomepage == null)
            {
                return HttpNotFound();
            }
            return View(welcomepage);
        }

        // POST: welcomepages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HttpPostedFileBase files,  welcomepage welcomepage)
        {
            if (ModelState.IsValid)
            {
                string old_bath = Path.Combine(Server.MapPath("~/welcomeimgs"), welcomepage.backgroundtimg );
                if (files != null)
                {

                    System.IO.File.Delete(old_bath);

                    string name = Guid.NewGuid() + Path.GetExtension(files.FileName);
                    string bath = Path.Combine(Server.MapPath("~/welcomeimgs"), name);
                    files.SaveAs(bath);
                    welcomepage.backgroundtimg = name;

                }
                db.Entry(welcomepage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(welcomepage);
        }

        // GET: welcomepages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            welcomepage welcomepage = db.welcomepages.Find(id);
            if (welcomepage == null)
            {
                return HttpNotFound();
            }
            return View(welcomepage);
        }

        // POST: welcomepages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, welcomepage welcomepagedel)
        {
            string old_bath = Path.Combine(Server.MapPath("~/welcomeimgs"), welcomepagedel.backgroundtimg);
            System.IO.File.Delete(old_bath);
            welcomepage welcomepage = db.welcomepages.Find(id);
            db.welcomepages.Remove(welcomepage);
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
