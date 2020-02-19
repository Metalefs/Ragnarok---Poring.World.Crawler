using Domain.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Persistency.Database
{
    public class DBConnection : IWebCrawlerMongoCollection
    {
        private IMongoDatabase database; 
        private IMongoClient client = new MongoClient("mongodb://localhost");

        public DBConnection()
        {
            database = client.GetDatabase("PoringCrawler");
        }


        public async Task Inserir<T>(T document, string collection) where T : class
        {
            IMongoCollection<T> Collection = database.GetCollection<T>(collection);
            await Collection.InsertOneAsync(document);
        }

        public void InserirVarios<T>(List<T> document, string collection) where T : class
        {
            lock (this)
            {
                IMongoCollection<T> Collection = database.GetCollection<T>(collection);
                Collection.InsertMany(document);
            }
        }

        public void Editar<T>(Expression<Func<T, bool>> filter, T document, string collection) where T : class
        {
            IMongoCollection<T> Collection = database.GetCollection<T>(collection);
            Collection.ReplaceOne(filter, document);
        }

        public void Remover<T>(Expression<Func<T, bool>> filter, string collection) where T : class
        {
            IMongoCollection<T> Collection = database.GetCollection<T>(collection);
            Collection.DeleteOne<T>(filter);
        }

        public void Find<T>(Expression<Func<T, bool>> filter, string collection) where T : class
        {
            IMongoCollection<T> Collection = database.GetCollection<T>(collection);
            Collection.Find<T>(collection);
        }
    }
}
