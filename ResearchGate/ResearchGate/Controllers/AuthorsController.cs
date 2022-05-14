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
    public class AuthorsController : Controller
    {
        private DBEntities db = new DBEntities();

        // GET: Authors
        public ActionResult Index()
        {
            return View(db.Authors.ToList());
        }

        // GET: Authors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author author = db.Authors.Find(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }





        //First One============================================

        [HttpGet]
        public ActionResult FilterSearchEmail_get()
        {
            Author author = new Author();

            if (author == null)
            {
                return HttpNotFound();
            }

            return View(author);
        }

        [HttpPost]
        public ActionResult FilterSearchEmail_get([Bind(Include = "AuthorID,Email")] Author author)
        {

            if (author == null)
            {
                return HttpNotFound();
            }

            Author SearchEmailauthor = db.Authors.Where(x => x.Email == author.Email).ToList().FirstOrDefault();

            if( SearchEmailauthor == null )
            {
                ModelState.AddModelError(nameof(Author.Email), "No result found!");
                return View("NotFound");
            }

            else
            {
                return RedirectToAction("Details" , new { id = SearchEmailauthor.AuthorID });
            }



            return View(author);
        }

 





        //Second One============================================

        [HttpGet]
        public ActionResult FilterSearchName_get()
        {
            Author author = new Author();

            if (author == null)
            {
                return HttpNotFound();
            }

            return View(author);
        }

        [HttpPost]
        public ActionResult FilterSearchName_get([Bind(Include = "AuthorID,FirstName")] Author author)
        {

            if (author == null)
            {
                return HttpNotFound();
            }

            Author SearchEmailauthor = db.Authors.Where(x => x.FirstName == author.FirstName).ToList().FirstOrDefault();

            if (SearchEmailauthor == null)
            {
                ModelState.AddModelError(nameof(Author.Email), "No result found!");
                return View("NotFound");
            }

            else
            {
                return RedirectToAction("FilterSearchName_post", new { id = SearchEmailauthor.AuthorID });
            }

            return View(author);
        }


        public ActionResult FilterSearchName_post(int? id)
        {
            Author author = db.Authors.Find(id);

            var SearchEmailauthor = db.Authors.Where(x => x.FirstName == author.FirstName).ToList();

            if (SearchEmailauthor == null)
            {
                return HttpNotFound();
            }

            return View(SearchEmailauthor);
        }





        //Third One============================================
        [HttpGet]
        public ActionResult FilterSearchUni_get()
        {
            Author author = new Author();

            if (author == null)
            {
                return HttpNotFound();
            }

            return View(author);
        }

        [HttpPost]
        public ActionResult FilterSearchUni_get([Bind(Include = "AuthorID,University")] Author author)
        {

            if (author == null)
            {
                return HttpNotFound();
            }

            Author SearchEmailauthor = db.Authors.Where(x => x.University == author.University).ToList().FirstOrDefault();

            if (SearchEmailauthor == null)
            {
                ModelState.AddModelError(nameof(Author.Email), "No result found!");
                return View("NotFound");
            }

            else
            {
                return RedirectToAction("FilterSearchUni_post", new { id = SearchEmailauthor.AuthorID });
            }

            return View(author);
        }


        public ActionResult FilterSearchUni_post(int? id)
        {
            Author author = db.Authors.Find(id);

            var SearchEmailauthor = db.Authors.Where(x => x.University == author.University).ToList();

            if (SearchEmailauthor == null)
            {
                return HttpNotFound();
            }

            return View(SearchEmailauthor);
        }




        public ActionResult MyPaper_Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
  
            var tag = db.Tags.Where(x => x.AuthID == id).ToList();
            return View(tag);
        }


        // GET: Authors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Authors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AuthorID,Email,Password,FirstName,LastName,University,Department,Mobile,ProfImage")] Author author)
        {
            if (ModelState.IsValid)
            {
                db.Authors.Add(author);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(author);
        }

        // GET: Authors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author author = db.Authors.Find(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }

        // POST: Authors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AuthorID,Email,Password,FirstName,LastName,University,Department,Mobile,ProfImage")] Author author)
        {
            if (ModelState.IsValid)
            {
                db.Entry(author).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(author);
        }

        // GET: Authors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author author = db.Authors.Find(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }

        // POST: Authors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Author author = db.Authors.Find(id);
            db.Authors.Remove(author);
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
