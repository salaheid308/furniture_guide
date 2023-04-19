using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dalelk.Models;

namespace Dalelk.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult index()
        {
            ViewBag.filt = db.categories.Where(x => x.city.cityname == "دمياط").ToList();
            ViewBag.city = db.cities.ToList();
            
            ViewBag.products = db.products.ToList();
            ViewBag.data = db.welcomepages.ToList();
            return View(db.categories.ToList());
        }

        public ActionResult categorycontent(int? id ,int?cid , string searshbox, string idd ,int? filpric)
        {
            if (id == null || cid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           
            category category = db.categories.Find(id);
            city city = db.cities.Find(cid);
            ViewBag.categotydata = db.categories.Where(e=>e.cityid == cid);
            var proincat = db.products.Where(cat => cat.categoryid == id);

            //ViewBag.colordata = proincat.Select(p => p.color).Distinct();
            ViewBag.color2data = db.productcolor2.Where(x => x.categoryid == id).Select(p => p.color2name).Distinct();

            ViewBag.colordata = db.productcolor.Where(x => x.categoryid == id).Select(p => p.colorname).Distinct();
            ViewBag.color2data = db.productcolor2.Where(x => x.categoryid == id).Select(p => p.color2name).Distinct();
            ViewBag.price = db.productprice.Where(x => x.categoryid == id).Select(p => p.price).Distinct();
            ViewBag.sizedata = proincat.Select(o=>o.productsize).Distinct();
            
            if (category == null|| city==null)
            {
                return HttpNotFound();
            }
            if (searshbox != null)
            {
                var result3 = db.products.Where(l => l.categoryid == id);
                var result4 = result3.Where(a => a.productname.Contains(searshbox)
                
                || a.productcolor.Contains(searshbox)
                || a.productcolor2.Contains(searshbox)
                || a.productsize.Contains(searshbox)).ToList();
                ViewBag.categotydata = db.categories.Where(e => e.cityid == cid);
                ViewBag.mov = result4;
            }
            if (idd != null || filpric !=null)
            {
                var result3 = db.products.Where(l => l.categoryid == id);
                var result4 = result3.Where(a => a.productsize == idd
                ||a.productprice == filpric
                || a.productcolor2 == idd
                || a.productcolor==idd).ToList();
                ViewBag.categotydata = db.categories.Where(e => e.cityid == cid);
                ViewBag.mov = result4;
            }


            return View(category);
            
        }

        public ActionResult gallrycontent(int? id, int? cid, string searshbox, string idd, int? filpric)
        {
            if (id == null || cid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            gallries gallry = db.gallries.Find(id);
            city city = db.cities.Find(cid);

            ViewBag.gallrydata = db.gallries.Where(e => e.cityid == cid);
            ViewBag.catodata = db.categories.Where(o => o.cityid == cid);
            ViewBag.colordata = db.productcolor.Where(x => x.gallryid == id).Select(p => p.colorname).Distinct();
            ViewBag.color2data = db.productcolor2.Where(x => x.gallryid == id).Select(p => p.color2name).Distinct();
            ViewBag.pricedata = db.productprice.Where(x => x.gallryid == id).Select(o => o.price).Distinct().ToList();
            ViewBag.sizedata = db.productsize.Where(x => x.gallryid == id).Select(o => o.size).Distinct();

            if (gallry == null || city == null)
            {
                return HttpNotFound();
            }
            if (searshbox != null)
            {
                var result3 = db.products.Where(l => l.gallryid == id);
                var result4 = result3.Where(a => a.productname.Contains(searshbox)

                || a.productcolor.Contains(searshbox)
                || a.productcolor2.Contains(searshbox)
                || a.productsize.Contains(searshbox)).ToList();
                ViewBag.gallrydata = db.gallries.Where(e => e.cityid == cid);
                ViewBag.mov = result4;
            }
            if (idd != null || filpric != null)
            {
                var result3 = db.products.Where(l => l.gallryid == id);
                var result4 = result3.Where(a => a.productsize == idd
                || a.productprice == filpric
                || a.productcolor2 == idd
                || a.productcolor == idd).ToList();
                ViewBag.gallrydata = db.gallries.Where(e => e.cityid == cid);
                ViewBag.mov = result4;
            }


            return View(gallry);
           



        }

        public ActionResult citycontent(int? id, string searshbox, string idd, string ssbox, int? filpric)
        {
            
            ViewBag.productdata = db.products.Where(x => x.category.city.cityname.Contains(ssbox) || x.category.cityid == id).ToList();
           
            if (id != null)
            {
                ViewBag.categotydata = db.categories.Where(x => x.cityid == id);
                ViewBag.gallrydata = db.gallries.Where(x => x.cityid == id);
                ViewBag.colordata = db.productcolor.Where(x => x.category.cityid == id).Select(o=>o.colorname).Distinct();
                ViewBag.color2data = db.productcolor2.Where(x => x.category.cityid == id).Select(o=>o.color2name).Distinct();
                ViewBag.pricedata = db.productprice.Where(x => x.category.cityid == id).Select(o => o.price).Distinct().ToList();
                ViewBag.sizedata = db.productsize.Where(x => x.category.cityid == id).Select(i=>i.size).Distinct();
               
                ViewBag.id = id;
                if (searshbox != null)
                {
                    var result3 = db.products.Where(l => l.category.cityid == id);
                    var result4 = result3.Where(a => a.productname.Contains(searshbox)
                    
                    || a.productcolor.Contains(searshbox)
                    || a.productcolor2.Contains(searshbox)
                    || a.productsize.Contains(searshbox)).ToList();
                    ViewBag.mov = result4;
                }
                if (idd != null || filpric != null)
                {
                    var result3 = db.products.Where(l => l.category.cityid == id);
                    var result4 = result3.Where(a => a.productsize == idd
                    ||a.productprice == filpric
                    || a.productcolor2 == idd
                    || a.productcolor == idd).ToList();
                    ViewBag.mov = result4;
                }
                return View(db.cities.Find(id));
            }


            if (ssbox != null)
            {
                city city = db.cities.FirstOrDefault(x => x.cityname.Contains(ssbox));
                if (city !=null)
                {
                    ViewBag.id = city.id;
                    var fondcityid = city.id;
                    ViewBag.categotydata = db.categories.Where(x => x.cityid == fondcityid);
                    ViewBag.gallrydata = db.gallries.Where(x => x.cityid == fondcityid);
                    ViewBag.colordata = db.productcolor.Where(x => x.category.cityid == fondcityid);
                    ViewBag.color2data = db.productcolor2.Where(x => x.category.cityid == fondcityid);
                    ViewBag.pricedata = db.productprice.Where(x => x.category.cityid == fondcityid);
                    ViewBag.sizedata = db.productsize.Where(x => x.category.cityid == fondcityid);
                   


                    if (searshbox != null)
                    {
                        var result3 = db.products.Where(l => l.category.cityid == fondcityid);
                        var result4 = result3.Where(a => a.productname.Contains(searshbox)
                        
                        || a.productcolor.Contains(searshbox)
                        || a.productcolor2.Contains(searshbox)
                        || a.productsize.Contains(searshbox)).ToList();
                        ViewBag.mov = result4;


                    }
                    if (idd != null|| filpric!=null)
                    {
                        var result3 = db.products.Where(l => l.category.cityid == fondcityid);
                        var result4 = result3.Where(a => a.productsize == idd
                        ||a.productprice == filpric
                        || a.productcolor2 == idd
                        || a.productcolor == idd).ToList();
                        ViewBag.mov = result4;

                    }
                    return View(city);
                }
                else
                {
                    return HttpNotFound();
                }
                
            }
            return View();
        } 

        public ActionResult plogs(string searshbox)
        {
            if (searshbox != null)
            {
                

                var userplo = db.plogs.ToList();
                var userplogs = userplo.Where(a => a.content.Contains(searshbox) || a.title.Contains(searshbox)).ToList();
                return View(userplogs);
            }
            var plogs = db.plogs.ToList(); /*Include(p => p.user)*/
            return View(plogs);
        }

        public ActionResult plogdetails(int? id)
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

            var cid = plogs.id ;
            var npo = db.plogs.Find(cid + 1);
            var rpo = db.plogs.Find(cid - 1);
            if (npo !=null)
            {
                ViewBag.ntitle = db.plogs.Find(cid + 1);
            }
            if (rpo!=null)
            {
                ViewBag.ptitle = db.plogs.Find(cid - 1);
            }
            
            return View(plogs);
        }

        
        public ActionResult driverslist(string searshbox, int? id)
        {
            if (id == 0)
            {
                ViewBag.id = 0;
                var shipdriv = db.drivers.Where(q => q.drivertype == "مندوب شحن");

                if (searshbox != null)
                {
                    var wantdri = shipdriv.Where(a => a.carname.Contains(searshbox) || a.username.Contains(searshbox) || a.city.cityname.Contains(searshbox)).ToList();

                    ViewBag.filcity = wantdri.Select(x => x.city.cityname).Distinct();

                    ViewBag.filacr = wantdri.Select(x => x.cartype).Distinct();
                    return View(wantdri);
                }
                ViewBag.filcity = shipdriv.ToList().Select(x => x.city.cityname).Distinct();

                ViewBag.filacr = shipdriv.ToList().Select(x => x.cartype).Distinct();
                return View(shipdriv.ToList());

            }

            if (id == 1)
            {
                ViewBag.id = 1;
                var humndriv = db.drivers.Where(q => q.drivertype == "مندوب توصيل");
                if (searshbox != null)
                {
                    var wantdri = humndriv.Where(a => a.carname.Contains(searshbox) || a.username.Contains(searshbox) || a.city.cityname.Contains(searshbox)).ToList();
                    ViewBag.filcity = wantdri.Select(x => x.city.cityname).Distinct();

                    ViewBag.filacr = wantdri.Select(x => x.cartype).Distinct();
                    return View(wantdri);
                }
                ViewBag.filcity = humndriv.ToList().Select(x => x.city.cityname).Distinct();

                ViewBag.filacr = humndriv.ToList().Select(x => x.cartype).Distinct();
                return View(humndriv.ToList());
            }
            ViewBag.filcity = db.drivers.ToList().Select(x => x.city.cityname).Distinct();

            ViewBag.filacr = db.drivers.ToList().Select(x => x.cartype).Distinct();
            return View(db.drivers.ToList());
        }
        public ActionResult driverdetails(int? id ,string myprofile )
        {
            if (id == null &&myprofile ==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            drivers drivers = db.drivers.Find(id);
            if (drivers == null && myprofile == null)
            {
                return HttpNotFound();
            }
            var samecity = from y in db.cities join p in db.driverprices on y.id equals p.cityid where p.driverid == id select y;
           
            var citynorepeat = db.cities.Except(samecity).ToList();
            ViewBag.cityli = citynorepeat;

            if (myprofile !=null)
            {
                drivers div = db.drivers.First(p => p.userid == myprofile);
                var cityprice = db.driverprices.Where(w => w.driverid == div.id).ToList();

                 samecity = from y in db.cities join p in db.driverprices on  y.id  equals p.cityid where p.driverid == div.id  select y;
                 citynorepeat =  db.cities.Except(samecity).ToList();
                ViewBag.cityli = citynorepeat;
                var drivrs =  db.drivers.First(r => r.userid == myprofile);
                return View(drivrs);
            }
            
               var  citypricee = db.driverprices.Where(w => w.driverid == drivers.id).ToList() ;
              return View(drivers);
        }

        public ActionResult productdetails(int?  id)
        {

          product  pro =   db.products.Find(id);
            if (pro == null || id == null)
            {
                return HttpNotFound();
            }
            var fondrate = db.productrates.Where(r => r.productid == id).Count();
            var star5 = db.productrates.Where(r => r.rate == 5 && r.productid == id).Count();
            var star4 = db.productrates.Where(r => r.rate == 4 && r.productid == id).Count();
            var star3 = db.productrates.Where(r => r.rate == 3 && r.productid == id).Count();
            var star2 = db.productrates.Where(r => r.rate == 2 && r.productid == id).Count();
            var star1 = db.productrates.Where(r => r.rate == 1 && r.productid == id).Count();
            
                ViewBag.star5percent = 0;
                ViewBag.star4percent = 0;
                ViewBag.star3percent = 0;
                ViewBag.star2percent = 0;
                ViewBag.star1percent = 0;
            
            if (fondrate != 0)
            {
                ViewBag.star5percent = (int)Math.Round(((double)star5 / (double)fondrate) * 100);
                ViewBag.star4percent = (int)Math.Round(((double)star4 / (double)fondrate) * 100);
                ViewBag.star3percent = (int)Math.Round(((double)star3 / (double)fondrate) * 100);
                ViewBag.star2percent = (int)Math.Round(((double)star2 / (double)fondrate) * 100);
                ViewBag.star1percent = (int)Math.Round(((double)star1 / (double)fondrate) * 100);
            }
            ViewBag.total = fondrate;
            return View(pro);

        }


        public ActionResult cart()
        {
            return View();
        }

        public ActionResult checkout()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        // GET: categories/Details/5
    }
}
