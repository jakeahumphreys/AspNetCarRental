using EIRLSSAssignment1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIRLSSAssignment1.DAL
{
    public interface IDrivingLicenseRepository : IDisposable
    {
        IList<DrivingLicense> GetDrivingLicenses();
        DrivingLicense GetDrivingLicenseById(int id);
        void Insert(DrivingLicense DrivingLicense);
        void Update(DrivingLicense DrivingLicense);
        void Delete(DrivingLicense DrivingLicense);
        void Save();
        void Truncate();
    }
}