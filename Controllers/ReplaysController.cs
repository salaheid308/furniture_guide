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
    public class ReplaysController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Replays
        public ActionResult Index()
        {
            var replays = db.Replays.Include(r => r.comment);
            return View(replays.ToList());
        }

        // GET: Replays/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Replay replay = db.Replays.Find(id);
            if (replay == null)
            {
                return HttpNotFound();
            }
            return View(replay);
        }

        // GET: Replays/Create
        public ActionResult Create(int id)
        {
            return RedirectToAction("plogdetails", "Home", new { id = id });
        }

        // POST: Replays/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( int comentid , plogs plog, string rep)
        {
            if (rep !="")
            {
                var userid = User.Identity.GetUserId();
                var currentuser = db.Users.Where(a => a.Id == userid).SingleOrDefault();
                var currentcoment = db.comments.Find(comentid);
                var currntplog = db.plogs.Find(plog.id);
                var relaterep = db.Replays.Where(q => q.commentid == comentid ).ToList();
                //var result = listA.Union(listB).ToList();
                var v = relaterep.Where(p => relaterep.All(p2 => p2.userid != p.userid)).ToList();
                var w = relaterep.Where(p => relaterep.All(p2 => p2.userid == p.userid)).ToList();
                     v.AddRange(w) ;

                Replay nreply = new Replay();
                nreply.plogid = plog.id;
                nreply.commentid = comentid;
                nreply.userreplay = rep;
                nreply.userid = currentuser.Id;
                nreply.datetime = DateTime.Now;
                db.Replays.Add(nreply);
                db.SaveChanges();
                
                if (nreply.userid != currentcoment.userid )
                {
                    notification nempnoti = new notification();
                    nempnoti.count = 1;
                    nempnoti.notify = "رد" + " " + currentuser.UserName + " " + "علي تعليقك في مقال" + " " + currntplog.title+" "+ "كومنت تاريخه يوم " + currentcoment.datetime.Day;
                    nempnoti.userid = currentcoment.userid;
                    nempnoti.plogid = plog.id;
                    nempnoti.username = currentuser.UserName;
                    db.notifications.Add(nempnoti);
                    db.SaveChanges();
                }
                
                    foreach (var item in v)
                    {
                            
                            if (item.userid != nreply.userid)
                            {
                               
                                notification nempnoti = new notification();
                                nempnoti.count = 1;
                                nempnoti.notify = currntplog.title + " " + "علي ردك في مقال بعنوان" + " " + item.user.UserName + " " + "رد";
                                nempnoti.notify = "رد" + " " + currentuser.UserName + " " + "علي ردك في مقال" + " " + currntplog.title +" " +"كومنت تاريخه يوم " + currentcoment.datetime.Day;
                                nempnoti.userid = item.userid;
                                nempnoti.plogid = plog.id;
                                nempnoti.username = item.user.UserName;
                                db.notifications.Add(nempnoti);
                                db.SaveChanges();
                            }
                    }
                
                return RedirectToAction("plogdetails", "Home", new { id = plog.id });
            }

            return RedirectToAction("plogdetails", "Home", new { id = plog.id });

        }

        // GET: Replays/Edit/5
        public ActionResult Edit(int? id)
        {
            var userid = User.Identity.GetUserId();
            var currentuser = db.Users.Where(a => a.Id == userid).SingleOrDefault();
            var cuurrep = db.Replays.First(q => q.userid == currentuser.Id);
            if (cuurrep != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Replay reply = db.Replays.Find(id);
                if (reply == null)
                {
                    return HttpNotFound();
                }

                return View(reply);
            }
            return HttpNotFound();
        }

        // POST: Replays/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Replay replay )
        {
            
            if (replay.userreplay != null)
            {
                replay.datetime = DateTime.Now;
                db.Entry(replay).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("plogdetails", "Home", new { id = replay.plogid});
            }

            return RedirectToAction("plogdetails", "Home", new { id = replay.plogid});
        }

        // GET: Replays/Delete/5
        public ActionResult Delete(int? id)
        {
            var userid = User.Identity.GetUserId();
            var currentuser = db.Users.Where(a => a.Id == userid).SingleOrDefault();
            var currep = db.Replays.First(q => q.userid == currentuser.Id);
            if (currep != null)
            {

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Replay rep = db.Replays.Find(id);
                if (rep == null)
                {
                    return HttpNotFound();
                }
                return View(rep);
            }
            return HttpNotFound();
        }

        // POST: Replays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Replay replay = db.Replays.Find(id);
            db.Replays.Remove(replay);
            db.SaveChanges();
            return RedirectToAction("plogdetails", "Home", new { id = replay.plogid });
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
