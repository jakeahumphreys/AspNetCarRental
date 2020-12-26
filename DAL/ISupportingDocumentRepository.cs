using EIRLSSAssignment1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIRLSSAssignment1.DAL
{
    public interface ISupportingDocumentRepository : IDisposable
    {
        IList<SupportingDocument> GetSupportingDocuments();
        SupportingDocument GetSupportingDocumentById(int id);
        void Insert(SupportingDocument SupportingDocument);
        void Update(SupportingDocument SupportingDocument);
        void Delete(SupportingDocument SupportingDocument);
        void Save();
    }
}