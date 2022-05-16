using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using ResearchGate.Infrastructure;
using ResearchGate.Models;

namespace ResearchGate.Controllers
{
    public class AuthorsController : Controller 
    {
        private DBEntities db = new DBEntities();
        
        // GET
        [HttpGet]
        public ActionResult Login()
        {

            Session["username"] = null;
            Session["AuthID"] = null;
            Session["ProfImg"] = null;

            return View();
        }

        //Post
        [HttpPost]
        public ActionResult Login([Bind(Include ="Email , Password")] Author author)
        {
            Author authorData = isAuthorFound(author);

            if (authorData != null)
            {
                Session["username"] = authorData.FirstName;
                Session["AuthID"] = authorData.AuthorID;
                Session["ProfImg"] = authorData.ProfImage;

                return RedirectToAction("Details" , new { id = authorData.AuthorID });
            }
            else
            {
                ViewBag.error = "Invalid User";
                return View(author);
            }
        }

        public Author isAuthorFound([Bind(Include = "Email , Password")] Author author)
        {
             author =
                db.Authors.Where(x => x.Email == author.Email && x.Password == author.Password).ToList().FirstOrDefault();

            return (author);
        }



        // GET: Authors
        [CustomAuthenticationFilter]
        public ActionResult Index()
        {
            return View(db.Authors.ToList());
        }



        // GET: Authors/Details/5
        [CustomAuthenticationFilter]
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




        // GET: Authors/Create
        public ActionResult Create()
        {
            //Authentication

            Session["username"] = null;
            Session["AuthID"] = null;
            Session["ProfImg"] = null;

            return View();
        }

        // POST: Authors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AuthorID,Email,Password,ConfPass,FirstName,LastName,University,Department,Mobile,ProfImage")] Author author, HttpPostedFileBase ProfImgFile)
        {

            if (!isEmailValid(author))
            {
                ModelState.AddModelError("Email", "Email Already Used");
            }

            if (ModelState.IsValid)
            {

                author.ProfImage = GetProfImagePath(ProfImgFile);

                db.Authors.Add(author);

                db.SaveChanges();

                //Authentication

                Session["username"] = author.FirstName;
                Session["AuthID"] = author.AuthorID;
                Session["ProfImg"] = author.ProfImage;


                return RedirectToAction("Details", new { id = author.AuthorID });
            }

            return View(author);
        }


        public bool isEmailValid(Author author)
        {
            Author searchEmail = db.Authors.AsNoTracking().Where(x => x.Email == author.Email).ToList().FirstOrDefault();

            if (searchEmail != null)
            {
                ModelState.AddModelError("Email", "Email Already Used");
                return false;
            }

            return true;
        }


        public String GetProfImagePath(HttpPostedFileBase ProfImgFile)
        {
            string path = "";

            if (ProfImgFile != null)
            {
                path = "~/Content/ProfileImages/" + Path.GetFileName(ProfImgFile.FileName);
                ProfImgFile.SaveAs(Server.MapPath(path));
            }
            else
            {
                path = "~/Content/ProfileImages/profile_default_img.png";
            }
            return path;
        }



        // GET: Authors/Edit/5
        [CustomAuthenticationFilter]
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            [Bind(Include = "AuthorID,Email,Password,ConfPass,FirstName,LastName,University,Department,Mobile,ProfImage")] Author author, HttpPostedFileBase EditProfImgFile)
        {

            var NewEditedAuthor = GetNewEditedAuthor(author);

            var OldEditedAuthor = GetOldEditedAuthor(author);

            if (NewEditedAuthor != null && NewEditedAuthor.Email != OldEditedAuthor.Email)
            {

                ModelState.AddModelError("Email", "Email Already Used");

            }

            EditPath(EditProfImgFile, author, OldEditedAuthor);

            if (ModelState.IsValid)
            {
                db.Entry(author).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = author.AuthorID });
            }
            return View(author);
        }

        public Author GetNewEditedAuthor(Author author)
        {
            var NewEditedAuthor = db.Authors.AsNoTracking().Where(x => x.Email == author.Email).ToList().FirstOrDefault();

            return NewEditedAuthor;
        }

        public Author GetOldEditedAuthor(Author author)
        {
            var OldEditedAuthor = db.Authors.AsNoTracking().Where(x => x.AuthorID == author.AuthorID).ToList().FirstOrDefault();

            return OldEditedAuthor;
        }

        public void EditPath(HttpPostedFileBase EditProfImgFile, Author author, Author OldEditedAuthor)
        {
            string path = "";

            if (EditProfImgFile != null)
            {
                path = "~/Content/ProfileImages/" + Path.GetFileName(EditProfImgFile.FileName);
                EditProfImgFile.SaveAs(Server.MapPath(path));
                author.ProfImage = path;
            }
            else
            {
                author.ProfImage = OldEditedAuthor.ProfImage;
            }
        }



        [ActionName("SearchByEmail")]
        [HttpGet]
        [CustomAuthenticationFilter]
        public ActionResult GetAuthorEmail()
        {
            Author author = new Author();

            return View(author);
        }


        [HttpPost]
        public ActionResult SearchByEmail([Bind(Include = "AuthorID,Email")] Author author)
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
        }




        [ActionName("SearchByName")]
        [HttpGet]
        [CustomAuthenticationFilter]
        public ActionResult GetAuthorName()
        {
            Author author = new Author();         

            return View(author);
        }


        [HttpPost]
        public ActionResult SearchByName([Bind(Include = "AuthorID,FirstName")] Author author)
        {
            if (author == null)
            {
                return HttpNotFound();
            }

            Author searchedAuthor = db.Authors.Where(x => x.FirstName == author.FirstName).ToList().FirstOrDefault();

            if (searchedAuthor == null)
            {
                ModelState.AddModelError(nameof(Author.FirstName), "No result found!");
                return View("NotFound");
            }

            else
            {
                return RedirectToAction("ReturnSearchedNames", new { id = searchedAuthor.AuthorID });
            }
        }


        [CustomAuthenticationFilter]
        public ActionResult ReturnSearchedNames(int? id)
        {
            Author author = db.Authors.Find(id);

            var searchedAuthors = db.Authors.Where(x => x.FirstName == author.FirstName).ToList();

            if (searchedAuthors == null)
            {
                return HttpNotFound();
            }

            return View(searchedAuthors);
        }




        [ActionName("SearchByUniversity")]
        [CustomAuthenticationFilter]
        [HttpGet]
        public ActionResult GetAuthorUniversity()
        {
            Author author = new Author();

            return View(author);
        }

        [HttpPost]
        [CustomAuthenticationFilter]
        public ActionResult SearchByUniversity([Bind(Include = "AuthorID,University")] Author author)
        {

            if (author == null)
            {
                return HttpNotFound();
            }

            Author searchedAuthor = db.Authors.Where(x => x.University == author.University).ToList().FirstOrDefault();

            if (searchedAuthor == null)
            {
                ModelState.AddModelError(nameof(Author.Email), "No result found!");
                return View("NotFound");
            }

            else
            {
                return RedirectToAction("ReturnUniversityAuthors", new { id = searchedAuthor.AuthorID });
            }
        }

        [CustomAuthenticationFilter]
        public ActionResult ReturnUniversityAuthors(int? id)
        {
            Author author = db.Authors.Find(id);

            var SearchedAuthors = db.Authors.Where(x => x.University == author.University).ToList();

            if (SearchedAuthors == null)
            {
                return HttpNotFound();
            }

            return View(SearchedAuthors);
        }




        [CustomAuthenticationFilter]
        public ActionResult ShowMyPapers(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
  
            var MyPapers = db.Tags.Where(x => x.AuthID == id).ToList();

            if( MyPapers.Count == 0)
            {
                return View("NotFound");
            }

            return View(MyPapers);
        }




        // GET: Authors/Delete/5
        [CustomAuthenticationFilter]
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
