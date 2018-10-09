using System.Collections.Generic;
using System.Threading.Tasks;
using guilded.rose.api.Domain.Models;

namespace guilded.rose.api.Domain.Business.Interfaces
{
    public interface ICategoryBL
    {
        Task<List<Category>> GetCategories();
    }
}