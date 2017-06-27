using System;
using System.Linq;
using System.Linq.Expressions;

// repositories are containers that contain our data access code. in order the make them interchangeable we need to use an interface

namespace RibbitMvc.Data
{
    //defines a set of methods for CRUD
    public interface IRepository<T> : IDisposable where T : class
    {
        //retrieve all records within a particular table
        IQueryable<T> All();

        //determine if there any records that meet a particular criteria
        bool Any(Expression<Func<T, bool>> predicate);

        // count property to get the amount of records within the table
        int Count { get; }

        //create
        T Create(T t);

        // delete passing in an entity object
        int Delete(T t);

        //delete that accepts a predicate, so we can delete multiple records based upon a particular criteria
        int Delete(Expression<Func<T, bool>> predicate);

        //find a single record within the tabe
        T Find(params object[] keys);
        T Find(Expression<Func<T, bool>> predicate);

        //find multiple records
        IQueryable<T> FindAll(Expression<Func<T, bool>> predicate);
        //find multiple records with an index and a size, so if we needed paging we could use this method
        IQueryable<T> FindAll(Expression<Func<T, bool>> predicate, int index, int size);

        // update method, where we pass in the updated entity
        int Update(T t);        
    }
}