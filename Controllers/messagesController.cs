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
    public class messagesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: messages
        [Authorize]
        public ActionResult mymessages(string mymsg , string id)
        {

            var curuserid = User.Identity.GetUserId();
            var currentuser = db.Users.Where(a => a.Id == curuserid).SingleOrDefault();
            if (mymsg !=null )
            {
                var messages = db.messages.Where(y => y.userid == mymsg || y.usersendid == mymsg );
                
                return View(messages.ToList());
            }
            if (id != null )
            {
                var msgfound = db.messages.Where(o => o.usersendid == id).ToList();
                foreach (var item in msgfound)
                {
                    item.count = 0;
                    db.Entry(item).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("mymessages", "messages", new { mymsg = currentuser.Id});
            }
            return HttpNotFound();
        }

        // GET: messages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            messages messages = db.messages.Find(id);
            if (messages == null)
            {
                return HttpNotFound();
            }
            return View(messages);
        }

        // GET: messages/Create
        [Authorize]
        public ActionResult Create(int? driverid, int? galid)
        {
            if (driverid != null)
            {
                return RedirectToAction("driverdetails", "Home", new { id = driverid });
            }
            if (galid != null)
            {
                return RedirectToAction("userdetails", "gallries", new { id = galid });
            }

            return HttpNotFound();
        }

        // POST: messages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int? id , string msg , string userid, string drivertype ,string gallryname)
        {
            var curuserid = User.Identity.GetUserId();
            var currentuser = db.Users.Where(a => a.Id == curuserid).SingleOrDefault();
            if (drivertype != null)
            {
                messages newmessage = new messages();
                newmessage.count = 1;
                newmessage.datetim = DateTime.Now;
                newmessage.sendermessage = msg;
                newmessage.userid = userid;
                newmessage.username = currentuser.UserName;
                newmessage.usersendid = currentuser.Id;
                db.messages.Add(newmessage);
                db.SaveChanges();
                return RedirectToAction("driverdetails", "Home", new { id = id });
            }
            else if (gallryname != null)
            {

                messages newmessage = new messages();
                newmessage.count = 1;
                newmessage.datetim = DateTime.Now;
                newmessage.sendermessage = msg;
                newmessage.userid = userid;
                newmessage.username = currentuser.UserName;
                newmessage.usersendid = currentuser.Id;
                db.messages.Add(newmessage);
                db.SaveChanges();
                return RedirectToAction("userdetails", "gallries", new { id = id });
            }
            else
            {
                messages newmessage = new messages();
                newmessage.count = 1;
                newmessage.datetim = DateTime.Now;
                newmessage.sendermessage = msg;
                newmessage.userid = userid;
                newmessage.username = currentuser.UserName;
                newmessage.usersendid = currentuser.Id;
                db.messages.Add(newmessage);
                db.SaveChanges();
                return RedirectToAction("mymessages", "messages", new { mymsg = currentuser.Id });
            }
           
        }

        // GET: messages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            messages messages = db.messages.Find(id);
            if (messages == null)
            {
                return HttpNotFound();
            }

            return View(messages);
        }

        // POST: messages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,userid,count,sendermessage,recivemessage,usersendid,username")] messages messages)
        {
            if (ModelState.IsValid)
            {
                db.Entry(messages).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(messages);
        }

        // GET: messages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            messages messages = db.messages.Find(id);
            if (messages == null)
            {
                return HttpNotFound();
            }
            return View(messages);
        }

        // POST: messages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            messages messages = db.messages.Find(id);
            db.messages.Remove(messages);
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
