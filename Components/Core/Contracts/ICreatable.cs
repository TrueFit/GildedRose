using System;
using System.Collections.Generic;
using System.Text;

namespace GildedRose.Core.Contracts
{
    public interface ICreatable
    {
        DateTime Created { get; set; }

        int CreatedBy { get; set; }
    }
}
