using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Newtonsoft.Json;
using ShoppingCartApi.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace ShoppingCartApi.Repositories
{
    /// <summary>
    /// The interface contains methods for Operaions on Collections
    /// </summary>
    /// <typeparam name="TModel"-></typeparam>
    public interface IDbCollectionOperationsRepository<TModel, in TPk>
    {
        Task<IEnumerable<TModel>> GetItemsFromCollectionAsync();
        Task<TModel> GetItemFromCollectionAsync(TPk id);
        Task<TModel> AddDocumentIntoCollectionAsync(TModel item);
        Task<TModel> UpdateDocumentFromCollection(TPk id, TModel item);
        Task DeleteDocumentFromCollectionAsync(TPk id);
    }

    /// <summary>
    /// The following class is used to create DocumentDB, Collections if not exist
    /// </summary>
    public class DbCollectionOperationsRepository : IDbCollectionOperationsRepository<UserDetailsModel, string>
    {
        #region The DocumentDB Endpoint, Key, DatabaseId and CollectionId declaration
        private static readonly string Endpoint = "https://shoppingdbpcdp2018.documents.azure.com:443/";
        private static readonly string Key = "MlaBEKnQnXxiUt81FyldpX4x2HhnoxYkRHmUkdisxL9Ivb1dfGZ1PQ86uDMY7x1wH3m1354HkEyk4VAXkTGdAA==";
        private static readonly string DatabaseId = "shoppingcartDB";
        private static readonly string CollectionId = "UserDetails";
        private static DocumentClient docClient;
        #endregion

        public DbCollectionOperationsRepository()
        {
            docClient = new DocumentClient(new Uri(Endpoint), Key);
            CreateDatabaseIfNotExistsAsync().Wait();
            CreateCollectionIfNotExistsAsync().Wait();
        }

        #region Private methods to create Database and Collection if not Exist
        /// <summary>
        /// The following function has following steps
        /// 1. Try to read database based on the DatabaseId passed as URI link, if it is not found the execption will be thrown
        /// 2. In the execption the database will be created of which Id will be set as DatabaseId 
        /// </summary>
        /// <returns></returns>
        private static async Task CreateDatabaseIfNotExistsAsync()
        {
            try
            {
                //1. this check weather DB exist with given DataBase ID
                await docClient.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(DatabaseId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    //2. This will create DB  if not exist.
                    await docClient.CreateDatabaseAsync(new Database { Id = DatabaseId });
                }
                else
                {
                    throw;
                }
            }
        }
        /// <summary>
        /// The following function has following steps
        /// 1.Read the collection based on the DatabaseId and Collectionid passed as URI, if not found then throw exeption
        /// //2.In exception create a collection.
        /// </summary>
        /// <returns></returns>
        private static async Task CreateCollectionIfNotExistsAsync()
        {
            try
            {
                //1.
                await docClient.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    //2.
                    await docClient.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(DatabaseId),
                        new DocumentCollection { Id = CollectionId },
                        new RequestOptions { OfferThroughput = 1000 });
                }
                else
                {
                    throw;
                }
            }
        }
        #endregion

        /// <summary>
        /// The method to create a new Document in the collection 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<UserDetailsModel> AddDocumentIntoCollectionAsync(UserDetailsModel item)
        {
            try
            {
                var document = await docClient.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId), item);
                var res = document.Resource;
                var person = JsonConvert.DeserializeObject<UserDetailsModel>(res.ToString());
                return person;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to Delete document
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteDocumentFromCollectionAsync(string id)
        {
            await docClient.DeleteDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id));
        }

        /// <summary>
        /// Method to read Item from the document based on id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UserDetailsModel> GetItemFromCollectionAsync(string id)
        {
            try
            {
                Document doc = await docClient.ReadDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id));
                return JsonConvert.DeserializeObject<UserDetailsModel>(doc.ToString());
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }


        /// <summary>
        /// Method to Read all Documents from the collection
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<UserDetailsModel>> GetItemsFromCollectionAsync()
        {
            var documents = docClient.CreateDocumentQuery<UserDetailsModel>(
                  UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
                  new FeedOptions { MaxItemCount = -1 })
                  .AsDocumentQuery();
            List<UserDetailsModel> persons = new List<UserDetailsModel>();
            while (documents.HasMoreResults)
            {
                persons.AddRange(await documents.ExecuteNextAsync<UserDetailsModel>());
            }
            return persons;
        }
        /// <summary>
        /// Method to Update Document
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<UserDetailsModel> UpdateDocumentFromCollection(string id, UserDetailsModel item)
        {
            try
            {
                var document = await docClient.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id), item);
                var data = document.Resource.ToString();
                var person = JsonConvert.DeserializeObject<UserDetailsModel>(data);
                return person;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        
    }
}
