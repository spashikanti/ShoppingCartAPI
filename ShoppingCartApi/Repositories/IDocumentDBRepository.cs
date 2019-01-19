using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;

namespace ShoppingCartApi.Repositories
{
    public interface IDocumentDBRepository<T> where T : class
    {
        Task<Document> CreateItemAsync(T item, string collectionId);
        Task DeleteItemAsync(string id, string collectionId);
        Task<T> GetItemAsync(string id, string collectionId);
        Task<IEnumerable<T>> GetItemsAsync(Expression<Func<T, bool>> predicate, string collectionId);
        Task<IEnumerable<T>> GetItemsAsync(string collID);
        Task<Document> UpdateItemAsync(string id, T item, string collectionId);
    }
}
