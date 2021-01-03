using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EIRLSSAssignment1.DAL;
using EIRLSSAssignment1.Models;

namespace EIRLSSAssignment1.Controllers
{
    public class ConfigurationController : Controller
    {
        private ConfigurationRepository _configurationRepository;

        public ConfigurationController()
        {
            _configurationRepository = new ConfigurationRepository(new ApplicationDbContext());
        }

        public ActionResult Index()
        {
            return View(_configurationRepository.GetConfigurations());
        }

        public ActionResult Details(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Configuration configuration = _configurationRepository.GetConfigurationById(id);
            if (configuration == null)
            {
                return HttpNotFound();
            }
            return View(configuration);
        }

        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Configuration configuration)
        {

            configuration.IsConfigurationActive = true;

            if (ModelState.IsValid)
            {
                DisableActiveConfiguration();

                _configurationRepository.Insert(configuration);
                _configurationRepository.Save();

                return RedirectToAction("Index");
            }

            return View(configuration);
        }

        public ActionResult Edit(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Configuration configuration = _configurationRepository.GetConfigurationById(id);
            if (configuration == null)
            {
                return HttpNotFound();
            }
            return View(configuration);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Configuration configuration)
        {

            if (configuration.IsConfigurationActive == true)
            {
                DisableActiveConfiguration();
            }

            if (ModelState.IsValid)
            {
                Configuration configToUpdate = _configurationRepository.GetConfigurationById(configuration.Id);

                configToUpdate.IsConfigurationActive = configuration.IsConfigurationActive;
                configToUpdate.Name = configuration.Name;
                configToUpdate.IsOpenForRentals = configuration.IsOpenForRentals;
                configToUpdate.OpeningTime = configuration.OpeningTime;
                configToUpdate.ClosingTime = configuration.ClosingTime;
                configToUpdate.MinRentalHours = configuration.MinRentalHours;
                configToUpdate.MaxRentalHours = configuration.MaxRentalHours;

                _configurationRepository.Update(configToUpdate);
                _configurationRepository.Save();

                return RedirectToAction("Index");
            }
            return View(configuration);
        }

        // GET: Configuration/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Configuration configuration = _configurationRepository.GetConfigurationById(id);
            if (configuration == null)
            {
                return HttpNotFound();
            }
            return View(configuration);
        }

        // POST: Configuration/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Configuration configuration = _configurationRepository.GetConfigurationById(id);
            _configurationRepository.Delete(configuration);
            _configurationRepository.Save();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _configurationRepository.Save();
            }
            base.Dispose(disposing);
        }

        public void DisableActiveConfiguration()
        {
            Configuration activeConfiguration = _configurationRepository.GetConfigurations().Where(c => c.IsConfigurationActive == true).SingleOrDefault();

            if(activeConfiguration != null)
            {
                activeConfiguration.IsConfigurationActive = false;
                _configurationRepository.Update(activeConfiguration);
                _configurationRepository.Save();
            }
        }
    }
}
