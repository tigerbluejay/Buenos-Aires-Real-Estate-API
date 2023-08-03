using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BuenosAiresRealEstate.API.RepositoryInterfaces
{
    // here we are saying that the Repository can be of any class type
    public interface IRepository<T> where T : class
    {
        /*
        Task<List<T>>: This is the return type of the method. It indicates that the method will return a 
        Task that will eventually produce a List<T>. The Task type is often used in asynchronous programming
        to represent a computation that may complete at some point in the future.

        Expression<Func<T, bool>>? filter = null: This is the first parameter of the method. It is of type 
        Expression<Func<T, bool>>. In C#, Expression is a type that represents a strongly-typed lambda 
        expression. It can be used to build and represent expressions at runtime. Here, Func<T, bool> is 
        a delegate representing a function that takes a parameter of type T and returns a bool. 
        The parameter filter is optional, indicated by the ? after its type (Expression<Func<T, bool>>?),
        which means it can be omitted when calling the method. If not provided, it defaults to null.

        string? includeProperties = null: This is the second parameter of the method. It is a string type 
        named includeProperties, also optional due to the ? after its type (string?). It is used for 
        specifying properties to include in the query results (if supported by the implementation). 
        If not provided, it defaults to null.

        int pageSize = 0: This is the third parameter of the method. It is of type int named pageSize 
        and is used for pagination. Pagination is a technique to split large result sets into smaller,
        more manageable chunks or pages. If not provided, it defaults to 0.

        int pageNumber = 1: This is the fourth parameter of the method. It is of type int named pageNumber 
        and represents the page number of the result set. If not provided, it defaults to 1.

        To summarize, the GetAllAsync method is an asynchronous method that returns a Task<List<T>>. It 
        is designed for fetching a list of entities of type T, optionally applying a filter to the query, 
        including specific properties, and supporting pagination. The filter, includeProperties, pageSize,
        and pageNumber parameters are optional, providing flexibility in how the query results are retrieved.
        */
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null,
            /* for pagination: */ int pageSize = 0, int pageNumber = 1);
        Task<T> GetAsync(Expression<Func<T, bool>>? filter = null, bool tracked = true,
            string? includeProperties = null);
        Task CreateAsync(T entity);
        Task RemoveAsync(T entity);
        Task SaveAsync();
    }
}
