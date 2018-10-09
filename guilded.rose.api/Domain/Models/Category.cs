using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace guilded.rose.api.Domain.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}