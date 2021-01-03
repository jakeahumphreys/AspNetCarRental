using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EIRLSSAssignment1.DAL;
using EIRLSSAssignment1.Models;
using Microsoft.AspNet.Identity;

namespace EIRLSSAssignment1.Controllers
{
    public class SupportingDocumentController : Controller
    {
        private SupportingDocumentRepository _supportingDocumentRepository;
        private ApplicationDbContext _applicationDbContext;

        public SupportingDocumentController()
        {
            _supportingDocumentRepository = new SupportingDocumentRepository(new ApplicationDbContext());
            _applicationDbContext = new ApplicationDbContext();
        }

        // GET: SupportingDocument
        public ActionResult Index()
        {
            return View(_supportingDocumentRepository.GetSupportingDocuments());
        }

        // GET: SupportingDocument/Details/5
        public ActionResult Details(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SupportingDocument supportingDocument = _supportingDocumentRepository.GetSupportingDocumentById(id);
            if (supportingDocument == null)
            {
                return HttpNotFound();
            }
            return View(supportingDocument);
        }

        // GET: SupportingDocument/Create
        public ActionResult Create(string userId)
        {
            if(userId == "")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                ViewBag.UserId = userId;

                var user = _applicationDbContext.Users.Find(userId);
                ViewBag.Name = user.Name;
                return View();
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SupportingDocumentViewModel supportingDocumentVM)
        {


            if (supportingDocumentVM.ImageToUpload != null)
            {
                supportingDocumentVM.SupportingDocument.Image = convertImageToByteArray(supportingDocumentVM.ImageToUpload);
            }

            if (ModelState.IsValid)
            {
                _supportingDocumentRepository.Insert(supportingDocumentVM.SupportingDocument);
                _supportingDocumentRepository.Save();

                if(supportingDocumentVM.SupportingDocument != null)
                {
                    var user = _applicationDbContext.Users.Find(User.Identity.GetUserId());
                    if (user != null)
                    {
                        if (user.SupportingDocumentId != 0)
                        {
                            user.SupportingDocumentId = supportingDocumentVM.SupportingDocument.Id; 
                            _applicationDbContext.SaveChanges();
                        }
                    }
                }

                return RedirectToAction("Index");
            }

            return View(supportingDocumentVM);
        }

        // GET: SupportingDocument/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SupportingDocument supportingDocument = _supportingDocumentRepository.GetSupportingDocumentById(id);
            if (supportingDocument == null)
            {
                return HttpNotFound();
            }

            var supportingDocumentVM = new SupportingDocumentViewModel { SupportingDocument = supportingDocument };

            return View(supportingDocumentVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SupportingDocumentViewModel supportingDocumentVM)
        {
            if (supportingDocumentVM.ImageToUpload != null)
            {
                supportingDocumentVM.SupportingDocument.Image = convertImageToByteArray(supportingDocumentVM.ImageToUpload);
            }

            if (ModelState.IsValid)
            {
                _supportingDocumentRepository.Update(supportingDocumentVM.SupportingDocument);
                _supportingDocumentRepository.Save();
                return RedirectToAction("Index");
            }
            return View(supportingDocumentVM);
        }

        public ActionResult Delete(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SupportingDocument supportingDocument = _supportingDocumentRepository.GetSupportingDocumentById(id);
            if (supportingDocument == null)
            {
                return HttpNotFound();
            }
            return View(supportingDocument);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            ApplicationUser user = _applicationDbContext.Users.Find(User.Identity.GetUserId());
            SupportingDocument supportingDocument = _supportingDocumentRepository.GetSupportingDocumentById(id);

            if (user.SupportingDocumentId == id)
            {
                user.SupportingDocumentId = null;
                _applicationDbContext.SaveChanges();
            }

            _supportingDocumentRepository.Delete(supportingDocument);
            _supportingDocumentRepository.Save();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _supportingDocumentRepository.Dispose();
            }
            base.Dispose(disposing);
        }

        private byte[] convertImageToByteArray(HttpPostedFileBase image)
        {
            byte[] imageByteArray = null;

            if (image != null)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    image.InputStream.CopyTo(memoryStream);
                    imageByteArray = memoryStream.GetBuffer();
                }
            }

            return imageByteArray;
        }
    }
}
