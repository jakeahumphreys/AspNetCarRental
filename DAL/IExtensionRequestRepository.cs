using EIRLSSAssignment1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIRLSSAssignment1.DAL
{
    public interface IExtensionRequestRepository : IDisposable
    {
        IList<ExtensionRequest> GetExtensionRequests();
        ExtensionRequest GetExtensionRequestById(int id);
        void Insert(ExtensionRequest ExtensionRequest);
        void Update(ExtensionRequest ExtensionRequest);
        void Delete(ExtensionRequest ExtensionRequest);
        void Save();
    }
}