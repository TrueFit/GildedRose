using System;

namespace guilded.rose.api.Domain.Models.Interfaces
{
    public interface IQualityAdjuster
    {
        IItem CreateAdjustedItem(IItem item, DateTime date);
    }
}