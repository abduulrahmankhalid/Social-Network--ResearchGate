using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ResearchGate.Models;

namespace ResearchGate.Controllers
{
    public class TagsController : Controller
    {
        private DBEntities db = new DBEntities();

        // GET: Tags
        public ActionResult Index()
        {
            var tags = db.Tags.Include(t => t.Author).Include(t => t.Paper);
            return View(tags.ToList());
        }

        // GET: Tags/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tag tag = db.Tags.Find(id);
            if (tag == null)
            {
                return HttpNotFound();
            }
            return View(tag);
        }

        // GET: Tags/Create
      
        public ActionResult Create(int? id)
        {
            ViewBag.AuthID = new SelectList(db.Authors, "AuthorID", "Email");
            return View();
        }

        // POST: Tags/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TagID,AuthID,PapID")] Tag tag,int? id)
        {
            tag.PapID = id;
            if (ModelState.IsValid)
            {
                db.Tags.Add(tag);
                db.SaveChanges();
                return RedirectToAction("Details","Papers",new { id=id});
            }

            ViewBag.AuthID = new SelectList(db.Authors, "AuthorID", "Email", tag.AuthID);
            ViewBag.PapID = new SelectList(db.Papers, "PaperID", "Title", tag.PapID);
            return View(tag);
        }

        // GET: Tags/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tag tag = db.Tags.Find(id);
            if (tag == null)
            {
                return HttpNotFound();
            }
            ViewBag.AuthID = new SelectList(db.Authors, "AuthorID", "Email", tag.AuthID);
            ViewBag.PapID = new SelectList(db.Papers, "PaperID", "Title", tag.PapID);
            return View(tag);
        }

        // POST: Tags/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TagID,AuthID,PapID")] Tag tag)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tag).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AuthID = new SelectList(db.Authors, "AuthorID", "Email", tag.AuthID);
            ViewBag.PapID = new SelectList(db.Papers, "PaperID", "Title", tag.PapID);
            return View(tag);
        }

        // GET: Tags/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tag tag = db.Tags.Find(id);
            if (tag == null)
            {
                return HttpNotFound();
            }
            return View(tag);
        }

        // POST: Tags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tag tag = db.Tags.Find(id);
            db.Tags.Remove(tag);
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
