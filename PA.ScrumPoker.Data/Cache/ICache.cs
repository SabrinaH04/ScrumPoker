using PA.ScrumPoker.Data.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace PA.ScrumPoker.Data.Cache
{
    public interface ICache<T> where T : IAmADto
    {
        IEnumerable<T> GetCache(string cacheKey);
        void SetCache(string cacheKey, IEnumerable<T> sourceEntity);
        void RemoveCache(string cacheKey);
    }
}
