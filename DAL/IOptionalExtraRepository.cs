using EIRLSSAssignment1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIRLSSAssignment1.DAL
{
    public interface IOptionalExtraRepository : IDisposable
    {
        IList<OptionalExtra> GetOptionalExtras();
        OptionalExtra GetOptionalExtraById(int id);
        void Insert(OptionalExtra OptionalExtra);
        void Update(OptionalExtra OptionalExtra);
        void Delete(OptionalExtra OptionalExtra);
        void Save();
    }
}