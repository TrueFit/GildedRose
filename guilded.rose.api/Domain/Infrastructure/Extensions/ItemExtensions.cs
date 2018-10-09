

using System;
using System.Collections.Generic;
using guilded.rose.api.Domain.Business;
using guilded.rose.api.Domain.Models;
using guilded.rose.api.Domain.Models.Interfaces;

namespace guilded.rose.api.Domain.Infrastructure.Extensions
{
    public static class ItemExtensions
    {
        public static IEnumerable<IItem> ToAdjustedItems<T>(this IEnumerable<T> items, DateTime date) where T : IItem
        {
            foreach (T item in items)
            {
                yield return ItemFactory.CreateAdjustedItem(item, date);
            }

        }
    }

}