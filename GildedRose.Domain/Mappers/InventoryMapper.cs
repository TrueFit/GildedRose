using GildedRose.Domain.Models;
using GildedRose.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GildedRose.Domain.Mappers
{
    public class InventoryMappingProfile : AutoMapper.Profile
    {
        public InventoryMappingProfile()
        {
            CreateMap<Degradation, DegradationValue>();
            CreateMap<Category, CategoryValue>();
            CreateMap<Quality, QualityValue>();
            CreateMap<InventoryItem, InventoryItemValue>();
            CreateMap<Inventory, InventoryValue>();
        }
    }
}
