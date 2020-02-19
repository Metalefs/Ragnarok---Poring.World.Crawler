using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IWebCrawlerMongoCollection
    {
        Task Inserir<T>(T entity, string collection) where T : class;
        void InserirVarios<T>(List<T> entity, string collection) where T : class;
        void Editar<T>(Expression<Func<T, bool>> filter, T document, string collection) where T : class;
        void Remover<T>(Expression<Func<T, bool>> filter, string collection) where T : class;
    }
}
