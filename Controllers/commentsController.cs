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
    public class commentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
       
        // GET: comments
        public ActionResult Index()
        {
            var comments = db.comments.Include(c => c.plog).Include(c => c.user);
            return View(comments.ToList());
        }

        // GET: comments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            comments comments = db.comments.Find(id);
            if (comments == null)
            {
                return HttpNotFound();
            }
            return View(comments);
        }

        // GET: comments/Create
        public ActionResult Create(int id)
        {

            return RedirectToAction("plogdetails", "Home", new { id = id });
        }

        // POST: comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(plogs plog , string com )
        {
            if (com != "" )
            {
                var userid = User.Identity.GetUserId();
                var currentuser = db.Users.Where(a => a.Id == userid).SingleOrDefault();
                var usedplog = db.plogs.Find(plog.id);
                

                var ncoment = new comments();
                ncoment.plogid = plog.id;
                ncoment.usercomment = com;
                ncoment.userid = currentuser.Id;
                ncoment.datetime = DateTime.Now;
                db.comments.Add(ncoment);
                db.SaveChanges();


                if (ncoment.userid != usedplog.userid)
                {
                    notification nempnoti = new notification();
                    nempnoti.count = 1;
                    nempnoti.notify = usedplog.title +" "+ "علي مقالك" + " " +currentuser.UserName + " " + "علق";
                    nempnoti.userid = usedplog.userid;
                    nempnoti.plogid = usedplog.id;
                    nempnoti.username = currentuser.UserName;
                    db.notifications.Add(nempnoti);
                    db.SaveChanges();
                }
                return RedirectToAction("plogdetails", "Home", new { id = plog.id });
            }
            
                return RedirectToAction("plogdetails", "Home", new { id = plog.id });
            
            
        }

        // GET: comments/Edit/5
        public ActionResult Edit(int? id)
        {
            var userid = User.Identity.GetUserId();
            var currentuser = db.Users.Where(a => a.Id == userid).SingleOrDefault();
            var cuurcomment = db.comments.First(q => q.userid == currentuser.Id);
            if (cuurcomment != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                comments comments = db.comments.Find(id);
                if (comments == null)
                {
                    return HttpNotFound();
                }

                return View(comments);
            }
            return HttpNotFound();
        }

        // POST: comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(comments comments)
        {
            if (comments.usercomment !=null )
            {
                comments.datetime = DateTime.Now;
                db.Entry(comments).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("plogdetails", "Home", new { id = comments.plogid });
            }

            return RedirectToAction("plogdetails", "Home", new { id = comments.plogid });
        }

        // GET: comments/Delete/5
        public ActionResult Delete(int? id)
        {
            var userid = User.Identity.GetUserId();
            var currentuser = db.Users.Where(a => a.Id == userid).SingleOrDefault();
            var cuurcomment = db.comments.First(q => q.userid == currentuser.Id);
            if (cuurcomment!=null)
            {

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                comments comments = db.comments.Find(id);
                if (comments == null)
                {
                    return HttpNotFound();
                }
                return View(comments);
            }
            return HttpNotFound();
        }

        // POST: comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            
            comments comments = db.comments.Find(id);
            db.comments.Remove(comments);
            db.SaveChanges();

            return RedirectToAction("plogdetails", "Home", new { id = comments.plogid });


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
