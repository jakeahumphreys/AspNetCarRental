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
using MVCWebAssignment1.Customisations;

namespace EIRLSSAssignment1.Controllers
{
    [CustomAuthorize(Roles = "User,Admin")]
    public class DrivingLicenseController : Controller
    {
        private DrivingLicenseRepository _drivingLicenseRepository;
        private ApplicationDbContext _applicationDbContext;

        public DrivingLicenseController()
        {
            _drivingLicenseRepository = new DrivingLicenseRepository(new ApplicationDbContext());
            _applicationDbContext = new ApplicationDbContext();
        }

        // GET: DrivingLicense
        public ActionResult Index()
        {
            return View(_drivingLicenseRepository.GetDrivingLicenses());
        }

        // GET: DrivingLicense/Details/5
        public ActionResult Details(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DrivingLicense drivingLicense = _drivingLicenseRepository.GetDrivingLicenseById(id);
            if (drivingLicense == null)
            {
                return HttpNotFound();
            }

            return View(drivingLicense);
        }

        // GET: DrivingLicense/Create
        public ActionResult Create()
        {
            if (User.IsInRole("Admin"))
            {
                ViewBag.userId = new SelectList(_applicationDbContext.Users.ToList(), "Id", "Name");
                return View();
            }
            else
            {
                var userId = User.Identity.GetUserId();
                var user = _applicationDbContext.Users.Find(userId);
                ViewBag.Name = user.Name;
                ViewBag.UserId = userId;
                return View();
            }
            
        }

        // POST: DrivingLicense/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DrivingLicenseViewModel drivingLicenseVM)
        {

            if (drivingLicenseVM.ImageToUpload != null)
            {
                drivingLicenseVM.License.Image = convertImageToByteArray(drivingLicenseVM.ImageToUpload);
            }

            if (ModelState.IsValid)
            {
                _drivingLicenseRepository.Insert(drivingLicenseVM.License);
                _drivingLicenseRepository.Save();

                if (drivingLicenseVM.License != null)
                {
                    var user = _applicationDbContext.Users.Find(User.Identity.GetUserId());
                    if(user != null)
                    {
                        if(user.DrivingLicenseId != 0)
                        {
                            user.DrivingLicenseId = drivingLicenseVM.License.Id;
                            _applicationDbContext.SaveChanges();
                        }
                    }
                }

                return RedirectToAction("Index", "Home", null);
            }

            return View(drivingLicenseVM);
        }

        // GET: DrivingLicense/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DrivingLicense drivingLicense = _drivingLicenseRepository.GetDrivingLicenseById(id);
            if (drivingLicense == null)
            {
                return HttpNotFound();
            }

            var drivingLicenseVM = new DrivingLicenseViewModel { License = drivingLicense, ImageToUpload = null};

            return View(drivingLicenseVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( DrivingLicenseViewModel drivingLicenseVM)
        {


            if (drivingLicenseVM.ImageToUpload != null)
            {
                drivingLicenseVM.License.Image = convertImageToByteArray(drivingLicenseVM.ImageToUpload);
            }


            if (ModelState.IsValid)
            {
                _drivingLicenseRepository.Update(drivingLicenseVM.License);
                _drivingLicenseRepository.Save();
;
                return RedirectToAction("Index", "Home", null);
            }
            return View(drivingLicenseVM);
        }

        // GET: DrivingLicense/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DrivingLicense drivingLicense = _drivingLicenseRepository.GetDrivingLicenseById(id);
            if (drivingLicense == null)
            {
                return HttpNotFound();
            }
            return View(drivingLicense);
        }

        // POST: DrivingLicense/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ApplicationUser user = _applicationDbContext.Users.Find(User.Identity.GetUserId());

            DrivingLicense drivingLicense = _drivingLicenseRepository.GetDrivingLicenseById(id);


            if (user.DrivingLicenseId == id)
            {
                user.DrivingLicenseId = null;
                _applicationDbContext.SaveChanges();
            }

            _drivingLicenseRepository.Delete(drivingLicense);
            _drivingLicenseRepository.Save();
            return RedirectToAction("Index", "Admin", null);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _drivingLicenseRepository.Save();
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
