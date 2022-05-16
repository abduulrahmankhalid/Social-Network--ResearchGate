using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ResearchGate.Infrastructure;
using ResearchGate.Models;


namespace ResearchGate.Controllers
{
    public class CommentsController : Controller
    {

        private DBEntities db = new DBEntities();


        // GET: Comments
        [CustomAuthenticationFilter]
        public ActionResult Index()
        {
            var comments = db.Comments.Include(c => c.Author).Include(c => c.Paper);
            return View(comments.ToList());
        }


        [CustomAuthenticationFilter]
        public ActionResult ShowMyPaperComments(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var PaperComments = db.Comments.Where(x => x.PapID == id).ToList();

            if (PaperComments.Count == 0)
            {
                return View("NotFound");
            }

            return View(PaperComments);
        }



        // GET: Comments/Details/5
        [CustomAuthenticationFilter]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Comment comment = db.Comments.Find(id);

            if (comment == null)
            {
                return HttpNotFound();
            }

            return View(comment);
        }

        // GET: Comments/Create
        [CustomAuthenticationFilter]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Comments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Comment comment , int? id)
        {
            comment.PapID = id;

            comment.AuthID = (int)Session["AuthID"];

            if (ModelState.IsValid)
            {
                db.Comments.Add(comment);
                db.SaveChanges();
                return RedirectToAction("Details" , "Papers" , new { id = comment.PapID });
            }

            ViewBag.AuthID = new SelectList(db.Authors, "AuthorID", "Email", comment.AuthID);
            ViewBag.PapID = new SelectList(db.Papers, "PaperID", "Title", comment.PapID);

            return View(comment);
        }


        // GET: Comments/Edit/5
        [CustomAuthenticationFilter]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Comment comment = db.Comments.Find(id);

            if (comment == null)
            {
                return HttpNotFound();
            }

            ViewBag.AuthID = new SelectList(db.Authors, "AuthorID", "Email", comment.AuthID);
            ViewBag.PapID = new SelectList(db.Papers, "PaperID", "Title", comment.PapID);

            return View(comment);
        }

        // POST: Comments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CommID,AuthID,PapID,Commnt")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AuthID = new SelectList(db.Authors, "AuthorID", "Email", comment.AuthID);
            ViewBag.PapID = new SelectList(db.Papers, "PaperID", "Title", comment.PapID);
            return View(comment);
        }



        // GET: Comments/Delete/5
        [CustomAuthenticationFilter]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Comment comment = db.Comments.Find(id);

            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
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
