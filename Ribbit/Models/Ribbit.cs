using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RibbitMvc.Models
{
    public class Ribbit
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }

        // to create an api that looks like this //ribbit.Author
        //so that if we have a ribbit object we could get the author of that object by using the author property
        // so we need another one of these navigation properties
        // needs to be virtual and of type user
        [ForeignKey("AuthorId")]
        public virtual User Author { get; set; }

        public string Status { get; set; }
        public DateTime DateCreated { get; set; }

    }
}