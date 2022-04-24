using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
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

            var record =
                db.Authors.Where(x => x.Email == author.Email && x.Password == author.Password).ToList().FirstOrDefault();

            if (record != null)
            {
                Session["username"] = record.FirstName;
                Session["AuthID"] = record.AuthorID;
                Session["ProfImg"] = record.ProfImage;
                return RedirectToAction("Details" , new { id = record.AuthorID });
            }
            else
            {
                ViewBag.error = "Invalid User";
                return View(author);
            }
        }

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

        // GET: Authors/Create
        public ActionResult Create()
        {
            Session["username"] = null;
            Session["AuthID"] = null;
            Session["ProfImg"] = null;

            return View();
        }

        // POST: Authors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AuthorID,Email,Password,ConfPass,FirstName,LastName,University,Department,Mobile,ProfImage")] Author author , HttpPostedFileBase ProfImgFile)
        {
            if (ModelState.IsValid)
            {
                string path = "";
                if (ProfImgFile !=null)
                {
                    path = "~/Content/ProfileImages/" + Path.GetFileName(ProfImgFile.FileName) ;
                    ProfImgFile.SaveAs(Server.MapPath(path));
                }
                else
                {
                    path = "~/Content/ProfileImages/profile_default_img.jpg";
                }
                author.ProfImage = path;
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
        public ActionResult Edit(
            [Bind(Include = "AuthorID,Email,Password,ConfPass,FirstName,LastName,University,Department,Mobile,ProfImage")] Author author , HttpPostedFileBase EditProfImgFile)
        {

            var before = db.Authors.AsNoTracking().Where( x=> x.AuthorID == author.AuthorID).ToList().FirstOrDefault();

            string path = "";
            if (EditProfImgFile != null)
            {
                path = "~/Content/ProfileImages/" + Path.GetFileName(EditProfImgFile.FileName);
                EditProfImgFile.SaveAs(Server.MapPath(path));
                author.ProfImage = path;
            }
            else
            {
                author.ProfImage = before.ProfImage;
            }

            if (ModelState.IsValid)
            {
                db.Entry(author).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = author.AuthorID });
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
