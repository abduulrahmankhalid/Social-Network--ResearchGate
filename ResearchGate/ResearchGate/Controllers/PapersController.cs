using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ResearchGate.Models;

using System.IO;


using Microsoft.AspNet.Identity;


namespace ResearchGate.Controllers
{
    public class PapersController : Controller
    {
        private DBEntities db = new DBEntities();

        // GET: Papers
        public ActionResult Index()
        {
            return View(db.Papers.ToList());
        }

        // GET: Papers/Details/5
        public ActionResult PaperPage()
        {
            return View();
        }

        public ActionResult Like(int? id)
        {
            Paper paper = db.Papers.ToList().Find(x => x.PaperID == id);

            paper.Likes += 1;

            db.Entry(paper).State = EntityState.Modified;

            db.SaveChanges();

            return RedirectToAction("Details", new { id = paper.PaperID });
        }

        public ActionResult Dislike(int? id)
        {
            Paper paper = db.Papers.ToList().Find(x => x.PaperID == id);

            paper.Dislikes += 1;

            db.Entry(paper).State = EntityState.Modified;

            db.SaveChanges();

            return RedirectToAction("Details", new { id = paper.PaperID });
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Paper paper = db.Papers.Find(id);
            if (paper == null)
            {
                return HttpNotFound();
            }
            return View(paper);
        }
        public ActionResult MyPaper_ID(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(db.Tags.Where(x => x.PapID == id).ToList());
            
        }

        // GET: Papers/Create
        public ActionResult Create()
        {
            return View();

        }

        // POST: Papers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PaperID,Title,Date,Abstract,Image,Likes,Dislikes")] Paper paper, HttpPostedFileBase PapImgFile)
        {
            string path = "";
            if (PapImgFile != null)
            {
                path = "~/Content/PaperImages/" + Path.GetFileName(PapImgFile.FileName);
                PapImgFile.SaveAs(Server.MapPath(path));
            }
            else
            {
                path = "~/Content/PaperImages/paper_default_img.jpg";
            }
            paper.Image = path;



            if (ModelState.IsValid)
            {
                //paper.AuthorID = User.Identity.GetUserId().ToString();
                db.Papers.Add(paper);
                db.SaveChanges();
                return RedirectToAction("Create","Tags",new {id=paper.PaperID });
            }



            return View(paper);
        }

        // GET: Papers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Paper paper = db.Papers.Find(id);
            if (paper == null)
            {
                return HttpNotFound();
            }
            return View(paper);
        }

        // POST: Papers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PaperID,Title,Date,Abstract,Image,Likes,Dislikes")] Paper paper)
        {
            if (ModelState.IsValid)
            {
                db.Entry(paper).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(paper);
        }

        // GET: Papers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Paper paper = db.Papers.Find(id);
            if (paper == null)
            {
                return HttpNotFound();
            }
            return View(paper);
        }

        // POST: Papers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Paper paper = db.Papers.Find(id);
            db.Papers.Remove(paper);
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
