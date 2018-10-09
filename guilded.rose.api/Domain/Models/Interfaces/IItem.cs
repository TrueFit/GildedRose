using System;

namespace guilded.rose.api.Domain.Models.Interfaces
{
    public interface IItem
    {
        int Id { get; set; }
        string Name { get; set; }
        int CategoryId { get; set; }
        int SellIn { get; set; }
        int Quality { get; set; }
        DateTime DateCreated { get; set; }

        Category Category { get; set; }
    }
}