using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RibbitMvc.Data
{
    //implement idisposable
    public interface IContext : IDisposable
    {
        //declare 3 members

        //a context needs to provide access to ....
        IUserRepository Users { get; }
        IRibbitRepository Ribbits { get; }

        IUserProfileRepository Profiles { get; }

        //method to save the changes
        int SaveChanges();

    }
}