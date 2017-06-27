using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using RibbitMvc.Models;

namespace RibbitMvc.Data  
{

    // DataContext for entity framework
    // entity framework needs to be added as a reference
    // then we can add //using System.Data.Entity;

    // we need to setup 3 properties that will serve as the tables
    public class RibbitDatabase : DbContext
    {
        // we need a constructor that calls the base classes constructor
        // passing in the name of our connection string
        // EF needs to know what connection string to use
        public RibbitDatabase() : base("RibbitConnection") { }


        //users tables
        public DbSet<User> Users { get; set; }

        //user profiles table
        public DbSet<UserProfile> UserProfiles { get; set; }

        //ribbits table
        public DbSet<Ribbit> Ribbits { get; set; }

        //belows sets up the mapping for the followers, followings and ribbits properties on our user class
        // we need to overide a method OnModelCreating
        
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // call the model builder entity, specify user
            // this has many followers
            // with many following
            modelBuilder.Entity<User>()
                .HasMany(u => u.Followers)
                .WithMany(u => u.Followings)
                // map this to a table. we will also have another table for the mappings of the followers and followings
                // for this we will use a lambda expression, where we will map the leftkey as followingid, we will map the right key as followerid
                // and we want to map this to the table Follow
                // this will create a table called "Follow" with the 2 columns below
                .Map(map =>
                {
                    map.MapLeftKey("FollowingId");
                    map.MapRightKey("FollowerId");
                    map.ToTable("Follow");
                });

            // call the model builder entity, specify user
            // this has many ribbits
            modelBuilder.Entity<User>().HasMany(u => u.Ribbits);

            //call the base onmodelcreating method
            base.OnModelCreating(modelBuilder);
        }

        // the first time we interact with our database classes, entity framework is going to generate our database for us.
        // we can use our entity classes for querying our database
        // we could do that within the controllers if we wanted to, but we dont want to do that
        // we want our controllers to be as clean as possible
        // so we are going to add another layer, a repository layer, responsible for querying our database.
        // 

    }
}