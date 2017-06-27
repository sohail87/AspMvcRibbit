using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace RibbitMvc.Data
{
    public class Context :IContext
    {
        // isnt the best implementation fo this constructure
        // if these 3 parameters are null, then it will create 3 objects, those 3 classes are specific to our entity framework datastore
        // so if we ever change out those repositories, then we have to address this code or create a whole new context class

        //our dbcontext
        private readonly DbContext _db;

        public Context(DbContext context = null, IUserRepository users = null, 
            IRibbitRepository ribbits = null, IUserProfileRepository profiles = null)
        {
            // if our paremeters have a value, if they do we use those values for our properties and our private variable
            // otherwise we create those objects

            _db = context ?? new RibbitDatabase();
            Users = users ?? new UserRepository(_db, true);
            Ribbits = ribbits ?? new RibbitRepository(_db, true);

            Profiles = profiles ?? new UserProfileRepository(_db, true);
        }
        public IUserRepository Users
        {
            get;
            private set;
        }

        public IRibbitRepository Ribbits
        {
            get;
            private set;
        }

        public IUserProfileRepository Profiles { get; private set; }

        public int SaveChanges()
        {
            return _db.SaveChanges();
        }

        public void Dispose()
        {
            if (_db != null)
            {
                try {
                    _db.Dispose();
                }
                catch { }

            }
        }
    }
}