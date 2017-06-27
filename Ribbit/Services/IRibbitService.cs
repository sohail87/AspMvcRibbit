using RibbitMvc.Models;
using System;
using System.Collections.Generic;
namespace RibbitMvc.Services
{
    public interface IRibbitService
    {
        Ribbit Create(int userId, string status, DateTime? created = null);
        Ribbit Create(User user, string status, DateTime? created = null);
        Ribbit GetBy(int id);
        IEnumerable<Ribbit> GetTimelineFor(int userId);
    }
}
 