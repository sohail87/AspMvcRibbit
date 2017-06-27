using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using RibbitMvc.Models;

namespace RibbitMvc.Models
{
    public class User
    {

        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime DateCreated { get; set; }

        public int UserProfileId { get; set; }


        //this navigation property needs to use userprofileid property as a foreignkey - by using a foreign key attribute
        //we need to using "System.ComponentModel.DataAnnotations.Schema;" statement for attrbitues
        //pass the name of the property we want to use as the foreign key
        [ForeignKey("UserProfileId")]
        //virtual makes it lazily loaded - data is not loaded unless we access the profile property.
        public virtual UserProfile Profile { get; set; }


        // to create an api that makes it easy to retrieve the ribbits of a user  like this // user.ribbits
        // another navigation property
        
        // sometimes if we access the property it might be null
        //so we create this backing property
        private ICollection<Ribbit> _ribbits;
        public virtual ICollection<Ribbit> Ribbits {
            // this property requires custom get method for when the collection is null
            // this is also the reason for the private variable _ribbits
            // we want to return _ribbits if its not null, but if it is null we will create a new collection of ribbits.
            // ?? is the null coalescing operator
            get { return _ribbits ?? ( _ribbits = new Collection<Ribbit>() ) ;}
            set { _ribbits = value;}
        }

        // this property requires custom get method for when the collection is null
        // this is also the reason for the private variable
        private ICollection<User> _followings;
        public virtual ICollection<User> Followings
        {
            // ?? = coalesce
            get { return _followings ?? (_followings = new Collection<User>() ); }
            set { _followings = value; }
        }

        // this property requires custom get method for when the collection is null
        // this is also the reason for the private variable
        private ICollection<User> _followers;
        public virtual ICollection<User> Followers
        {
            get { return _followers ?? (_followers = new Collection<User>()); }
            set { _followers = value; }
        }

        // the code for [ribbits, followings, followers] is not enough for entity framework
        // we need to manually set up these mappings for ribbits, followings, followers
        // this is done in our data context class inside the "data" folder, RibiitDatabase.cs file

    }
}