using System.Collections.Generic;
using System.Threading.Tasks;

namespace guilded.rose.api.Domain.DataAccess
{
    public interface IRepository<T>
    {
        Task<List<T>> Get();
        Task<T> GetByName(string name);
    }
}