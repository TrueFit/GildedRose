using System.Collections.Generic;
using GildedRose.Persistence.Context;
using GildedRose.Persistence.Core;
using GildedRose.Domain.Actions;
using GildedRose.Models;
using GildedRose.Persistence.Actions.Items;
using GildedRose.Persistence.Actions;

namespace GildedRose.Persistence.Database
{
    public class CategoryData : IDatabase<Category>
    {
        private readonly IDataExecutor _dataExecutor;

        private readonly InventoryContext _context;

        public CategoryData(InventoryContext context)
        {
            _dataExecutor = new DataExecutor();
            _context = context;
        }

        public CategoryData(IDataExecutor dataExecutor, InventoryContext context)
        {
            _dataExecutor = dataExecutor;
            _context = context;
        }

        public void AddEntity(Category category)
        {
            _dataExecutor.ExecuteCommand(new AddCategory(_context, category));
        }

        public void DeleteEntity(int categoryId)
        {
            _dataExecutor.ExecuteCommand(new DeleteCategory(_context, categoryId));
        }

        public int GetCount()
        {
            return _dataExecutor.ExecuteQuery(new GetCategoryCount(_context));
        }

        public List<Category> GetEntities(int? categoryId = null)
        {
            return _dataExecutor.ExecuteQuery(new GetCategories(_context));
        }

        public Category GetEntity(int categoryId)
        {
            return _dataExecutor.ExecuteQuery(new GetCategory(_context, categoryId));
        }

        public Category GetEntityByName(string name)
        {
            return _dataExecutor.ExecuteQuery(new GetCategoryByName(_context, name));
        }

        public void UpdateEntity(Category category)
        {
            _dataExecutor.ExecuteCommand(new UpdateCategory(_context, category));
        }
    }
}

