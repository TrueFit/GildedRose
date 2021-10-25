using GildedRoseAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GildedRoseAPI.Responses
{
    public class ItemResponse : GenericResponse
    {
        public Item item { get; set; }
    }
}
