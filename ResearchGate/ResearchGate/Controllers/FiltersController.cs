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
    public class FiltersController : Controller
    {

        private DBEntities db = new DBEntities();


        public Author FindAuthor(int? id)
        {

            Author CurrentAuthor = db.Authors.Find(id);

            return CurrentAuthor;
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

            if (SearchEmailauthor == null)
            {
                ModelState.AddModelError(nameof(Author.Email), "No result found!");
                return View("NotFound");
            }

            else
            {
                return RedirectToAction("Details", "Authors", new { id = SearchEmailauthor.AuthorID });
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
            Author author = FindAuthor(id);

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
            Author author = FindAuthor(id);

            var SearchedAuthors = db.Authors.Where(x => x.University == author.University).ToList();

            if (SearchedAuthors == null)
            {
                return HttpNotFound();
            }

            return View(SearchedAuthors);
        }
    }
}