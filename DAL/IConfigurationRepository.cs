using EIRLSSAssignment1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIRLSSAssignment1.DAL
{
    public interface IConfigurationRepository : IDisposable
    {
        IList<Configuration> GetConfigurations();
        Configuration GetConfigurationById(int id);
        void Insert(Configuration Configuration);
        void Update(Configuration Configuration);
        void Delete(Configuration Configuration);
        void Save();
    }
}