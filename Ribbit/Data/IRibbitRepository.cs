using RibbitMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RibbitMvc.Data
{
    public interface IRibbitRepository : IRepository<Ribbit>
    {
        //get a single ribbit
        Ribbit GetBy(int id);
        
        //get ribbits for an individual user
        IEnumerable<Ribbit> GetFor(User user);

        // add a ribbit to a user
        void AddFor(Ribbit ribbit, User user);
    }
}