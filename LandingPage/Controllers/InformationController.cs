using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.EnterpriseServices;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LandingPage.Models;
using PagedList;

namespace LandingPage.Controllers
{
    public class InformationController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Information
        [Authorize(Roles = "Admin")]
        public ActionResult Index(string sortOrder, int? status, DateTime? start, DateTime? end, string searchString, string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            var infor = from s in db.Information select s;
            if (searchString != null)
            {
                page = 1;
            }

            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;
         //   ViewBag.PageSize = new List<SelectListItem>()
         //{
             
         //    new SelectListItem() { Value="10", Text= "10" },
         //    new SelectListItem() { Value="15", Text= "15" },
         //    new SelectListItem() { Value="25", Text= "25" },
         //    new SelectListItem() { Value="50", Text= "50" },
         //};
            if (!String.IsNullOrEmpty(searchString))
            {
                infor = infor.Where(s => s.Name.Contains(searchString) || s.Email.Contains(searchString));
            }
            if (status.HasValue)
            {
                ViewBag.Status = status;
                infor = infor.Where(p => (int)p.Status == status.Value);

            }
            if (start != null)
            {
                var startDate = start.GetValueOrDefault().Date;
                startDate = startDate.Date + new TimeSpan(0, 0, 0);
                infor = infor.Where(p => p.CreateAt >= startDate);
            }
            if (end != null)
            {
                var endDate = end.GetValueOrDefault().Date;
                endDate = endDate.Date + new TimeSpan(23, 59, 59);
                infor = infor.Where(p => p.CreateAt <= endDate);
            }
            if (string.IsNullOrEmpty(sortOrder) || sortOrder.Equals("date-asc"))

            {
                ViewBag.DateSort = "date-desc";
                ViewBag.SortIcon = "fa fa-sort-asc";
            }
            else if (sortOrder.Equals("date-desc"))
            {
                ViewBag.DateSort = "date-asc";
                ViewBag.SortIcon = "fa fa-sort-desc";
            }
            switch (sortOrder)
            {
                
                case "date-asc":
                    infor = infor.OrderBy(p => p.CreateAt);
                    break;
                case "date-desc":
                    infor = infor.OrderByDescending(p => p.CreateAt);
                    break;
                
                default:
                    infor = infor.OrderByDescending(p => p.CreateAt);
                    ViewBag.SortIcon = "fa fa-sort";
                    break;
            }
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            return View(infor.ToPagedList(pageNumber, pageSize));
        }

        // GET: Information/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Information information = db.Information.Find(id);
            if (information == null)
            {
                return HttpNotFound();
            }
            return View(information);
        }

        // GET: Information/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Information/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Email,Phone,Message,Status,CreateAt")] Information information)
        {
            if (ModelState.IsValid)
            {
                db.Information.Add(information);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(information);
        }

        // GET: Information/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Information information = db.Information.Find(id);
            if (information == null)
            {
                return HttpNotFound();
            }
            return View(information);
        }

        // POST: Information/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Email,Phone,Message,Status,CreateAt")] Information information)
        {
            if (ModelState.IsValid)
            {
                db.Entry(information).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(information);
        }

        // GET: Information/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Information information = db.Information.Find(id);
            if (information == null)
            {
                return HttpNotFound();
            }
            return View(information);
        }

        // POST: Information/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Information information = db.Information.Find(id);
            db.Information.Remove(information);
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
