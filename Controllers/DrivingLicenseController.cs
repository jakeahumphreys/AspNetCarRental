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
    public class DrivingLicenseController : Controller
    {
        private DrivingLicenseRepository _drivingLicenseRepository;
        private ApplicationDbContext _applicationDbContext;

        public DrivingLicenseController()
        {
            _drivingLicenseRepository = new DrivingLicenseRepository(new DrivingLicenseContext());
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

        // POST: DrivingLicense/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DrivingLicenseViewModel drivingLicenseVM)
        {

            byte[] imageByteArray = null;

            if(drivingLicenseVM.ImageToUpload != null)
            {
                using(MemoryStream memoryStream = new MemoryStream())
                {
                    drivingLicenseVM.ImageToUpload.InputStream.CopyTo(memoryStream);
                    imageByteArray = memoryStream.GetBuffer();
                }
            }

            drivingLicenseVM.License.Image = imageByteArray;

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

                return RedirectToAction("Index");
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

            byte[] imageByteArray = null;

            if (drivingLicenseVM.ImageToUpload != null)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    drivingLicenseVM.ImageToUpload.InputStream.CopyTo(memoryStream);
                    imageByteArray = memoryStream.GetBuffer();
                }

                drivingLicenseVM.License.Image = imageByteArray;
            }


            if (ModelState.IsValid)
            {
                _drivingLicenseRepository.Update(drivingLicenseVM.License);
                _drivingLicenseRepository.Save();
;
                return RedirectToAction("Index");
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
            DrivingLicense drivingLicense = _drivingLicenseRepository.GetDrivingLicenseById(id);
            _drivingLicenseRepository.Delete(drivingLicense);
            _drivingLicenseRepository.Save();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _drivingLicenseRepository.Save();
            }
            base.Dispose(disposing);
        }
    }
}
