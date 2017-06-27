using RibbitMvc.Data;
using RibbitMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RibbitMvc.Services
{
    public class RibbitService : IRibbitService
    {
        private readonly IContext _context;
        private readonly IRibbitRepository _ribbits;

        public RibbitService(IContext context)
        {
            _ribbits = context.Ribbits;
            _context = context;
        }

        public Ribbit GetBy(int id)
        {
            return _ribbits.GetBy(id);
        }

        //Date time is nullable and optional
        public Ribbit Create(User user, string status, DateTime? created = null)
        {
            return Create(user.Id, status, created);
        }

        public Ribbit Create(int userId, string status, DateTime? created = null)
        {
            var ribbit = new Ribbit()
            {
                AuthorId = userId,
                Status = status,
                DateCreated = created.HasValue ? created.Value : DateTime.Now
            };
            _ribbits.Create(ribbit);
            _context.SaveChanges();
            return ribbit;
        }
        public IEnumerable<Ribbit> GetTimelineFor(int userId)
        {
            // retrieve the authors followers
            // if any of them have the id of supplied the user id
            // the ribbits of the people that the user (of the supllied id) follows
            // OR
            // get the ribbits for this user

            return _ribbits.FindAll(r => r.Author.Followers.Any(f => f.Id == userId) || r.AuthorId == userId)
                .OrderByDescending(r => r.DateCreated);
        }
    }
}