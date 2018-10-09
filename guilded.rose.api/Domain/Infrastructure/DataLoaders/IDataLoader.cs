using System.Collections.Generic;
using guilded.rose.api.Domain.Models;

namespace guilded.rose.api.Domain.Infrastructure.DataLoaders
{
    public interface IDataLoader
    {
        string Source { get; }

        List<Import> Build();
    }
}