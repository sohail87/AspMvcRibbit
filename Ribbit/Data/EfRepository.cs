using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace RibbitMvc.Data
{
    //entity framework repository
    public class EfRepository<T> : IRepository<T>
        // set a connstraint for T, so T has to be a class
        where T : class
    {
        //because this class will be specific for entity framework, we do need our DbContext 
        protected DbContext Context;
        // we might be sharing this same context between different repositories
        // in the case of our application we will want to have the same context for the user, the ribbit and the user profile repository
        // entity framework is a connected ORM, we will typically want to use the same context throughout the lifecycle of a single request
        // we can control the behaviour of our repository in relation to our context
        protected readonly bool ShareContext;

        //first constructor.  this(context, false) calls the second constructor
        // if we wanted to use this same class in other projects. it would be wise to set this to false. 
        // in this application we would just need to ensure that we pass in that second value of true when calling it
        public EfRepository(DbContext context) : this(context, false) { }
        //second constructor
        public EfRepository(DbContext context, bool sharedContext)
        {
            // set class variables
            Context = context;
            ShareContext = sharedContext;
        }

        // Since we are working with entity classes of type T
        // we need the DbSet of this particular entity class
        // this is the property we will use throughout this class in order to interact with our database table
        protected DbSet<T> DbSet
        {
            get
            {
                //
                return Context.Set<T>();
            }
        }

        public IQueryable<T> All()
        {
            //converts ienumerable to Linq.IQueryable
            return DbSet.AsQueryable();
        }

        public bool Any(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            //example of predicate is "Any(item => item == 2)"
            return DbSet.Any(predicate);
        }

        public int Count
        {
            //returns the number of elements in a sequence
            get { return DbSet.Count(); }
        }

        public T Create(T t)
        {
            // msdn= Adds the given entity to the context underlying the set in the Added state such that it will be inserted into the database when SaveChanges is called.
            // add an entity to the dbset
            DbSet.Add(t);



            //initiate the transaction if not shared
            if (!ShareContext)
            {
                Context.SaveChanges();
            }

            return t;
        }

        public int Delete(T t)
        {
            // msdn= Marks the given entity as Deleted such that it will be deleted from the database when SaveChanges is called.
            // msdn= Note that the entity must exist in the context in some other state before this method is called.
            DbSet.Remove(t);

            
            //initiate the transaction if not shared
            if (!ShareContext)
            {
                return Context.SaveChanges();
            }

            return 0;
        }

        //predicate is a lamda expression
        public int Delete(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            //retrieve the records that meet our particular criteria (using our function below)
            var records = FindAll(predicate);

            // there is no Remove() overload that we can specify a list of records to remove, so we have to loop through all the records
            foreach (var record in records)
            {
                DbSet.Remove(record);
            }

            //initiate the transaction if not shared
            if (!ShareContext)
            {
                return Context.SaveChanges();
            }

            return 0;
        }

        //find methods are simple, this first overload method, just retrun dbset.find and pass in the keys
        public T Find(params object[] keys)
        {
            return DbSet.Find(keys);
        }


        //second overload method is almost as simple, return db set method and pass in the lamda predicate.
        public T Find(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return DbSet.SingleOrDefault(predicate);
        }

        // first find all method is simple as well, return db set method and pass in the lamda predicate
        public IQueryable<T> FindAll(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return DbSet.Where(predicate).AsQueryable();
        }

        // second findall overload method
        //index is page no or the number of times the size has been called before
        public IQueryable<T> FindAll(System.Linq.Expressions.Expression<Func<T, bool>> predicate, int index, int size)
        {
            //index is page no (0 = first page) or the number of times the size has been called before
            // if we want to display 50 rows (size) on the 1st page (index), then index = 0, which makes skip = 0
            // if we want to display 50 rows (size) on the 3rd page (index), then index = 2, which makes skip = 100
            var skip = index * size;

            //setting query to be our dbset
            IQueryable<T> query = DbSet;

            //only if predicate is not null perform the where statement
            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            
            // we need to check if the skip value is 0, because if it is zero we essentially write code like this:
            // query.skip(0).take(size);
            // this looks like a free operation "skip(0)".
            // its not gauranteed to be a free operation, the linq provider does not have to optimise skip 0.
            // so we inturn need to make sure our code is optimised, avoid skip 0 as much as possible
            if (skip != 0)
            {
                query = query.Skip(skip);
            }

            //finally we want to return query, take the amount of recors specified by size and then return as queryable.
            return query.Take(size).AsQueryable();

        }

        public int Update(T t)
        {
            // we need to create out entry
            var entry = Context.Entry(t);

            // first attach the entity object to our context, we might not need to do this but best to avoid issues.
            DbSet.Attach(t);

            //after we attach, we need to set the state. this has an entity state of modified.
            entry.State = EntityState.Modified;


            if (!ShareContext)
            {
                return Context.SaveChanges();
            } 

            return 0;
        }

        public void Dispose()
        {
            // check if we are sharing the context, if we are then we dont dispose, other repositories might need the context
            // there should be something else outisde our repositories that manage the dispoals of our context.

            if (!ShareContext && Context != null)
            {
                try
                {
                    Context.Dispose();
                }
                catch { }
            }
        }
    }
}