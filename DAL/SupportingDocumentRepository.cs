using EIRLSSAssignment1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace EIRLSSAssignment1.DAL
{
    public class SupportingDocumentRepository : ISupportingDocumentRepository
    {
        private readonly SupportingDocumentContext _context;

        public SupportingDocumentRepository(SupportingDocumentContext context)
        {
            _context = context;
        }

        public IList<SupportingDocument> GetSupportingDocuments()
        {
            return _context.SupportingDocuments.ToList();
        }

        public SupportingDocument GetSupportingDocumentById(int id)
        {
            return _context.SupportingDocuments.Where(x => x.Id == id).SingleOrDefault();
        }

        public void Insert(SupportingDocument SupportingDocument)
        {
            _context.SupportingDocuments.Add(SupportingDocument);
        }

        public void Update(SupportingDocument SupportingDocument)
        {
            _context.Entry(SupportingDocument).State = EntityState.Modified;
        }

        public void Delete(SupportingDocument SupportingDocument)
        {
            _context.SupportingDocuments.Remove(SupportingDocument);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}