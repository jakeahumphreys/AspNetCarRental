using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using EIRLSSAssignment1.Models;

namespace EIRLSSAssignment1.ServiceLayer
{
    public class ImportService
    {
        public List<DvlaImportedLicense> ImportDvlaLicenses()
        {
            var myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
            var filePath = Path.Combine(myDocuments, "/BangerImports/dvla.csv");

            if (!Directory.Exists(filePath))
            {
                CreateDirectory(filePath);
            }

            foreach (var line in File.ReadAllLines(filePath))
            {
                
            }

            return File.ReadAllLines(filePath).Select(line => line.Split(',')).Select(x => new DvlaImportedLicense
            {
                LicenseNumber = x[0],
                FamilyName = x[1],
                Forenames = x[2],
                DateOfBirth = DateTime.Parse(x[3]),
                YearOfIssue = DateTime.Parse(x[4]),
                Expires = DateTime.Parse(x[5]),
                IssuingAuthority = x[6],
                Address = x[7],
                Status = x[8],
                Date = DateTime.Parse(x[9])
            }).ToList();
        }

        public void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }
    }
}