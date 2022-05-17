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


        // GET: Authors
        [CustomAuthenticationFilter]
        public ActionResult Index()
        {
            return View(db.Authors.ToList());
        }


        public Author FindAuthor(int? id)
        {

            Author CurrentAuthor = db.Authors.Find(id);

            return CurrentAuthor;
        }


        // GET
        [HttpGet]
        public ActionResult Login()
        {
            //Authentication
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
        


        // GET: Authors/Details/5
        [CustomAuthenticationFilter]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Author author = FindAuthor(id);

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

            Author author = FindAuthor(id);

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




        [CustomAuthenticationFilter]
        public ActionResult ShowMyPapers(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            var MyPapers = GetMyPapers(id);

            if( MyPapers.Count == 0)
            {
                return View("NotFound");
            }

            return View(MyPapers);
        }


        public List<Tag> GetMyPapers(int? id)
        {

            var MyPapers = db.Tags.Where(x => x.AuthID == id).ToList();

            return MyPapers;
        }




        // GET: Authors/Delete/5
        [CustomAuthenticationFilter]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Author author = FindAuthor(id);

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
            Author author = FindAuthor(id);

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
