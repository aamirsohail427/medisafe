using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EHealthCare.Models
{
    public class Base : IDisposable
    {

        internal EHealthCareEntities _db;

        public Base()
        {
            try
            {
                _db = new EHealthCareEntities();
                _db.Configuration.LazyLoadingEnabled = true;
                _db.Configuration.ProxyCreationEnabled = false;
            }
            catch
            {
            }
        }
        private void CleanUp()
        {
            if (_db != null) _db.Dispose();
        }
        public void Dispose()
        {
            this.CleanUp();
            GC.SuppressFinalize(this);
        }
    }
}