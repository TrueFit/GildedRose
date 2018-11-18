using AutoMapper;
using GildedRose.Domain.Models;
using GildedRose.Models;
using GildedRose.Persistence.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace GildedRose.Domain
{
    public class CategoryDomain : IDomain<CategoryValue>
    {
        private CategoryData _categories;
        private IMapper _mapper;

        public CategoryDomain(IDatabase<Category> categories, IMapper mapper)
        {
            _mapper = mapper;
            _categories = (CategoryData)categories;
        }

        public void AddCategory(CategoryValue categoryValue)
        {
            var category = _mapper.Map<Category>(categoryValue);
            var existingCategory = _categories.GetEntityByName(categoryValue.Name);
            if (existingCategory == null)
            {
                _categories.AddEntity(category);
            }
        }

        public IEnumerable<CategoryValue> GetCategories()
        {
            return _mapper.Map<List<CategoryValue>>(_categories.GetEntities(null));
        }

        public CategoryValue GetCategory(int id)
        {
            return _mapper.Map<CategoryValue>(_categories.GetEntity(id));
        }

        public void DeleteCategory(int id)
        {
            _categories.DeleteEntity(id);
        }

    }
}
