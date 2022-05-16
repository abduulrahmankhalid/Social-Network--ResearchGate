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
using ResearchGate.Infrastructure;

namespace ResearchGate.Controllers
{
    public class PapersController : Controller
    {
        private DBEntities db = new DBEntities();

        // GET: Papers
        [CustomAuthenticationFilter]
        public ActionResult Index()
        {
            return View(db.Papers.ToList());
        }



        public Paper GetCurrentPaper(int? id)
        {
            Paper CurrentPaper = db.Papers.ToList().Find(x => x.PaperID == id);

            return CurrentPaper;
        }

        [CustomAuthenticationFilter]
        public ActionResult Like(int? id)
        {
            Paper CurrentPaper = GetCurrentPaper(id);

            IncreaseLikes(CurrentPaper);            

            db.Entry(CurrentPaper).State = EntityState.Modified;

            db.SaveChanges();

            return RedirectToAction("Details", new { id = CurrentPaper.PaperID });
        }

        public Paper IncreaseLikes(Paper CurrentPaper)
        {           

            CurrentPaper.Likes += 1;

            return CurrentPaper;
        }




        [CustomAuthenticationFilter]
        public ActionResult Dislike(int? id)
        {
            Paper CurrentPaper = GetCurrentPaper(id);

            IncreaseDisLikes(CurrentPaper);

            db.Entry(CurrentPaper).State = EntityState.Modified;

            db.SaveChanges();

            return RedirectToAction("Details", new { id = CurrentPaper.PaperID });
        }

        public Paper IncreaseDisLikes(Paper CurrentPaper)
        {

            CurrentPaper.Dislikes += 1;

            return CurrentPaper;
        }




        [CustomAuthenticationFilter]
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




        [CustomAuthenticationFilter]
        public ActionResult ShowPaperContributers(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var ContributedAuthors = db.Tags.Where(x => x.PapID == id).ToList();

            if(ContributedAuthors.Count == 0)
            {
                return View("NotFound");
            }

            return View(ContributedAuthors);           
        }




        // GET: Papers/Create
        [CustomAuthenticationFilter]
        public ActionResult Create()
        {
            return View();

        }

        // POST: Papers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PaperID,Title,Date,Abstract,Image,Likes,Dislikes")] Paper paper, HttpPostedFileBase PapImgFile)
        {
            paper.Image = GetPaperImagePath(PapImgFile);
            paper.Likes = 0;
            paper.Dislikes = 0;
            if (ModelState.IsValid)
            {
                db.Papers.Add(paper);
                db.SaveChanges();
                return RedirectToAction("Create","Tags",new {id=paper.PaperID });
            }

            return View(paper);
        }

        public String GetPaperImagePath(HttpPostedFileBase PapImgFile)
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
            return path;
        }




        // GET: Papers/Edit
        [CustomAuthenticationFilter]
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


        // POST: Papers/Edit
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



        // GET: Papers/Delete
        [CustomAuthenticationFilter]
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

        // POST: Papers/Delete
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
